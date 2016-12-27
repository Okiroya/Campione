using System;

namespace Okiroya.Campione.Tests.Internal
{
    internal class DependencyTestService<T> : IDependencyTestInterface<T>
    {
        public T Val { get; set; }
    }
}
