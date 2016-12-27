using System;

namespace Okiroya.Campione.DataAccess.MsSql
{
    public sealed class MsSqlUtilitySettings
    {
        public bool RetrieveStatistics { get; set; }

        public bool GetInfoMessages { get; set; }

        public static MsSqlUtilitySettings Default
        {
            get
            {
                return new MsSqlUtilitySettings
                {
                    RetrieveStatistics = false,
                    GetInfoMessages = false
                };
            }
        }
    }
}
