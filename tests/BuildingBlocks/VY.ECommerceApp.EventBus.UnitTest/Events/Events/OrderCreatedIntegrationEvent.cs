using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VY.Ecommerce.EventBus.Base.Events;

namespace VY.EcommerceApp.EventBus.UnitTest.Events.Events
{
    public class OrderCreatedIntegrationEvent :IntegrationEvent
    {
        public int Id { get; set; }

        public OrderCreatedIntegrationEvent(int Id)
        {
            this.Id = Id;
        }
    }
}
