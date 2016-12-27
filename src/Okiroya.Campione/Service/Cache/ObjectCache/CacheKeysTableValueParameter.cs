using Okiroya.Campione.DataAccess;
using System;
using System.Collections.Generic;

namespace Okiroya.Campione.Service.Cache
{
    public class CacheKeysTableValueParameter : TableValueParameter<long>
    {
        private static Dictionary<string, string> CacheKeysTableValueParameterColumnMapping = new Dictionary<string, string> { { "CacheKey", "CacheKey" } };

        public CacheKeysTableValueParameter()
            : base(CacheKeysTableValueParameterColumnMapping)
        { }
    }
}
