using System;
using System.Collections.Generic;

namespace Okiroya.Campione.Configuration.DI
{
    public class RegisterDependencyContainerOptions
    {
        public IEnumerable<RegisterDependencyContainerElement> Dependencies { get; set; }

        public RegisterDependencyContainerOptions()
        { }
    }
}
