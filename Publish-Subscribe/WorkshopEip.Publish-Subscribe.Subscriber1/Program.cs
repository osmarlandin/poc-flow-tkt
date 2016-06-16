using RabbitMQ.Client;
using System;
using System.Text;

namespace WorkshopEip.Publish_Subscribe.Subscriber1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Subscriber1";
            Console.WindowHeight = 7;
            Console.WindowWidth = 50;

            string exchange = "eip.pubsub.pub";
            string queue = "eip.pubsub.queue01";

            var factory = new ConnectionFactory()
            {
                Uri = "amqp://workshop-eip:workshop-eip@localhost:5672/"
            };

            using (var conn = factory.CreateConnection())
            using (IModel channel = conn.CreateModel())
            {
                channel.ExchangeDeclare(exchange, ExchangeType.Fanout, true);
                channel.QueueDeclare(queue, true, false, false, null);
                channel.QueueBind(queue, exchange, string.Empty);

                QueueingBasicConsumer consumer = new QueueingBasicConsumer(channel);

                channel.BasicConsume(queue, true, consumer);

                while (true)
                {
                    var ea = consumer.Queue.Dequeue();
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(message);
                }
            }
        }
    }
}
