using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VY.Ecommerce.EventBus.Base.Abstraction;
using VY.Ecommerce.EventBus.Base.SubManagers;

namespace VY.Ecommerce.EventBus.Base.Events
{
    public class BaseEventBus : IEventBus
    {
        public readonly IServiceProvider ServiceProvider;
        public readonly IEventBusSubscriptionManager SubsManager;
        private EventBusConfig eventBusConfig;

        public BaseEventBus(EventBusConfig config,IServiceProvider serviceProvider)
        {
            eventBusConfig = config;
            ServiceProvider = serviceProvider;
            SubsManager = new InMemoryEventBusSubscriptionManager(ProcessEventName);
        }

        public virtual string ProcessEventName(string eventName)
        {
            if (eventBusConfig.DeleteEventPrefix)
                eventName = eventName.TrimStart(eventBusConfig.EventNamePrefix.ToArray());
            if (eventBusConfig.DeleteEventSuffix)
                eventName = eventName.TrimEnd(eventBusConfig.EventNameSuffix.ToArray());
            return eventName;
        }
        public virtual string GetSubName(string eventName) 
        {
            return $"{eventBusConfig.SubscriberClientAppName}.{ProcessEventName(eventName)}";
        }
        public virtual void Dispose()
        {
            eventBusConfig = null;
        }

        public async Task<bool> ProcessEvent(string eventName,string message) 
        {
            eventName = ProcessEventName(eventName);

            var processed = false;



            return processed;
        }

        public void Publish(IntegrationEvent @event)
        {
            throw new NotImplementedException();
        }

        public void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            throw new NotImplementedException();
        }

        public void UnSubscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            throw new NotImplementedException();
        }
    }
}
