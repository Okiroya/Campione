using Okiroya.Campione.DataAccess.Sql;
using System;
using System.Collections.Generic;

namespace Okiroya.Campione.DataAccess.MsSql
{
    public class MsSqlUtilityResult : BaseSqlUtilityResult
    {
        public MsSqlUtilityStatisticsResult Statistics { get; private set; }

        public IEnumerable<MsSqlUtilityInfoMessageResult> InfoMessage { get; private set; }

        public MsSqlUtilityResult(
            ICollection<DataItem> returnedData,
            IDictionary<string, object> outputParameters,
            MsSqlUtilityStatisticsResult statistics,
            IEnumerable<MsSqlUtilityInfoMessageResult> infoMessage) : base(returnedData, outputParameters)
        {
            Statistics = statistics;

            InfoMessage = infoMessage;
        }
    }
}
