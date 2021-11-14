using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VY.Ecommerce.EventBus.AzureServiceBus;
using VY.Ecommerce.EventBus.Base;
using VY.Ecommerce.EventBus.Base.Abstraction;
using VY.Ecommerce.EventBus.RabbitMQ;

namespace VY.Ecommerce.EventBus.Factory
{
    public static class EventBusFactory
    {
        public static IEventBus Create(EventBusConfig config,IServiceProvider serviceProvider)
        {
            return config.EventBusType switch
            {
                EventBusType.AzureServiceBus => new EventBusServiceBus(config,serviceProvider),
                _ => new EventBusRabbitMQ(config,serviceProvider),
            };
        }

    }
}
