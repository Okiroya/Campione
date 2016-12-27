using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Okiroya.Campione.SystemUtility.FastMember
{
    /// <summary>
    /// Набор extension методов к AssemblyWrapper
    /// </summary>
    internal static class TypeExtensions
    {
        /// <summary>
        /// Сгенерировать конструктор класса
        /// </summary>
        /// <param name="typeBuilder"></param>
        /// <returns></returns>
        public static TypeBuilder CreateConstructor(this TypeBuilder typeBuilder)
        {
            ConstructorBuilder constructor = typeBuilder.DefineConstructor(
                        MethodAttributes.Public,
                        CallingConventions.Standard,
                        Type.EmptyTypes);

            ConstructorInfo constructorInfo = typeof(object).GetConstructor(Type.EmptyTypes);

            ILGenerator il = constructor.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Call, constructorInfo);
            il.Emit(OpCodes.Ret);

            return typeBuilder;
        }

        /// <summary>
        /// Сгенерировать реализацию наследуемых интерфейсов
        /// </summary>
        /// <param name="typeBuilder"></param>
        /// <param name="interfaceTypes"></param>
        /// <returns></returns>
        public static TypeBuilder CreateInterfaceImplementation(this TypeBuilder typeBuilder, Type[] interfaceTypes)
        {
            if (interfaceTypes != null)
            {
                foreach (var item in interfaceTypes)
                {
                    typeBuilder.AddInterfaceImplementation(item);
                }
            }

            return typeBuilder;
        }

        /// <summary>
        /// Сгенерировать набор свойств
        /// </summary>
        /// <param name="typeBuilder"></param>
        /// <param name="propertyDefinitions"></param>
        /// <returns></returns>        
        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Justification = "В данном случае ToLowerInvariant() используется для задания полей динамического класса")]
        public static TypeBuilder CreateProperties(this TypeBuilder typeBuilder, IDictionary<string, Type> propertyDefinitions)
        {
            //TODO: добавить поддержку сущностей с вложенностью
            if ((propertyDefinitions != null) && propertyDefinitions.Any())
            {
                FieldBuilder fieldBuilder;
                PropertyBuilder propertyBuilder;
                MethodBuilder methodBuilder;
                ILGenerator ilGenerator;
                foreach (var item in propertyDefinitions)
                {
                    //exclude Id property
                    if (item.Key.Equals("Id", StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    //underlying field
                    fieldBuilder = typeBuilder
                        .DefineField(string.Format(CultureInfo.InvariantCulture, "_{0}", item.Key.ToLowerInvariant()),
                            item.Value,
                            FieldAttributes.Private);

                    //property
                    propertyBuilder = typeBuilder
                        .DefineProperty(item.Key,
                            PropertyAttributes.HasDefault,
                            item.Value,
                            Type.EmptyTypes);

                    //get property                    
                    methodBuilder = typeBuilder
                        .DefineMethod(string.Format(CultureInfo.InvariantCulture, "get_{0}", item.Key),
                            MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
                            item.Value,
                            Type.EmptyTypes);

                    ilGenerator = methodBuilder.GetILGenerator();

                    ilGenerator.Emit(OpCodes.Ldarg_0);
                    ilGenerator.Emit(OpCodes.Ldfld, fieldBuilder);
                    ilGenerator.Emit(OpCodes.Ret);

                    //map method to property
                    propertyBuilder.SetGetMethod(methodBuilder);

                    //set property
                    methodBuilder = typeBuilder
                        .DefineMethod(string.Format(CultureInfo.InvariantCulture, "set_{0}", item.Key),
                            MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
                            typeof(void),
                            new Type[] { item.Value });

                    ilGenerator = methodBuilder.GetILGenerator();

                    ilGenerator.Emit(OpCodes.Ldarg_0);
                    ilGenerator.Emit(OpCodes.Ldarg_1);
                    ilGenerator.Emit(OpCodes.Stfld, fieldBuilder);
                    ilGenerator.Emit(OpCodes.Ret);

                    //map method to property
                    propertyBuilder.SetSetMethod(methodBuilder);
                }
            }

            return typeBuilder;
        }
    }
}
