using System;
using System.Collections.Generic;
using System.Linq;

namespace Okiroya.Campione.SystemUtility.Extensions
{
    /// <summary>
    /// Функции-расширения для типов IEnumerable
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Преобразование элементов перечисления с помощью функции конвертации
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="converter"></param>
        /// <returns></returns>
        public static IEnumerable<TResult> EnumerateDataItems<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> converter)
        {
            Guard.ArgumentNotNull(source);
            Guard.ArgumentNotNull(converter);

            return source.Select(converter);
        }

        /// <summary>
        /// Преобразование элементов перечисления с помощью функции конвертации
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TInput1"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="input1"></param>
        /// <param name="converter"></param>
        /// <returns></returns>
        public static IEnumerable<TResult> EnumerateDataItems<TSource, TInput1, TResult>(this IEnumerable<TSource> source, TInput1 input1, Func<TInput1, TSource, TResult> converter)
        {
            Guard.ArgumentNotNull(source);
            Guard.ArgumentNotNull(converter);

            return source.Select(p => converter(input1, p));
        }

        /// <summary>
        /// Преобразование элементов перечисления с помощью функции конвертации
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TInput1"></typeparam>
        /// <typeparam name="TInput2"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="input1"></param>
        /// <param name="input2"></param>
        /// <param name="converter"></param>
        /// <returns></returns>
        public static IEnumerable<TResult> EnumerateDataItems<TSource, TInput1, TInput2, TResult>(this IEnumerable<TSource> source, TInput1 input1, TInput2 input2, Func<TInput1, TInput2, TSource, TResult> converter)
        {
            Guard.ArgumentNotNull(source);
            Guard.ArgumentNotNull(converter);

            return source.Select(p => converter(input1, input2, p));
        }

        /// <summary>
        /// Безопасное преобразование перечисления к массиву
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static TResult[] SafeToArray<TResult>(this IEnumerable<TResult> data)
        {
            return data != null ?
                data.ToArray() :
                new TResult[0];
        }

        /// <summary>
        /// Безопасное преобразование перечисления к элементу
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static TResult SafeToItem<TResult>(this IEnumerable<TResult> data)
        {
            return data != null ?
                data.FirstOrDefault() :
                default(TResult);
        }
    }
}
