using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Collections.Generic;
using System.Globalization;

namespace Okiroya.Campione.SystemUtility.FastMember
{
    /// <summary>
    /// Утилита генерирования динамических типов
    /// </summary>
    internal static class AssemblyWrapper
    {
        private static AssemblyBuilder _assemblyBuilder;
        private static ModuleBuilder _moduleBuilder;

        static AssemblyWrapper()
        {
            var assemblyName = new AssemblyName("Okiroya.Campione.SystemUtility.FastMember.DynamicPlus");
            
            _assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndCollect);

            _moduleBuilder = _assemblyBuilder.DefineDynamicModule(_assemblyBuilder.GetName().Name);
        }

        /// <summary>
        /// Сгенерировать тип на основе передаваемых свойств
        /// </summary>
        /// <param name="name">Имя типа</param>
        /// <param name="propertyDefinitions">Словарь значений "наименование свойства - тип свойства"</param>
        /// <param name="interfaceTypes">Список интерфейсов, которые должен наследовать тип</param>
        /// <param name="parent">Тип родительского класса</param>
        /// <returns></returns>
        public static Type GenerateType(string name, IDictionary<string, Type> propertyDefinitions, Type[] interfaceTypes = null, Type parent = null)
        {
            Guard.ArgumentNotEmpty(name);

            return DefineType(name, parent)
                .CreateInterfaceImplementation(interfaceTypes)
                .CreateConstructor()
                .CreateProperties(propertyDefinitions)
                .CreateTypeInfo()
                .AsType();
        }

        private static TypeBuilder DefineType(string name, Type parent, TypeAttributes attributes = TypeAttributes.Public | TypeAttributes.Class)
        {
            return _moduleBuilder.DefineType(
                string.Format(CultureInfo.InvariantCulture, "{0}_{1}", name, Guid.NewGuid().ToString().Replace("-", "")),
                attributes,
                parent);
        }
    }
}
