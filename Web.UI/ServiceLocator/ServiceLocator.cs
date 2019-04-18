using System;
using System.Collections.Generic;

namespace Web.UI.ServiceLocator
{
    public static class ServiceLocator 
    {
        private readonly static IDictionary<Type, object> services = new Dictionary<Type, object>();

        public static void Register<T>( T service)
        {

            services[typeof(T)] = service;
        }

        public static T GetService<T>()
        {
            try
            {
                return (T)services[typeof(T)];
            }
            catch (KeyNotFoundException)
            {
                throw new ApplicationException("The requested service is not registered");
            }
        }
    }
}