using System;
using System.Collections.Generic;

namespace Okiroya.Campione.Service.Dynamic
{
    internal static class TypeInterpreter<T>
    {
        private static Dictionary<Type, Func<object, T>> _converters = new Dictionary<Type, Func<object, T>>();

        static TypeInterpreter()
        { }

        public static T GetValue(string name, object val)
        {
            var returnType = typeof(T);

            return _converters.ContainsKey(returnType) ?
                _converters[returnType](val) :
                (T)val;
        }
    }
}
