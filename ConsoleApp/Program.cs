using System;
using System.Threading;
using System.Threading.Tasks;
using ObservableConcurrentQueue;

namespace ConsoleApp
{
    class Program
    {
        private static readonly ObservableConcurrentQueue<int> ObservableConcurrentQueue = new ObservableConcurrentQueue<int>();

        static void Main(string[] args)
        {            
            ObservableConcurrentQueue.ContentChanged += OnObservableConcurrentQueueContentChanged;
            Multiple();  
            Console.WriteLine("End. Press any key to exit...");
            Console.ReadKey(true);
        }

        static void Multiple()
        {
            var rnd = new Random();            

            Task.Run(() =>
            {
                for (int i = 1; i <= 10; i++)
                {
                    ObservableConcurrentQueue.Enqueue(i + rnd.Next(0, 2000));
                    Thread.Sleep(rnd.Next(0, 2000));
                }
            });

            Task.Run(() =>
            {
                for (int i = 1; i <= 10; i++)
                {
                    ObservableConcurrentQueue.Enqueue(i + rnd.Next(0, 2000));
                    Thread.Sleep(rnd.Next(0, 2000));
                }
            });

            Task.Run(() =>
            {
                for (int i = 1; i <= 10; i++)
                {
                    ObservableConcurrentQueue.Enqueue(i + rnd.Next(0, 2000));
                    Thread.Sleep(rnd.Next(0, 2000));
                }
            });
        }

        static void Single()
        {            
            Task.Run(() => {
                Console.WriteLine("Enqueue elements...");
                for (int i = 1; i <= 20; i++)
                {
                    ObservableConcurrentQueue.Enqueue(i);
                    Thread.Sleep(100);
                }

                int item;

                Console.WriteLine("Peek & Dequeue 5 elements...");
                for (int i = 0; i < 5; i++)
                {
                    ObservableConcurrentQueue.TryPeek(out item);
                    Thread.Sleep(300);
                    ObservableConcurrentQueue.TryDequeue(out item);
                    Thread.Sleep(300);
                }

                ObservableConcurrentQueue.TryPeek(out item);
                Thread.Sleep(300);

                Console.WriteLine("Dequeue all elements...");
                while (ObservableConcurrentQueue.TryDequeue(out item))
                {
                    Thread.Sleep(300);
                }
            });
            Console.WriteLine("End. Press any key to exit...");
            Console.ReadKey(true);
        }

        /// <summary>
        /// The observable concurrent queue on changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        private static void OnObservableConcurrentQueueContentChanged(object sender, NotifyConcurrentQueueChangedEventArgs<int> args)
        {
            if (args.Action == NotifyConcurrentQueueChangedAction.Enqueue)
            {
                Console.BackgroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("New Item added: {0}", args.ChangedItem);
                ObservableConcurrentQueue.TryDequeue(out var item);
            }

            if (args.Action == NotifyConcurrentQueueChangedAction.Dequeue)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine("New Item deleted: {0}", args.ChangedItem);
            }

            if (args.Action == NotifyConcurrentQueueChangedAction.Peek)
            {
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("Item peeked: {0}", args.ChangedItem);
            }

            if (args.Action == NotifyConcurrentQueueChangedAction.Empty)
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Queue is empty");
            }

            Console.ResetColor();
        }
    }
}
