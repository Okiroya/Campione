using System;
using System.Diagnostics;
using Okiroya.Campione.SystemUtility;
using Okiroya.Campione.DataAccess;
using Okiroya.Campione.Service.Logging;

namespace Okiroya.Campione.Service
{
    /// <summary>
    /// Фасад для безопасного выполнения делегатов
    /// </summary>
    public static class ManagedExecutionHelper
    {
        /// <summary>
        /// Выполнить делегат типа Action
        /// </summary>
        /// <exception cref="EntityServiceException">При ошибке генерируется ошибка сервиса</exception>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="action"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="methodName"></param>
        internal static void TryExecuteAction<T1, T2>(Action<T1, T2> action, T1 arg1, T2 arg2, string methodName = null)
        {
            try
            {
                TraceCall(Ext<T1, T2, int, int>(Curring<T1, T2, int>(action)), arg1, arg2, 0, methodName);
            }
            catch (Exception ex)
            {
                throw new EntityServiceException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Выполнить делегат типа Func
        /// </summary>
        /// <exception cref="EntityServiceException">При ошибке генерируется ошибка сервиса</exception>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="methodName"></param>
        /// <returns></returns>
        internal static TResult TryExecuteFunc<T1, T2, TResult>(Func<T1, T2, TResult> func, T1 arg1, T2 arg2, string methodName = null)
        {
            try
            {
                return TraceCall(Ext<T1, T2, int, TResult>(func), arg1, arg2, 0, methodName);
            }
            catch (Exception ex)
            {
                throw new EntityServiceException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Выполнить делегат типа Func
        /// </summary>
        /// <exception cref="EntityServiceException">При ошибке генерируется ошибка сервиса</exception>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        /// <param name="methodName"></param>
        /// <returns></returns>
        internal static TResult TryExecuteFunc<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> func, T1 arg1, T2 arg2, T3 arg3, string methodName = null)
        {
            try
            {
                return TraceCall(func, arg1, arg2, arg3, methodName);
            }
            catch (Exception ex)
            {
                throw new EntityServiceException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Каррирование делегата "действие" - преображение его в в делегата "функцию"
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="action"></param>
        /// <returns></returns>
        public static Func<T1, T2, TResult> Curring<T1, T2, TResult>(Action<T1, T2> action)
        {
            Guard.ArgumentNotNull(action);

            return (p, q) =>
            {
                action(p, q);

                return default(TResult);
            };
        }

        /// <summary>
        /// Расширение функции на один аргумент
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public static Func<T1, T2, T3, TResult> Ext<T1, T2, T3, TResult>(Func<T1, T2, TResult> func)
        {
            Guard.ArgumentNotNull(func);

            return (p, q, r) =>
            {
                return func(p, q);
            };
        }

        /// <summary>
        /// Выполнить действие, при этом записать в лог время выполнения и ошибку, если она возникнет
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        /// <param name="methodName"></param>
        /// <returns></returns>
        private static TResult TraceCall<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> func, T1 arg1, T2 arg2, T3 arg3, string methodName = null)
        {
            TResult result = default(TResult);

            methodName = methodName ?? "undefined";

            try
            {
                var timer = new Stopwatch();

                timer.Restart();

                result = func(arg1, arg2, arg3);

                timer.Stop();

                string timerData = timer.TotalSecondsAsString();

                LoggingFacade.GetInstance().Log(
                    new TraceLogEntry
                    {
                        Category = "Время исполнения",
                        Message = string.Concat("Метод ", methodName, " выполнился за ", timerData, " сек"),
                        MethodName = methodName
                    });
            }
            catch (Exception ex)
            {
                LoggingFacade.GetInstance().Log(new ErrorLogEntry(ex));

                throw;
            }

            return result;
        }
    }
}
