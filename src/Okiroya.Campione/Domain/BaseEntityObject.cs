using System;

namespace Okiroya.Campione.Domain
{
    /// <summary>
    /// Базовый объект в системе. Также является объектом "супертип слоя"
    /// </summary>
    
    public abstract class BaseEntityObject<T> : IEntityObject<T> where T : IComparable<T>, IEquatable<T>
    {
        private string _objectTypeSysName;
        
        #region IEntityObject

        /// <summary>
        /// Идентификатор сущности
        /// </summary>
        public T Id { get; set; }

        /// <summary>
        /// Системное имя типа сущности
        /// </summary>
        public virtual string EntityTypeSysName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_objectTypeSysName))
                {
                    _objectTypeSysName = GetType().FullName;
                }

                return _objectTypeSysName;
            }
            set
            {
                _objectTypeSysName = value;
            }
        }

        #endregion
    }
}
