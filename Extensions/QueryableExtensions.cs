using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace BergerMsfaApi.Extensions
{
    public static class QueryableExtensions
    {
        public static ProjectionExpression<TSource> NewModel<TSource>(this IQueryable<TSource> source)
        {
            return new ProjectionExpression<TSource>(source);
        }

        /*
         var matches = ctx.People.Where( 
          BuildOrExpression<People, string>( 
           p => p.Firstname, names 
        ));         
         */
        public static Expression<Func<TElement, bool>> BuildOrExpression<TElement, TValue>(
            Expression<Func<TElement, TValue>> valueSelector,
            IEnumerable<TValue> values
        )
        {
            if (null == valueSelector)
                throw new ArgumentNullException("valueSelector");


            if (null == values)
                throw new ArgumentNullException("values");


            ParameterExpression p = valueSelector.Parameters.Single();


            if (!values.Any())
                return e => false;


            var equals = values.Select(value =>
                (Expression)Expression.Equal(
                    valueSelector.Body,
                    Expression.Constant(
                        value,
                        typeof(TValue)
                    )
                )
            );

            var body = equals.Aggregate<Expression>(
                (accumulate, equal) => Expression.Or(accumulate, equal)
            );

            return Expression.Lambda<Func<TElement, bool>>(body, p);
        }

        public static IQueryable<T> Like<T>(this IQueryable<T> source, string propertyName, string keyword)
        {
            var type = typeof(T);
            var parameter = Expression.Parameter(type, "param");
            var memberAccess = Expression.MakeMemberAccess(parameter, type.GetProperty(propertyName));

            var constant = Expression.Constant("%" + keyword + "%");
            var contains = memberAccess.Type.GetMethod("Contains");

            var methodExp = Expression.Call(memberAccess, contains, Expression.Constant(keyword));
            var lambda = Expression.Lambda<Func<T, bool>>(methodExp, parameter);
            return source.Where(lambda);
        }

        public static IQueryable<T> ReduceConstPredicates<T>(this IQueryable<T> source)
        {
            var visitor = new ConstPredicateReducer();
            var expression = visitor.Visit(source.Expression);
            if (expression != source.Expression)
                return source.Provider.CreateQuery<T>(expression);
            return source;
        }

    }

    public class ProjectionExpression<TSource>
    {
        private static readonly Dictionary<string, Expression> ExpressionCache = new Dictionary<string, Expression>();

        private readonly IQueryable<TSource> _source;

        public ProjectionExpression(IQueryable<TSource> source)
        {
            _source = source;
        }

        public IQueryable<TDest> ToMap<TDest>()
        {
            var queryExpression = GetCachedExpression<TDest>() ?? BuildExpression<TDest>();

            return _source.Select(queryExpression);
        }

        private static Expression<Func<TSource, TDest>> GetCachedExpression<TDest>()
        {
            var key = GetCacheKey<TDest>();

            return ExpressionCache.ContainsKey(key) ? ExpressionCache[key] as Expression<Func<TSource, TDest>> : null;
        }

        private static Expression<Func<TSource, TDest>> BuildExpression<TDest>()
        {
            var sourceProperties = typeof(TSource).GetProperties();
            var destinationProperties = typeof(TDest).GetProperties().Where(dest => dest.CanWrite);
            var parameterExpression = Expression.Parameter(typeof(TSource), "src");

            var bindings = destinationProperties
                                .Select(destinationProperty => BuildBinding(parameterExpression, destinationProperty, sourceProperties))
                                .Where(binding => binding != null);

            var expression = Expression.Lambda<Func<TSource, TDest>>(Expression.MemberInit(Expression.New(typeof(TDest)), bindings), parameterExpression);

            var key = GetCacheKey<TDest>();

            ExpressionCache.Add(key, expression);

            return expression;
        }

        private static MemberAssignment BuildBinding(Expression parameterExpression, MemberInfo destinationProperty, IEnumerable<PropertyInfo> sourceProperties)
        {
            var sourceProperty = sourceProperties.FirstOrDefault(src => src.Name == destinationProperty.Name);

            if (sourceProperty != null)
            {
                return Expression.Bind(destinationProperty, Expression.Property(parameterExpression, sourceProperty));
            }

            var propertyNames = SplitCamelCase(destinationProperty.Name);

            if (propertyNames.Length == 2)
            {
                sourceProperty = sourceProperties.FirstOrDefault(src => src.Name == propertyNames[0]);

                if (sourceProperty != null)
                {
                    var sourceChildProperty = sourceProperty.PropertyType.GetProperties().FirstOrDefault(src => src.Name == propertyNames[1]);

                    if (sourceChildProperty != null)
                    {
                        return Expression.Bind(destinationProperty, Expression.Property(Expression.Property(parameterExpression, sourceProperty), sourceChildProperty));
                    }
                }
            }

            return null;
        }

        private static string GetCacheKey<TDest>()
        {
            return string.Concat(typeof(TSource).FullName, typeof(TDest).FullName);
        }

        private static string[] SplitCamelCase(string input)
        {
            return Regex.Replace(input, "([A-Z])", " $1", RegexOptions.Compiled).Trim().Split(' ');
        }

    }

    class ConstPredicateReducer : ExpressionVisitor
    {
        int evaluateConst;
        private ConstantExpression TryEvaluateConst(Expression node)
        {
            evaluateConst++;
            try { return Visit(node) as ConstantExpression; }
            finally { evaluateConst--; }
        }
        protected override Expression VisitConditional(ConditionalExpression node)
        {
            var testConst = TryEvaluateConst(node.Test);
            if (testConst != null)
                return Visit((bool)testConst.Value ? node.IfTrue : node.IfFalse);
            return base.VisitConditional(node);
        }
        protected override Expression VisitBinary(BinaryExpression node)
        {
            if (node.Type == typeof(bool))
            {
                var leftConst = TryEvaluateConst(node.Left);
                var rightConst = TryEvaluateConst(node.Right);
                if (leftConst != null || rightConst != null)
                {
                    if (node.NodeType == ExpressionType.AndAlso)
                    {
                        if (leftConst != null) return (bool)leftConst.Value ? (rightConst ?? Visit(node.Right)) : Expression.Constant(false);
                        return (bool)rightConst.Value ? Visit(node.Left) : Expression.Constant(false);
                    }
                    else if (node.NodeType == ExpressionType.OrElse)
                    {

                        if (leftConst != null) return !(bool)leftConst.Value ? (rightConst ?? Visit(node.Right)) : Expression.Constant(true);
                        return !(bool)rightConst.Value ? Visit(node.Left) : Expression.Constant(true);
                    }
                    else if (leftConst != null && rightConst != null)
                    {
                        var result = Expression.Lambda<Func<bool>>(Expression.MakeBinary(node.NodeType, leftConst, rightConst)).Compile().Invoke();
                        return Expression.Constant(result);
                    }
                }
            }
            return base.VisitBinary(node);
        }
        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (evaluateConst > 0)
            {
                var objectConst = node.Object != null ? TryEvaluateConst(node.Object) : null;
                if (node.Object == null || objectConst != null)
                {
                    var arguments = new object[node.Arguments.Count];
                    bool canEvaluate = true;
                    for (int i = 0; i < arguments.Length; i++)
                    {
                        var argumentConst = TryEvaluateConst(node.Arguments[i]);
                        if (canEvaluate = (argumentConst != null))
                            arguments[i] = argumentConst.Value;
                        else
                            break;
                    }
                    if (canEvaluate)
                    {
                        var result = node.Method.Invoke(objectConst != null ? objectConst.Value : null, arguments);
                        return Expression.Constant(result, node.Type);
                    }
                }
            }
            return base.VisitMethodCall(node);
        }
        protected override Expression VisitUnary(UnaryExpression node)
        {
            if (evaluateConst > 0 && (node.NodeType == ExpressionType.Convert || node.NodeType == ExpressionType.ConvertChecked))
            {
                var operandConst = TryEvaluateConst(node.Operand);
                if (operandConst != null)
                {
                    var result = Expression.Lambda(node.Update(operandConst)).Compile().DynamicInvoke();
                    return Expression.Constant(result, node.Type);
                }
            }
            return base.VisitUnary(node);
        }
        protected override Expression VisitMember(MemberExpression node)
        {
            object value;
            if (evaluateConst > 0 && TryGetValue(node, out value))
                return Expression.Constant(value, node.Type);
            return base.VisitMember(node);
        }
        static bool TryGetValue(MemberExpression me, out object value)
        {
            object source = null;
            if (me.Expression != null)
            {
                if (me.Expression.NodeType == ExpressionType.Constant)
                    source = ((ConstantExpression)me.Expression).Value;
                else if (me.Expression.NodeType != ExpressionType.MemberAccess
                    || !TryGetValue((MemberExpression)me.Expression, out source))
                {
                    value = null;
                    return false;
                }
            }
            if (me.Member is PropertyInfo)
                value = ((PropertyInfo)me.Member).GetValue(source);
            else
                value = ((FieldInfo)me.Member).GetValue(source);
            return true;
        }
    }
}
