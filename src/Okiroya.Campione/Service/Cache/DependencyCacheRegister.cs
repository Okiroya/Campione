using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Okiroya.Campione.SystemUtility;

namespace Okiroya.Campione.Service.Cache
{
    /// <summary>
    /// Реестр зависимостей команд. Обычно команда на чтение данных зависит от выполения команды на изменения данных, т.е
    /// при изменении данных необходимо сбросить кэш результата выполнения команды на чтение данных
    /// </summary>
    public static class DependencyCacheRegister
    {
        private static ConcurrentDictionary<string, IList<string>> _register = new ConcurrentDictionary<string, IList<string>>();

        /// <summary>
        /// Добавить зависимость одной команды от другой
        /// </summary>
        /// <param name="commandName">Наименование команды</param>
        /// <param name="dependendedCommandName">Наименование зависимой команды</param>
        public static void AddToDependcy(string commandName, string dependendedCommandName)
        {
            Guard.ArgumentNotEmpty(commandName);
            Guard.ArgumentNotEmpty(dependendedCommandName);

            //Защита от зацикливания
            if (!dependendedCommandName.Equals(commandName, StringComparison.OrdinalIgnoreCase))
            {
                AddToDependcy(commandName, new[] { dependendedCommandName });
            }
        }

        /// <summary>
        /// Добавить зависимость одной команды от других
        /// </summary>
        /// <param name="commandName">Наименование команды</param>
        /// <param name="dependencyCommands">Список зависимых команд</param>
        public static void AddToDependcy(string commandName, IEnumerable<string> dependencyCommands)
        {
            Guard.ArgumentNotEmpty(commandName);
            Guard.ArgumentNotNull(dependencyCommands);

            //Защита от зацикливания
            dependencyCommands = dependencyCommands.Where(p => !p.Equals(commandName, StringComparison.OrdinalIgnoreCase));

            _register.AddOrUpdate(
                commandName,
                new List<string>(dependencyCommands),
                (key, oldValue) =>
                {
                    return oldValue.Zip(
                        dependencyCommands,
                        (first, second) =>
                        {
                            return first.Equals(second, StringComparison.OrdinalIgnoreCase) ?
                                first :
                                second;
                        })
                        .ToList();
                });
        }

        /// <summary>
        /// Вернуть список зависимостей для команды. Если зависимостей нет, то возвращается null
        /// </summary>
        /// <param name="commandName">Наименование команды</param>
        /// <returns></returns>
        public static IEnumerable<string> GetDependencies(string commandName)
        {
            Guard.ArgumentNotEmpty(commandName);

            IList<string> result;

            return _register.TryGetValue(commandName, out result) ?
                result :
                null;
        }
    }
}
