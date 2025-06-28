using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.Dependency
{
    public class ServiceRegistrator : MonoBehaviour
    {
         [SerializeField] private List<MonoBehaviour> _instances = new();

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            InitializeServices();
        }

        private void Start()
        {
            Debug.Log($"Total registered services: {ServiceLocator.Services.Count}");
        }
        
        private void InitializeServices()
        {
            foreach (var instance in _instances)
            {
                if (instance is IService service)
                {
                    var interfaces = service.GetType().GetInterfaces().Where(i => typeof(IService).IsAssignableFrom(i) && i != typeof(IService));

                    foreach (var iface in interfaces)
                    {
                        ServiceLocator.Register(iface, service);
                        DontDestroyOnLoad(instance);
                    }
                }
            }
        }
    }
}