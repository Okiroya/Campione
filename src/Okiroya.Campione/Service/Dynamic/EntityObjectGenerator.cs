using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;
using Okiroya.Campione.Domain;
using Okiroya.Campione.DataAccess;
using Okiroya.Campione.SystemUtility.DI;
using Okiroya.Campione.SystemUtility.FastMember;

namespace Okiroya.Campione.Service.Dynamic
{
    /// <summary>
    /// Динамический билдер сущностей по их мета информации
    /// </summary>
    public static class EntityObjectGenerator<TKey>
        where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        private static ConcurrentDictionary<string, Type> _cachedTypes = new ConcurrentDictionary<string, Type>();
        private static ConcurrentDictionary<Type, Tuple<Func<Dictionary<string, object>, IEntityObject<TKey>>, Func<string, string>>> _cachedCreator = new ConcurrentDictionary<Type, Tuple<Func<Dictionary<string, object>, IEntityObject<TKey>>, Func<string, string>>>();

        /// <summary>
        /// Сгенерировать сущность определенного типа по мета информации
        /// </summary>
        /// <param name="entityTypeName">Имя типа сущности</param>
        /// <param name="metas">Набор мета информации</param>
        /// <returns>Экземпляр динамического типа</returns>
        public static IEntityObject<TKey> CreateEntityObjectFromMeta(string entityTypeName, IEnumerable<DataItemElement> metas)
        {
            IEntityObject<TKey> result = null;

            if ((metas != null) && metas.Any())
            {
                var objectType = TryGetOrGenerateDynamicType(entityTypeName, metas.ToDictionary(p => p.Item1, p => p.Item2));

                if (objectType != null)
                {
                    result = PopulateEntity(objectType, metas.ToDictionary(p => p.Item1, p => p.Item3)); //entityTypeName
                }
            }

            return result;
        }

        /// <summary>
        /// Сгенерировать сущность определенного типа по мета информации
        /// </summary>
        /// <typeparam name="TResult">Тип сущности</typeparam>
        /// <param name="metas">Набор мета информации</param>
        /// <returns>Экземпляр статического типа</returns>
        public static TResult CreateEntityObjectFromMeta<TResult>(IEnumerable<DataItemElement> metas)
            where TResult : class, IEntityObject<TKey>
        {
            return (TResult)PopulateEntity(typeof(TResult), metas.ToDictionary(p => p.Item1, p => p.Item3));
        }

        /// <summary>
        /// Сгенерировать сущность заданного типа, привести ее к определенному типу и установить ее свойства
        /// </summary>
        /// <typeparam name="TResult">Тип сущности</typeparam>
        /// <param name="sourceType">Тип объекта</param>
        /// <param name="properties">Набор свойств</param>
        /// <returns>Экземпляр статического типа</returns>
        public static TResult CreateEntityObjectFromMeta<TResult>(Type sourceType, Dictionary<string, object> properties)
            where TResult : class, IEntityObject<TKey>
        {
            return (TResult)PopulateEntity(sourceType, properties);
        }

        private static Type TryGetOrGenerateDynamicType(string entityTypeName, IDictionary<string, Type> propertyDefinitions)
        {
            Type result = null;

            if (!_cachedTypes.TryGetValue(entityTypeName, out result))
            {
                result = AssemblyWrapper.GenerateType(
                    entityTypeName,
                    propertyDefinitions,
                    new Type[] { typeof(IEntityObject<TKey>) },
                    typeof(BaseEntityObject<TKey>));

                if (result != null)
                {
                    _cachedTypes.TryAdd(entityTypeName, result);
                }
            }

            return result;
        }

        private static IEntityObject<TKey> PopulateEntity(Type genType, Dictionary<string, object> metas, string entityTypeName = null)
        {
            Tuple<Func<Dictionary<string, object>, IEntityObject<TKey>>, Func<string, string>> creator = null;
            Func<Dictionary<string, object>, IEntityObject<TKey>> evaluator = null;
            Func<string, string> nameResolver = null;

            if (!_cachedCreator.TryGetValue(genType, out creator))
            {
                var baseObjectType = typeof(BaseEntityObject<TKey>);

                var properties = genType
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty)
                    .Where(q => q.CanWrite)
                    .Where(q => !q.DeclaringType.Equals(baseObjectType));

                var list = new List<MemberBinding>();

                IDataServiceMapper dataMapper = null;

                if (RegisterDependencyContainer<IDataServiceMapper>.IsRegisteredFor(genType))
                {
                    dataMapper = RegisterDependencyContainer<IDataServiceMapper>.ResolveFor(genType);
                }

                nameResolver =
                    q =>
                    {
                        if (dataMapper != null)
                        {
                            q = dataMapper.MapFromDomainNameToRepositoryOne(q);
                        }

                        return q;
                    };

                Expression call = null;
                var p = Expression.Parameter(typeof(Dictionary<string, object>), "p");
                foreach (var item in properties)
                {
                    call = Expression.Call(
                        typeof(DictionaryExtensions),
                        "GetValue",
                        new[] { item.PropertyType },
                        new Expression[]
                    {
                        p,
                        Expression.Constant(nameResolver(item.Name))
                    });

                    list.Add(Expression.Bind(item, call));
                }

                evaluator = Expression.Lambda<Func<Dictionary<string, object>, IEntityObject<TKey>>>(
                    Expression.MemberInit(Expression.New(genType), list),
                    new[] { p })
                .Compile();

                _cachedCreator.TryAdd(genType, Tuple.Create(evaluator, nameResolver));
            }
            else
            {
                evaluator = creator.Item1;
                nameResolver = creator.Item2;
            }

            var result = evaluator(metas);

            var idPropertyName = nameResolver("Id");
            if ((result != null) && metas.Any(p => p.Key.Equals(idPropertyName, StringComparison.OrdinalIgnoreCase)))
            {
                result.Id = (TKey)metas.FirstOrDefault(p => p.Key.Equals(idPropertyName, StringComparison.OrdinalIgnoreCase)).Value;
            }

            if (!string.IsNullOrWhiteSpace(entityTypeName))
            {
                result.EntityTypeSysName = entityTypeName;
            }

            return result;
        }
    }
}
