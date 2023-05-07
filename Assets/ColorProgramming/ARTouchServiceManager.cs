using ColorProgramming;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ColorProgramming
{
    public class ARTouchServiceManager
    {
        private readonly Dictionary<Type, ARTouchService> services = new();

        public void RegisterService<T>(T service)
            where T : ARTouchService
        {
            services[typeof(T)] = service;

            // If an exclusive service is registered, disable all active services except the exclusive one
            if (service.IsExclusive)
            {
                foreach (var activeService in services.Values.ToList())
                {
                    if (!activeService.IsExclusive)
                    {
                        activeService.IsEnabled = false;
                    }
                }
            }
        }

        public void UnregisterService<T>(T service)
            where T : ARTouchService
        {
            services.Remove(typeof(T));

            if (service.IsExclusive)
            {
                foreach (var activeService in services.Values.ToList())
                {
                    if (!activeService.IsExclusive)
                    {
                        activeService.IsEnabled = true;
                    }
                }
            }
        }

        public T GetService<T>()
            where T : ARTouchService
        {
            if (!services.TryGetValue(typeof(T), out var service))
            {
                throw new Exception($"Service of type {typeof(T).Name} not found.");
            }

            return (T)service;
        }

        public void TriggerTapEvents(ARTouchData touchData)
        {
            foreach (var service in services.Values.ToList())
            {
                if (service.IsEnabled)
                {
                    service.OnTap(touchData);
                }
            }
        }

        public void TriggerHoldEvents(ARTouchData touchData)
        {
            foreach (var service in services.Values.ToList())
            {
                if (service.IsEnabled)
                {
                    service.OnHold(touchData);
                }
            }
        }

        public void TriggerReleaseEvents(ARTouchData touchData)
        {
            foreach (var service in services.Values.ToList())
            {
                if (service.IsEnabled)
                {
                    service.OnRelease(touchData);
                }
            }
        }
    }
}
