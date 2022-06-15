﻿using Data;
using Logic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KwetterPostService.RabbitMQ
{
    public class RabbitMqSubscriber : BackgroundService
    {
        private IConnection connection;
        private IModel channel;
        private readonly PostLogic logic;
        private readonly IServiceScopeFactory scopeFactory;
        private string queuename = "";

        public RabbitMqSubscriber(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
            var scope = scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<PostDbContext>();

            logic = new PostLogic(dbContext);
            StartClient();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                //todo put own logic here
                var id = Convert.ToInt32(message);
                logic.DeleteAllPostsByUser(id);
            };
            channel.BasicConsume(queue: queuename,
                                 autoAck: true,
                                 consumer: consumer);

            return Task.CompletedTask;
        }

        private void StartClient()
        {
            var factory = new ConnectionFactory() { HostName = "localhost", Port = 49154 };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();

            channel.ExchangeDeclare(exchange: "delete_user_queue", type: ExchangeType.Fanout);

            queuename = channel.QueueDeclare().QueueName;
            //channel.QueueDeclare(queue: "delete_user_queue",
            //                     durable: false,
            //                     exclusive: false,
            //                     autoDelete: false,
            //                     arguments: null);

            channel.QueueBind(queue: queuename, exchange: "delete_user_queue", routingKey: "");

        }

        public override void Dispose()
        {
            if (channel.IsOpen)
            {
                channel.Close();
                connection.Close();
            }
            base.Dispose();
        }
    }
}
