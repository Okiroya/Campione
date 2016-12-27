using System;

namespace Okiroya.Campione.Domain
{
    /// <summary>
    /// Контракт для сущности в системе. Все объекты в системы являются наследниками данного контракта
    /// </summary>
    public interface IEntityObject<T> where T : IComparable<T>, IEquatable<T>
    {
        /// <summary>
        /// Идентификатор сущности
        /// </summary>
        T Id { get; set; }

        /// <summary>
        /// Системное имя типа сущности
        /// </summary>
        string EntityTypeSysName { get; set; }
    }
}
