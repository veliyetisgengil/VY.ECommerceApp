using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VY.Ecommerce.EventBus.Base.Events;

namespace VY.Ecommerce.EventBus.Base.Abstraction
{
    public interface IIntegrationEventHandler<TIntegrationEvent> : IntegrationEventHandler where TIntegrationEvent : IntegrationEvent
    {
        Task Handle(TIntegrationEvent @event);
    }

    public interface IntegrationEventHandler
    {
    }
}
