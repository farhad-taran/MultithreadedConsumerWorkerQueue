using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MTVSP
{
    class InputProcessor
    {
        //used to hold all the threads
        public static ConcurrentBag<Queue> ThreadQueues = new ConcurrentBag<Queue>();

        public static ConcurrentBag<Message> DeliveredMessages = new ConcurrentBag<Message>();

        public void Start(string input)
        {
            var ints = input.Split(' ');
            int threads, messages;

            try
            {
                 threads = Convert.ToInt32(ints[0]);
                 messages = Convert.ToInt32(ints[1]);
                
                
            }
            catch (Exception)
            {
                throw new Exception("Please enter the number of threads followed by the number of messages, ie 10 100 ");
            }
            
            CreateThreads(threads);
            CreateMessages(messages);

            WaitHandle.WaitAll(ThreadQueues.Select(t => t.AutoReset).ToArray());

            PrintResults();
        }

        private static void PrintResults()
        {
            var avgDispatch = DeliveredMessages.Select(m => m.DispatchCount).Average();
            var delivered = DeliveredMessages.GroupBy(c => c.DispatchCount).OrderBy(d => d.Key).ToList();

            Console.WriteLine(avgDispatch);

            foreach (var number in delivered)
            {
                Console.WriteLine("{0} {1}", number.Key, number.Count());
            }
        }

        private static void CreateMessages(int messages)
        {
            foreach (var t in ThreadQueues)
            {
                for (int i = 0; i < messages; i++)
                {
                    var message = new Message()
                    {
                        StartThreadId = Thread.CurrentThread.ManagedThreadId,
                        Id = i,
                        DispatchCount = 1
                    };

                    t.EnqueueMessage(message);
                }
                //START EACH QUEUE
                t.Start();
            }
        }

        private static void CreateThreads(int threads)
        {
            for (int i = 0; i < threads; i++)
            {
                ThreadQueues.Add(new Queue());
            }
        }
    }
}
