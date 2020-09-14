using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BergerMsfaApi.Attributes;
using BergerMsfaApi.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BergerMsfaApi.Extensions
{
    public static class EfCoreExtensions
    {
        #region Convert UniqueKeyAttribute on Entities to UniqueKey in DB
        private static IEnumerable<UniqueKeyAttribute> GetUniqueKeyAttributes(IMutableEntityType entityType, IMutableProperty property)
        {
            if (entityType == null)
            {
                throw new ArgumentNullException(nameof(entityType));
            }
            else if (entityType.ClrType == null)
            {
                throw new ArgumentNullException(nameof(entityType.ClrType));
            }
            else if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }
            else if (property.Name == null)
            {
                throw new ArgumentNullException(nameof(property.Name));
            }
            var propInfo = entityType.ClrType.GetProperty(
                property.Name,
                BindingFlags.NonPublic |
                BindingFlags.Public |
                BindingFlags.Static |
                BindingFlags.Instance |
                BindingFlags.DeclaredOnly);
            if (propInfo == null)
            {
                return null;
            }
            return propInfo.GetCustomAttributes<UniqueKeyAttribute>();
        }

        public static void BuildUniqueKey(this ModelBuilder modelBuilder)
        {
            // Iterate through all EF Entity types
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var properties = entityType.GetProperties();
                if ((properties != null) && (properties.Any()))
                {
                    foreach (var property in properties)
                    {
                        var uniqueKeys = GetUniqueKeyAttributes(entityType, property);
                        if (uniqueKeys != null)
                        {
                            foreach (var uniqueKey in uniqueKeys.Where(x => x.Order == 0))
                            {
                                // Single column Unique Key
                                if (String.IsNullOrWhiteSpace(uniqueKey.GroupId))
                                {
                                    entityType.AddIndex(property).IsUnique = true;
                                }
                                // Multiple column Unique Key
                                else
                                {
                                    var mutableProperties = new List<IMutableProperty>();
                                    properties.ToList().ForEach(x =>
                                    {
                                        var uks = GetUniqueKeyAttributes(entityType, x);
                                        if (uks != null)
                                        {
                                            foreach (var uk in uks)
                                            {
                                                if ((uk != null) && (uk.GroupId == uniqueKey.GroupId))
                                                {
                                                    mutableProperties.Add(x);
                                                }
                                            }
                                        }
                                    });
                                    entityType.AddIndex(mutableProperties).IsUnique = true;
                                }
                            }
                        }
                    }
                }

            }
        }
        #endregion Convert UniqueKeyAttribute on Entities to UniqueKey in DB

        public static void BuildDataBase(this ModelBuilder modelBuilder)
        {
            var assList = AppDomain.CurrentDomain.GetAssemblies()
               .Where(s => s.FullName.Contains("FMAplication"))
               .ToList();
            var builder = typeof(ModelBuilder).GetMethod("Entity");

            List<Type> types;


            if (AppIdentity.IsMigrationEnable)
            {
                types = assList.SelectMany(assembly => assembly.GetTypes()
                        .Where(t => typeof(IBaseEntity).IsAssignableFrom(t)
                                    && !t.IsDefined(typeof(IgnoreEntityAttribute), false)
                                    && !t.IsDefined(typeof(IgnoreMigrationAttribute), false)))
                    .ToList();
            }
            else
            {
                types = assList.SelectMany(assembly => assembly.GetTypes()
                        .Where(t => typeof(IBaseEntity).IsAssignableFrom(t)
                                    && !t.IsDefined(typeof(IgnoreEntityAttribute), false)))
                    .ToList();
            }

            types.ForEach(type =>
            {
                if (builder != null)
                    builder.MakeGenericMethod(type)
                        .Invoke(modelBuilder, new object[] { });
            });
        }

    }
}
