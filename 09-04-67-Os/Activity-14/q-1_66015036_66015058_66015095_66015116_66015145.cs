using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Channels;

namespace Ex03
{
    class Program
    {
        private static object Lock = new object();
        private static string x = "";
        private static int exitflag = 0;
        private static bool status = false;

        static void ThReadX(object i)
        {
            while (exitflag == 0)
            {
                lock (Lock)
                {
                    if (status)
                    {
                        Console.WriteLine("Thread-{0} : X = {1}", i, x);
                        status = false;
                    }
                }
            }
            Console.WriteLine("Thread {0} exit", i);
            
        }

        static void ThWriteX()
        {
            string xx = "";
            while (exitflag == 0)
            {
                lock (Lock)
                {
                    Console.Write("Input: ");
                    xx = Console.ReadLine();
                    if (xx == "exit")
                        exitflag = 1;
                    else
                    {
                        x = xx;
                        status = true;
                    }
                }
            }
        }

        static void Main()
        {
            Thread A = new Thread(ThReadX);
            Thread B = new Thread(ThWriteX);

            A.Start(1);
            B.Start();
            

        }
    }
}