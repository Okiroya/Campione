using System;
using System.Threading;

namespace Okiroya.Campione.SystemUtility
{
    /// <summary>
    /// "Облегченная" версия синхронизатора потоков Monitor
    /// </summary>
    /// <remarks>
    /// Увеличение быстродействия достигнуто за счет использования конструкции пользовательского режима для одного конкурирующего потока.
    /// Если конкурирующих потоков несколько, то используется конструкция режима ядра, которая на порядок менее производительная чем конструкция пользовательского режима
    /// </remarks>
    public sealed class HybridLock : IDisposable
    {
        private int _waiters = 0;
        private AutoResetEvent _waiterLock = new AutoResetEvent(false);        

        public void Enter()
        {
            if (Interlocked.Increment(ref _waiters) == 1)
            {
                return;
            }

            _waiterLock.WaitOne();
        }

        public void Leave()
        {
            if (Interlocked.Decrement(ref _waiters) == 0)
            {
                return;
            }

            _waiterLock.Set();
        }

        public void Dispose()
        {
            _waiterLock.Dispose();
        }
    }
}
