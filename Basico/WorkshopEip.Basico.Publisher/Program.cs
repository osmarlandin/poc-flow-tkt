using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading;

namespace WorkshopEip.Basico.Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Publisher";
            Console.WindowHeight = 7;
            Console.WindowWidth = 50;

            var factory = new ConnectionFactory()
            {
                Uri = "amqp://workshop-eip:workshop-eip@localhost:5672/"
            };

            using (var conn = factory.CreateConnection())
            using (IModel channel = conn.CreateModel())
            {
                channel.ExchangeDeclare("eip.bas.pub", ExchangeType.Fanout, true);
                channel.QueueDeclare("eip.bas.queue", false, false, false, null);

                long i = 0;

                while (true)
                {
                    string message = i++.ToString();
                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish("eip.bas.pub", "", null, body);

                    Console.WriteLine(" [x] Sent {0}", message);
                    Thread.Sleep(1000);
                }
            }
        }
    }
}
