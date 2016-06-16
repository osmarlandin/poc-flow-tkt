using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading;

namespace WorkshopEip.Basico.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Consumer";
            Console.WindowHeight = 7;
            Console.WindowWidth = 50;

            var factory = new ConnectionFactory()
            {
                Uri = "amqp://workshop-eip:workshop-eip@localhost:5672/"
            };

            string exchange = "eip.bas.pub";
            string queue = "eip.bas.queue";

            using (var conn = factory.CreateConnection())
            using (IModel channel = conn.CreateModel())
            {
                channel.ExchangeDeclare(exchange, ExchangeType.Fanout, true);
                channel.QueueDeclare(queue, false, false, false, null);
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
