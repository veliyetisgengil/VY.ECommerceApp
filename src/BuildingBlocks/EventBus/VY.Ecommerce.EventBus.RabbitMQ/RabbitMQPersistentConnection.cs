﻿using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace VY.Ecommerce.EventBus.RabbitMQ
{
    public class RabbitMQPersistentConnection : IDisposable
    {
        private readonly IConnectionFactory connectionFactory;
        private readonly int retryCount;
        private IConnection connection;
        private object lock_object = new object();
        private bool _dispose;

        public RabbitMQPersistentConnection(IConnectionFactory connectionFactory,int retryCount=5)
        {
            this.connectionFactory = connectionFactory;
            this.retryCount = retryCount;
        }
        public bool IsConnection => connection != null && connection.IsOpen;

        public IModel CreateModel()
        {
            return connection.CreateModel();
        }

        public void Dispose()
        {
            _dispose = true;
            connection.Dispose();
        }

        public bool TryConnect()
        {
            lock (lock_object)
            {
                var policy = Policy.Handle<SocketException>()
                    .Or<BrokerUnreachableException>()
                    .WaitAndRetry(retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                         {

                         }
                         );
                policy.Execute(()=> { connection = connectionFactory.CreateConnection(); });

                if (IsConnection)
                {
                    //log
                    connection.ConnectionShutdown += Connection_ConnectionShutdown;
                    connection.ConnectionBlocked += Connection_ConnectionBlocked; 
                    connection.CallbackException += Connection_CallbackException;
                    return true;
                }
                return false;

            }
        }

        private void Connection_CallbackException(object sender, global::RabbitMQ.Client.Events.CallbackExceptionEventArgs e)
        {
            if (_dispose) return;
            //log
            TryConnect();
        }

        private void Connection_ConnectionBlocked(object sender, global::RabbitMQ.Client.Events.ConnectionBlockedEventArgs e)
        {
            if (_dispose) return;
            //log
            TryConnect();
        }

        private void Connection_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            if (_dispose) return;
            //log
            TryConnect();
        }
    }
}
