using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RabbitMQ.Client;
using VY.Ecommerce.EventBus.Base;
using VY.Ecommerce.EventBus.Base.Abstraction;
using VY.Ecommerce.EventBus.Factory;
using VY.EcommerceApp.EventBus.UnitTest.Events.EventHandler;
using VY.EcommerceApp.EventBus.UnitTest.Events.Events;

namespace VY.EcommerceApp.EventBus.UnitTest
{
    [TestClass]
    public class EventBusTests
    {
        private ServiceCollection services; 
        public EventBusTests()
        {
            this.services = new ServiceCollection();
            services.AddLogging(configure => configure.AddConsole());
        }

        [TestMethod]
        public void subscribe_event_on_rabbitmq_test()
        {
            services.AddSingleton<IEventBus>(sp =>
            {
                EventBusConfig config = new()
                {
                    ConnectionRetryCount = 5,
                    SubscriberClientAppName = "EventBusUnitTest",
                    DefaultTopicName = "SellingBuddyTopicName",
                    EventBusType = EventBusType.RabbitMQ,
                    EventNameSuffix = "IntegrationEvent",
                    //Connection = new ConnectionFactory()
                    //{
                    //    HostName = "localhost",
                    //    Port = 5672,
                    //    UserName = "guest",
                    //    Password = "guest"
                    //}
                };
                return EventBusFactory.Create(config,sp);
            });

            var sp = services.BuildServiceProvider();

            var eventBus = sp.GetRequiredService<IEventBus>();

            eventBus.Subscribe<OrderCreatedIntegrationEvent, OrderCreatedIntegrationEventHandler>();
            eventBus.UnSubscribe<OrderCreatedIntegrationEvent, OrderCreatedIntegrationEventHandler>();

        }

        [TestMethod]
        public void subscribe_event_on_azure_test()
        {
            services.AddSingleton<IEventBus>(sp =>
            {
                EventBusConfig config = new()
                {
                    ConnectionRetryCount = 5,
                    SubscriberClientAppName = "EventBusUnitTest",
                    DefaultTopicName = "SellingBuddyTopicName",
                    EventBusType = EventBusType.AzureServiceBus,
                    EventNameSuffix = "IntegrationEvent",
                    EventBusConnectionString = "Endpoint=sb://eyecommerce.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=cDadbRXOxa0UxNilPvL83le0aIyoP1FKB1Yocd8JSfg="
                };
                return EventBusFactory.Create(config, sp);
            });

            var sp = services.BuildServiceProvider();

            var eventBus = sp.GetRequiredService<IEventBus>();

            eventBus.Subscribe<OrderCreatedIntegrationEvent, OrderCreatedIntegrationEventHandler>();
            eventBus.UnSubscribe<OrderCreatedIntegrationEvent, OrderCreatedIntegrationEventHandler>();

        }
    }
}
