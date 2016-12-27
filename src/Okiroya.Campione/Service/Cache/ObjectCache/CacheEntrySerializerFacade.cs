using Okiroya.Campione.SystemUtility;
using System;
using System.Collections.Concurrent;

namespace Okiroya.Campione.Service.Cache
{
    public static class CacheEntrySerializerFacade
    {
        private static readonly ConcurrentDictionary<string, CacheEntrySerializer> _knownSerializers = new ConcurrentDictionary<string, CacheEntrySerializer>();

        public static byte[] Serialize(string entityType, object entity)
        {
            Guard.ArgumentNotEmpty(entityType);
            Guard.ArgumentNotNull(entity);

            CacheEntrySerializer serializer = null;
            if (!_knownSerializers.TryGetValue(entityType, out serializer))
            {
                throw new NotSupportedException(string.Concat("Сериализатор для типа ", entityType, " не зарегистрирован"));
            }

            return serializer.Serialize(entityType, entity);
        }

        public static object Deserialize(string entityType, byte[] body)
        {
            Guard.ArgumentNotEmpty(entityType);
            Guard.ArgumentNotNull(body);

            CacheEntrySerializer serializer = null;
            if (!_knownSerializers.TryGetValue(entityType, out serializer))
            {
                throw new NotSupportedException(string.Concat("Сериализатор для типа ", entityType, " не зарегистрирован"));
            }

            return serializer.Deserialize(entityType, body);
        }

        public static void RegisterSerializer<T>(CacheEntrySerializer<T> serializer)
        {
            Guard.ArgumentNotNull(serializer);

            _knownSerializers.TryAdd(serializer.EntryType, serializer);
        }
    }
}
