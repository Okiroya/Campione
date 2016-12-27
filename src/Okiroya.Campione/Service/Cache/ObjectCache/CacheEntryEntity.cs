using Okiroya.Campione.Domain;
using System;

namespace Okiroya.Campione.Service.Cache
{
    
    public class CacheEntryEntity : Int64EntityObject
    {
        public long UtcCreated { get; set; }

        public long? UtcLastAccessed { get; set; }

        public long UtcAbsoluteExpiration { get; set; }        

        public int SlidingExpiration { get; set; }

        public bool NotRemovable { get; set; }

        public string EntryType { get; set; }

        public byte[] Entry { get; set; }
    }
}
