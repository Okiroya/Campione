using System;
using System.Collections.Generic;
using Okiroya.Campione.SystemUtility;

namespace Okiroya.Campione.DataAccess
{
    /// <summary>
    /// Набор методов-расширений для параметров постраничного доступа
    /// </summary>
    public static class ParametersExtensions
    {
        /// <summary>
        /// Выходной параметр - количество элементов в коллекции
        /// </summary>
        public const string RowCountParamName = "Total";
        /// <summary>
        /// Входной параметр - номер текущей страницы
        /// </summary>
        public const string PageNumberParamName = "Page";
        /// <summary>
        /// Входной параметр - количество элементов на странице
        /// </summary>
        public const string PageSizeParamName = "PageSize";
        /// <summary>
        /// Возвращаемый параметр
        /// </summary>
        public const string ReturnValueParamName = "return_value";
        /// <summary>
        /// Входной параметр - ключ сортировки
        /// </summary>
        public const string SortByParamName = "SortBy";
        /// <summary>
        /// Входной параметр - направление сортировки
        /// </summary>
        public const string SortDirectionParamName = "SortDirection";
        /// <summary>
        /// Префикс выходного параметра
        /// </summary>
        public const string OutParamPrefixName = "out-";
        /// <summary>
        /// Постфикс параметра - направление сортировки
        /// </summary>
        public const string RevParamPostfixName = "-rev";
        /// <summary>
        /// Количество элементов на странице по-умолчанию
        /// </summary>
        public const int DefaultPageSize = 20;

        /// <summary>
        /// Добавить параметры постраничного доступа
        /// </summary>
        /// <param name="parameters">Параметры команды</param>
        /// <param name="page">Номер текущей страницы</param>
        /// <param name="pageSize">Количество элементов на странице</param>
        /// <returns></returns>
        public static IDictionary<string, object> AddPagedParameters(this IDictionary<string, object> parameters, int page, int pageSize)
        {
            Guard.ArgumentNotNull(parameters);

            var result = new Dictionary<string, object>(parameters);

            if (!result.ContainsKey($"{OutParamPrefixName}{RowCountParamName}"))
            {
                result.Add($"{OutParamPrefixName}{RowCountParamName}", DefaultParamValue<int>());
            }

            if (!result.ContainsKey(PageNumberParamName))
            {
                result.Add(PageNumberParamName, page);
            }

            if (!result.ContainsKey(PageSizeParamName))
            {
                result.Add(PageSizeParamName, pageSize);
            }

            return result;
        }

        /// <summary>
        /// Добавить параметры сортировки
        /// </summary>
        /// <param name="parameters">Параметры команды</param>
        /// <param name="sortBy">Ключ сортировки</param>
        /// <param name="isAscendingSort">Направление сортировки</param>
        /// <returns></returns>
        public static IDictionary<string, object> AddSortedParameters(this IDictionary<string, object> parameters, string sortBy, bool isAscendingSort)
        {
            Guard.ArgumentNotNull(parameters);

            var result = new Dictionary<string, object>(parameters);

            if (!result.ContainsKey(SortByParamName))
            {
                result.Add(SortByParamName, sortBy);
            }

            if (!result.ContainsKey(SortDirectionParamName))
            {
                result.Add(SortDirectionParamName, isAscendingSort);
            }

            return result;
        }

        /// <summary>
        /// Взять значение из выходных параметров
        /// </summary>
        /// <typeparam name="T">Тип получаемого значения</typeparam>
        /// <param name="parameters">Выходные параметры команды</param>
        /// <param name="paramName">Наименование парамтера</param>
        /// <returns></returns>
        public static T GetOutValue<T>(this IDictionary<string, object> parameters, string paramName)
        {
            Guard.ArgumentNotNull(parameters);
            Guard.ArgumentNotEmpty(paramName);

            paramName = string.Concat(OutParamPrefixName, paramName);

            return parameters.ContainsKey(paramName) ?
                (T)parameters[paramName] :
                default(T);
        }

        /// <summary>
        /// Взять возвращаемое значение
        /// </summary>
        /// <typeparam name="T">Тип возвращаемого значения</typeparam>
        /// <param name="parameters">Выходные параметры команды</param>
        /// <returns></returns>
        public static T GetReturnValue<T>(this IDictionary<string, object> parameters)
        {
            Guard.ArgumentNotNull(parameters);

            return parameters.ContainsKey(ReturnValueParamName) ?
                (T)parameters[ReturnValueParamName] :
                default(T);
        }

        /// <summary>
        /// Сгенерировать системное наименование выходного параметра
        /// </summary>
        /// <param name="paramName">Исходное наименование параметра</param>
        /// <returns></returns>
        public static string MakeOutParamName(string paramName)
        {
            Guard.ArgumentNotEmpty(paramName);

            return string.Concat(OutParamPrefixName, paramName);
        }

        /// <summary>
        /// Вернуть значение по-умолчанию
        /// </summary>
        /// <typeparam name="T">Тип возвращаемого значения</typeparam>
        /// <returns></returns>
        public static T DefaultParamValue<T>()
        {
            return default(T);
        }

        /// <summary>
        /// Вернуть значение для типа
        /// </summary>
        /// <typeparam name="T">Тип возвращаемого значения</typeparam>
        /// <param name="defaultValue">Значение по-умолчанию</param>
        /// <returns></returns>
        public static T DefaultParamValue<T>(T defaultValue)
        {
            return defaultValue;
        }
    }
}
