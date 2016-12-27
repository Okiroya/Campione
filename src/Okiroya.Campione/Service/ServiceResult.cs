using System;
using System.Collections.Generic;
using Okiroya.Campione.Domain;

namespace Okiroya.Campione.Service
{
    /// <summary>
    /// Контейнер, содержащий объекты динамического типа, наследующего IEntityObject, сконструированные по прочитанным данным, и выходные параметры.
    /// </summary>
    
    public class ServiceResult : ServiceResult<IEntityObject<int>, int>
    { }

    /// <summary>
    /// Контейнер, содержащий объекты, сконструированные по прочитанным данным и выходные параметры. Используется сервисом работы с данными
    /// </summary>
    /// <typeparam name="TResult">Тип объектов в контейнере - задает тип объектов в контейнере</typeparam>
    /// <typeparam name="TKey">Тип идентификатора объекта</typeparam>    
    public class ServiceResult<TResult, TKey>
        where TResult : IEntityObject<TKey>
        where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        /// <summary>
        /// Список объектов
        /// </summary>
        public IEnumerable<TResult> DataResult { get; set; }

        /// <summary>
        /// Выходные параметры
        /// </summary>
        public IDictionary<string, object> OutParameters { get; set; }
    }
}
