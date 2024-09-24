using System;
using System.Threading;

namespace OS_Problem_02
{
    class Thread_safe_buffer
    {
        static int[] TSBuffer = new int[10];
        static int Front = 0;
        static int Back = 0;
        static int Count = 0;
        static object _lock = new object();
        static bool _th01Completed = false;

        static void EnQueue(int eq)
        {
            lock (_lock)
            {
                while (Count == TSBuffer.Length) {
                    Monitor.Wait(_lock);
                }
                TSBuffer[Back] = eq;
                Back = (Back + 1) % TSBuffer.Length;
                Count++;
                Monitor.Pulse(_lock);
            }
        }

        static int DeQueue()
        {
            lock (_lock)
            {
                while (Count == 0)
                {
                    Monitor.Wait(_lock);
                }
                int x = TSBuffer[Front];
                Front = (Front + 1) % TSBuffer.Length;
                Count--;
                Monitor.Pulse(_lock);
                return x;
            }
        }

        static void th01()
        {
            for (int i = 1; i <= 50; i++)
            {
                EnQueue(i);
                Thread.Sleep(5);
            }
            lock (_lock)
            {
                _th01Completed = true;
                Monitor.PulseAll(_lock);
            }
        }

        static void th011()
        {
            lock (_lock)
            {
                while (!_th01Completed)
                {
                    Monitor.Wait(_lock);
                }

                for (int i = 100; i < 151; i++)
                {
                    EnQueue(i);
                    Thread.Sleep(5);
                }
            }
        }

        static void th02(object t)
        {
            for (int i = 0; i < 60; i++)
            {
                int j = DeQueue();
                Console.WriteLine("j={0}, thread:{1}", j, t);
                Thread.Sleep(100);
            }
        }

        static void Main(string[] args)
        {
            Thread t1 = new Thread(th01);
            Thread t11 = new Thread(th011);
            Thread t2 = new Thread(th02);
            Thread t21 = new Thread(th02);
            Thread t22 = new Thread(th02);

            t1.Start();
            t11.Start();
            t2.Start(1);
            t21.Start(2);
            t22.Start(3);

        }
    }
}