using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using Okiroya.Campione.Configuration.DI;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace Okiroya.Campione.SystemUtility.DI
{
    internal class RegisterDependencyContainerFactory
    {
        private static ConcurrentDictionary<string, Tuple<Type, bool>> _typeCache = new ConcurrentDictionary<string, Tuple<Type, bool>>();

        private RegisterDependencyContainerOptions _options;

        public RegisterDependencyContainerFactory(IOptions<RegisterDependencyContainerOptions> optionsAccessor)
        {
            Guard.ArgumentNotNull(optionsAccessor);

            _options = optionsAccessor.Value;

            Init();
        }

        public static void RegisterType(Type serviceType, Type dependencyType)
        {
            Guard.ArgumentNotNull(serviceType);
            Guard.ArgumentNotNull(dependencyType);

            _typeCache.AddOrUpdate(
                key: dependencyType.Name,
                addValue: new Tuple<Type, bool>(serviceType, serviceType.Name.Contains('`')),
                updateValueFactory: (key, oldValue) => new Tuple<Type, bool>(serviceType, serviceType.Name.Contains('`')));
        }

        public static T MakeService<T>(Type type)
        {
            Guard.ArgumentNotNull(type);

            T result = default(T);

            Func<string, Tuple<Type, bool>> predicate =
                (p) =>
                {
                    return _typeCache.ContainsKey(p) ?
                        _typeCache[p] :
                        null;
                };

            Tuple<Type, bool> typeDefinition = predicate(type.Name);

            if (typeDefinition == null)
            {
                typeDefinition = predicate(type.FullName);
            }

            if (typeDefinition != null)
            {
                Type typeOfService = typeDefinition.Item1;

                if (typeDefinition.Item2)
                {
                    var typeParameters = type.GetGenericArguments();

                    typeOfService = typeOfService.MakeGenericType(typeParameters);
                }

                result = (T)Activator.CreateInstance(typeOfService, null);
            }

            return result;
        }

        private void Init()
        {
            var dependencyDictionary = new Dictionary<string, RegisterDependencyContainerElement>();
            foreach (var dependency in _options.Dependencies)
            {
                if (dependency.Enabled)
                {
                    if (!string.IsNullOrEmpty(dependency.DependencyType) && !string.IsNullOrEmpty(dependency.ServiceType))
                    {
                        if (!dependencyDictionary.ContainsKey(dependency.Name))
                        {
                            dependencyDictionary.Add(dependency.Name, dependency);
                        }
                        else
                        {
                            dependencyDictionary[dependency.Name] = dependency;
                        }
                    }
                    else
                    {
                        throw new RegisterDependencyException(string.Concat("Регистрация сервиса ", dependency.Name, " не валидная"));
                    }
                }
            }

            foreach (var dependency in dependencyDictionary.Values)
            {
                var type = Type.GetType(dependency.ServiceType, false);

                if (type == null)
                {
                    throw new RegisterDependencyException(string.Concat("Тип ", dependency.ServiceType, " системе неизвестен"));
                }

                _typeCache.AddOrUpdate(
                    key: dependency.DependencyType,
                    addValue: new Tuple<Type, bool>(type, dependency.ServiceType.Contains('`')),
                    updateValueFactory: (key, oldValue) => new Tuple<Type, bool>(type, dependency.ServiceType.Contains('`')));
            }
        }
    }
}
