using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProducerConsumer2
{
    internal class Program
    {
        public static bool terminator = false;

        public static Queue<bool> producedGoods = new Queue<bool>();

        public static int attempts = 0;

        public static void Produce()
        {
            Random rand = new Random();

            while (terminator == false)
            {
                if (producedGoods.Count < 3)
                {
                    Monitor.Enter(producedGoods);
                    producedGoods.Enqueue(true);
                    Console.WriteLine($"Item has been produced.");
                    Monitor.PulseAll(producedGoods);
                    Monitor.Exit(producedGoods);
                    Thread.Sleep(100 / 15);
                }
                else
                {
                    Console.WriteLine("Producer wasn't allowed to produce.");
                    Thread.Sleep(3000);
                }
            }
        }

        public static void Consume()
        {
            Random rand = new Random();

            while (terminator == false)
            {
                if (producedGoods.Count == 0)
                {
                    Console.WriteLine("No items to consume");
                }
                else
                {
                    if (producedGoods.Count >= 0)
                    {
                        while (producedGoods.Count > 0)
                        {
                            Monitor.Enter(producedGoods);
                            producedGoods.Dequeue();
                            Console.WriteLine($"Item has been consumed.");
                            Monitor.PulseAll(producedGoods);
                            Monitor.Exit(producedGoods);
                            Thread.Sleep(100 / 15);
                        }
                    }
                }
                Thread.Sleep(2000);
            }
        }

        static void Main(string[] args)
        {
            Thread producer = new Thread(Produce);
            producer.Name = "Producer";
            producer.Start();

            Thread consumer = new Thread(Consume);
            consumer.Name = "Consumer";
            consumer.Start();

            while (terminator == false)
            {
                if (Console.ReadKey().Key == ConsoleKey.Enter)
                {
                    terminator = true;
                }
            }

            try
            {
                producer.Join();
                consumer.Join();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.ReadLine();
        }
    }
}
