using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VY.Ecommerce.EventBus.Base.Abstraction;
using VY.EcommerceApp.EventBus.UnitTest.Events.Events;

namespace VY.EcommerceApp.EventBus.UnitTest.Events.EventHandler
{
    public class OrderCreatedIntegrationEventHandler : IIntegrationEventHandler<OrderCreatedIntegrationEvent>
    {
        public Task Handle(OrderCreatedIntegrationEvent @event)
        {
            return Task.CompletedTask;
        }
    }
}
