using System;
using System.Collections.Generic;

namespace Okiroya.Campione.DataAccess.Sql
{
    public class BaseSqlUtilityResult
    {
        public ICollection<DataItem> ReturnedData { get; private set; }

        public IDictionary<string, object> OutputParameters { get; private set; }

        public BaseSqlUtilityResult(ICollection<DataItem> returnedData, IDictionary<string, object> outputParameters)
        {
            ReturnedData = returnedData;

            OutputParameters = outputParameters;
        }
    }
}
