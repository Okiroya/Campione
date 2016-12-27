using System;
using System.Diagnostics;

namespace Okiroya.Campione.SystemUtility
{
    /// <summary>
    /// Методы-расширения для Stopwatch
    /// </summary>
    public static class StopwatchExtensions
    {
        /// <summary>
        /// Вернуть затраченное кол-во секунд
        /// </summary>
        /// <param name="stopWatch"></param>
        /// <returns></returns>
        public static double TotalSeconds(this Stopwatch stopWatch)
        {
            Guard.ArgumentNotNull(stopWatch);

            double milliseconds = stopWatch.Elapsed.TotalMilliseconds;

            return milliseconds > 0 ?
                Math.Round((milliseconds) / 1000, 4, MidpointRounding.AwayFromZero) :
                0;
        }

        /// <summary>
        /// Вернуть строковое представление затраченного кол-ва секунд
        /// </summary>
        /// <param name="stopWatch"></param>
        /// <param name="resultFormat"></param>
        /// <returns></returns>
        public static string TotalSecondsAsString(this Stopwatch stopWatch, string resultFormat = "0.####")
        {
            Guard.ArgumentNotNull(stopWatch);
            Guard.ArgumentNotEmpty(resultFormat);
            
            return TotalSeconds(stopWatch).ToString(resultFormat);
        }
    }
}
