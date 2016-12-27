using System;

namespace Okiroya.Campione.Service.Cache
{
    public abstract class CacheEntrySerializer
    {
        public abstract string EntryType { get; }

        public abstract byte[] Serialize(string entityType, object entity);

        public abstract object Deserialize(string entityType, byte[] body);
    }

    public abstract class CacheEntrySerializer<T> : CacheEntrySerializer
    {
        public abstract byte[] Serialize(T entity);

        public abstract T Deserialize(byte[] body);

        public override byte[] Serialize(string entityType, object entity)
        {
            try
            {
                return Serialize((T)entity);
            }
            catch (InvalidCastException)
            {
                throw new ArgumentException(string.Concat("Тип сериализуемой сущности ", typeof(T), " не совпадает с указанным типом ", entityType), "entity");
            }
        }

        public override object Deserialize(string entityType, byte[] body)
        {
            return Deserialize(body);
        }
    }
}
