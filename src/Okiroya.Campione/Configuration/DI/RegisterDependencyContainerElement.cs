using System;

namespace Okiroya.Campione.Configuration.DI
{
    public class RegisterDependencyContainerElement
    {
        public string Name { get; set; }

        public string DependencyType { get; set; }

        public string ServiceType { get; set; }

        public bool Enabled { get; set; }

        public RegisterDependencyContainerElement()
        { }
    }
}
