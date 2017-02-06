using System;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace Okiroya.Campione.SystemUtility.DI
{
    public abstract class RegisterDependencyContainer
    {
        private static ConcurrentDictionary<string, string> _scopes = new ConcurrentDictionary<string, string>();

        internal static ConcurrentDictionary<string, string> Scopes => _scopes;

        public static void RegisterScope(string name, string scope)
        {
            Guard.ArgumentNotEmpty(name);
            Guard.ArgumentNotEmpty(scope);

            _scopes.AddOrUpdate(name, scope, (key, oldValue) => scope);
        }
    }

    /// <summary>
    /// DI - контейнер
    /// </summary>
    /// <typeparam name="T">Тип контракта, для которого используется DI контейнер</typeparam>
    public class RegisterDependencyContainer<T> : RegisterDependencyContainer
    {
        private static readonly string All = "*";

        private static ConcurrentDictionary<string, T> _cachedInstances = new ConcurrentDictionary<string, T>();
        private static ConcurrentDictionary<string, Func<T>> _cachedFactory = new ConcurrentDictionary<string, Func<T>>();
        private static ConcurrentDictionary<string, T> _defaultServices = new ConcurrentDictionary<string, T>();
        private static ConcurrentDictionary<string, Func<T>> _defaultFactory = new ConcurrentDictionary<string, Func<T>>();

        /// <summary>
        /// Зарегистрировать тип сервиса для типа
        /// </summary>
        /// <param name="destType">Тип, для которого регистрируется экземпляр сервиса</param>
        /// <param name="serviceType">Тип регистрируемого сервиса</param>
        public static void RegisterFor(Type destType, Type serviceType)
        {
            RegisterDependencyContainerFactory.RegisterType(serviceType, destType);
        }

        /// <summary>
        /// Зарегистрировать экземпляр сервиса для типа
        /// </summary>
        /// <param name="destType">Тип, для которого регистрируется экземпляр сервиса</param>
        /// <param name="service">Регистрируемый сервис</param>
        public static void RegisterFor(Type destType, T service)
        {
            if (destType != null)
            {
                Register(destType.FullName, service);
            }
        }

        /// <summary>
        /// Зарегистрировать экземпляр сервиса
        /// </summary>
        /// <param name="service">Регистрируемый сервис</param>
        public static void Register(T service)
        {
            Register(typeof(T).FullName, service);
        }

        /// <summary>
        /// Зарегистрировать экземпляр сервиса для определенного ключа
        /// </summary>
        /// <param name="name">Ключ регистрации</param>
        /// <param name="service">Регистрируемый сервис</param>
        public static void Register(string name, T service)
        {
            Guard.ArgumentNotEmpty(name);

            if (service != null)
            {
                _cachedInstances.AddOrUpdate(name, service, (key, oldValue) => service);
            }
        }

        /// <summary>
        /// Зарегистрировать фабрику сервиса
        /// </summary>
        /// <param name="factory">Регистрируемая фабрика</param>
        public static void Register(Func<T> factory)
        {
            Register(typeof(T).FullName, factory);
        }

        /// <summary>
        /// Зарегистрировать фабрику сервиса для определенного ключа
        /// </summary>
        /// <param name="name">Ключ регистрации</param>
        /// <param name="factory">Регистрируемая фабрика</param>
        public static void Register(string name, Func<T> factory)
        {
            Guard.ArgumentNotEmpty(name);

            if (factory != null)
            {
                _cachedFactory.AddOrUpdate(name, factory, (key, oldValue) => factory);
            }
        }

        /// <summary>
        /// Разрешить зависимость -  зарегистрированный ранее сервис для типа
        /// </summary>
        /// <param name="destType">Тип, для которого разрешается зависимость</param>
        /// <returns>Зарегистрированный сервис</returns>
        public static T ResolveFor(Type destType)
        {
            return destType != null ?
                Resolve(destType.FullName) :
                default(T);
        }

        /// <summary>
        /// Разрешить зависимость -  зарегистрированный ранее сервис
        /// </summary>
        /// <returns>Зарегистрированный сервис</returns>
        public static T Resolve()
        {
            var result = RegisterDependencyContainerFactory.MakeService<T>(typeof(T));

            if (result == null)
            {
                result = Resolve(typeof(T).FullName);
            }

            return result;
        }

        /// <summary>
        /// Разрешить зависимость -  зарегистрированный ранее сервис
        /// </summary>
        /// <param name="name">Ключ регистрации</param>
        /// <returns>Зарегистрированный сервис</returns>
        public static T Resolve(string name)
        {
            Guard.ArgumentNotEmpty(name);

            Func<T> factory = null;
            if (_cachedFactory.TryGetValue(name, out factory))
            {
                return factory();
            }
            else if (_defaultFactory.TryGetValue(name, out factory) || _defaultFactory.TryGetValue(All, out factory))
            {
                return factory();
            }
            else
            {
                return _cachedInstances.GetOrAdd(
                    name,
                    (p) =>
                    {
                        T service = default(T);

                        string scope;
                        if (!(_defaultServices.TryGetValue(Scopes.TryGetValue(name, out scope) ? scope : name, out service) || _defaultServices.TryGetValue(All, out service)))
                        {
                            throw new RegisterDependencyException(string.Concat("Зависимость для типа ", p, " не может быть разрешена"));
                        }

                        return service;
                    });
            }
        }

        /// <summary>
        /// Проверка, существует ли регистрация сервиса для типа
        /// </summary>
        /// <param name="destType">Тип, для которого проверяется регистрация</param>
        /// <returns>Существует ли регистрация или нет</returns>
        public static bool IsRegisteredFor(Type destType)
        {
            return destType != null ?
                IsRegistered(destType.FullName) :
                false;
        }

        /// <summary>
        /// Проверка, существует ли регистрация сервиса
        /// </summary>
        /// <param name="name">Ключ регистрации</param>
        /// <returns>Существует ли регистрация или нет</returns>
        public static bool IsRegistered(string name)
        {
            Guard.ArgumentNotEmpty(name);

            return _cachedInstances.ContainsKey(name) || _cachedFactory.ContainsKey(name) ||
                _defaultServices.ContainsKey(name) || _defaultFactory.ContainsKey(name);
        }

        /// <summary>
        /// Зарегистрировать сервис, который будет разрешаться по-умолчанию, если нет явной регистрации
        /// </summary>
        /// <param name="service">Регистрируемый сервис</param>
        public static void SetDefault(T service)
        {
            if (service != null)
            {
                _defaultServices.AddOrUpdate(All, service, (key, oldValue) => service);
            }
        }

        /// <summary>
        /// Зарегистрировать сервис, который будет разрешаться по-умолчанию, если нет явной регистрации
        /// </summary>
        /// <param name="name">Ключ регистрации</param>
        /// <param name="service">Регистрируемый сервис</param>
        public static void SetDefault(string name, T service)
        {
            Guard.ArgumentNotEmpty(name);

            if (service != null)
            {
                _defaultServices.AddOrUpdate(name, service, (key, oldValue) => service);
            }
        }

        /// <summary>
        /// Зарегистрировать фабрику сервиса, которая будет возвращать сервисы по-умолчанию, если нет явной регистрации
        /// </summary>
        /// <param name="factory">Регистрируемая фабрика</param>
        public static void SetDefault(Func<T> factory)
        {
            SetDefault(All, factory);
        }

        /// <summary>
        /// Зарегистрировать фабрику сервиса для определенного ключа, которая будет возвращать сервисы по-умолчанию, если нет явной регистрации
        /// </summary>
        /// <param name="name">Ключ регистрации</param>
        /// <param name="factory">Регистрируемая фабрика</param>
        public static void SetDefault(string name, Func<T> factory)
        {
            Guard.ArgumentNotEmpty(name);

            if (factory != null)
            {
                _defaultFactory.AddOrUpdate(name, factory, (key, oldValue) => factory);
            }
        }

        /// <summary>
        /// Зарегистрировать сервисы, которые будет разрешаться по-умолчанию, если нет явной регистрации
        /// </summary>
        /// <param name="names">Ключи регистрации</param>
        /// <param name="service">Регистрируемый сервис</param>
        public static void SetDefaults(IEnumerable<string> names, T service)
        {
            Guard.ArgumentNotNull(names);

            foreach (var item in names)
            {
                SetDefault(item, service);
            }
        }

        /// <summary>
        /// Сбросить все зарегистрированные сервисы
        /// </summary>
        /// <param name="leaveDefault">Оставить регистрации default сервисов</param>
        public static void Reset(bool leaveDefault = false)
        {
            _cachedInstances.Clear();

            _cachedFactory.Clear();

            if (!leaveDefault)
            {
                _defaultServices.Clear();

                _defaultFactory.Clear();
            }
        }
    }
}
