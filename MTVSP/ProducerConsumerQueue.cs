using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Collections.Generic;

namespace MTVSP
{
   public class Queue
    {
       private AutoResetEvent _autoReset = new AutoResetEvent(false);
       public AutoResetEvent AutoReset {
           get { return _autoReset; }
       }

       public Thread Thread;
        private ConcurrentQueue<Message> messages = new ConcurrentQueue<Message>();

        private Timer timer;

        Random rnd = new Random();

        public int ThreadId
        {
            get { return Thread.ManagedThreadId; }
        }

        public Queue()
        {
            Thread = new Thread(Work);
        }

        public void Start()
        {
            Thread.Start();
        }

        public void EnqueueMessage(Message message)
        {
           messages.Enqueue(message);
        }
        
        private void Work()
        {
            while (true)
            {
                
                Message message = null;
                
                    if (messages.Count > 0)
                    {
                        bool success = messages.TryDequeue(out message);
                        if (!success)
                        {
                            return;
                        }
                    }
                if (message != null)
                {
                    
                    //randomly generate destination thread id for message
                    var threadIds = InputProcessor.ThreadQueues.Select(t => t.ThreadId).ToList();
                    int destinationThreadId = rnd.Next(threadIds.Min(), threadIds.Max());
                    message.DestinationThreadId = destinationThreadId;

                    message.DispatchCount++;

                    if (message.DestinationThreadId == ThreadId)
                    {
                        message.Delivered = true;
                        InputProcessor.DeliveredMessages.Add(message);

                        //Console.WriteLine("thread {0} DEL message{1} : ", ThreadId, message.DestinationThreadId);
                    }
                    else
                    {
                        var destinationThread = InputProcessor.ThreadQueues.FirstOrDefault(
                            t => t.Thread.ManagedThreadId == message.DestinationThreadId);

                        if (destinationThread != null)
                        {
                            destinationThread.messages.Enqueue(message);
                            //Console.WriteLine("thread {0} RED message{1} : ", ThreadId, message.DestinationThreadId);
                        }
                    }
                    
                }
                else
                {
                    _autoReset.Set();
                }
            }
        }
    }
}