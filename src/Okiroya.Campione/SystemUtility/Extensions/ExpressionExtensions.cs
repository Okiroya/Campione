using System;
using System.Linq.Expressions;

namespace Okiroya.Campione.SystemUtility.Extensions
{
    /// <summary>
    /// Методы-расширения для Expressions
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class ExpressionExtensions<T>
    {
        private static readonly Func<T> _new = Expression.Lambda<Func<T>>(Expression.New(typeof(T))).Compile();

        /// <summary>
        /// Метод - конструирование нового объекта
        /// </summary>
        public static Func<T> New 
        { 
            get 
            { 
                return _new; 
            } 
        }
    }
}
