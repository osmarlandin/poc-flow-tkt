using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading;

namespace WorkshopEip.CompetingConsumers.Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Publisher";
            Console.WindowHeight = 7;
            Console.WindowWidth = 50;

            string exchange = "eip.compcons.pub";

            var factory = new ConnectionFactory()
            {
                Uri = "amqp://workshop-eip:workshop-eip@localhost:5672/"
            };

            using (var conn = factory.CreateConnection())
            using (IModel channel = conn.CreateModel())
            {
                channel.ExchangeDeclare(exchange, ExchangeType.Fanout, true);

                long i = 0;

                while (true)
                {
                    string message = string.Format("{0} - msg", i++.ToString());
                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish(exchange, "", null, body);

                    Console.WriteLine(" [x] Sent {0}", message);
                    Thread.Sleep(1000);
                }
            }
        }
    }
}
