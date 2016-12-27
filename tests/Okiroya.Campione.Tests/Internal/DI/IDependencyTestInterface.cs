using System;

namespace Okiroya.Campione.Tests.Internal
{
    internal interface IDependencyTestInterface<T>
    {
        T Val { get; set; }
    }
}
