using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.Dependency
{
    [DefaultExecutionOrder(-4000)]
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, object> _services = new();
        
        public static IReadOnlyDictionary<Type, object> Services => _services;

        public static void Register(Type type, object instance)
        {
            _services[type] = instance;

            if (instance is IService service)
            {
                service.IsInitialized = true;
                
                Debug.Log($"Services Type: {_services[type].GetType()} and Services Value: {instance.GetType().Name}");
            }
        }

        public static void Register<T>(object instance) where T : IService
        {
            _services[typeof(T)] = instance;
        }
        
        public static bool UnRegister(Type service)
        {
            return _services.Remove(service);
        }

        public static T Resolve<T>() where T : IService
        {
            if (_services.TryGetValue(typeof(T), out var instance))
                return (T)instance;

            return default;
        }

        public static bool TryGetService<T>(out object instance) where T : IService
        {
            if (_services.TryGetValue(typeof(T), out instance))
                return true;
            
            return false;
        }

        public static void Clear()
        {
            _services.Clear();
        }

        public static async UniTask<bool> IsInitialized()
        {
            await UniTask.WaitUntil(() => _services.Values.OfType<IService>().All(service => service.IsInitialized));
            return true;
        }
    }
}
