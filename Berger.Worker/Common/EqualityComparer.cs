using System;
using System.Collections.Generic;
using System.Text;
using Berger.Data.MsfaEntity.SAPTables;

namespace Berger.Worker.Common
{
    public class EqualityComparer<T, TKey>: IEqualityComparer<T>
    {
        private readonly Func<T, TKey> _keyFunction;

        public EqualityComparer(Func<T, TKey> keyFunction)
        {
            _keyFunction = keyFunction;
        }
        public bool Equals(T x, T y) => EqualityComparer<TKey>.Default.Equals(_keyFunction(x), _keyFunction(y));

        public int GetHashCode(T obj) => EqualityComparer<TKey>.Default.GetHashCode(_keyFunction(obj));
    }
}
