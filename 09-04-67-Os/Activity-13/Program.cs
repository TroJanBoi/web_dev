// start-program-2
// using System;
// using System.Diagnostics;

// namespace ex01
// {
//     class Program
//     {
//         private static int sum = 0;

//         static void plus()
//         {
//             int i;
//             for (i = 1; i < 1000001; i++)
//             {
//                 sum += i;
//             }
//         }

//         static void minus()
//         {
//             int i;
//             for (i = 0; i < 1000000; i++)
//             {
//                 sum -= i;
//             }
//         }

//         static void Main(string[] args)
//         {
//             Stopwatch sw = new Stopwatch();
//             Console.WriteLine("Start...");
//             sw.Start();
//             plus();
//             minus();
//             sw.Stop();
//             Console.WriteLine("sum = {0}", sum);
//             Console.WriteLine("Time used: " + sw.ElapsedMilliseconds.ToString() + "ms");
//         }
//     }
// }
// end-program-1


// start-program-2
// using System;
// using System.Diagnostics;

// namespace ex01
// {
//     class Program
//     {
//         private static object Lock = new object();
//         private static int sum = 0;

//         static void plus()
//         {
//            lock (Lock)
//            {
//                 int i;
//                 for (i = 1; i < 1000001; i++)
//                 {
//                     sum += i;
//                 }
//            }
//         }

//         static void minus()
//         {
//             int i;
//             for (i = 0; i < 1000000; i++)
//             {
//                 sum -= i;
//             }
//         }

//         static void Main(string[] args)
//         {
//             Thread P = new Thread(new ThreadStart(plus));
//             Thread M = new Thread(new ThreadStart(minus));

//             Stopwatch sw = new Stopwatch();
//             Console.WriteLine("Start...");
//             sw.Start();

//             P.Start();
//             M.Start();
            
//             P.Join();
//             M.Join();

//             sw.Stop();
//             Console.WriteLine("sum = {0}", sum);
//             Console.WriteLine("Time used: " + sw.ElapsedMilliseconds.ToString() + "ms");
//         }
//     }
// }
// end-program-2

// start-program-3
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
            Thread C = new Thread(ThReadX);

            A.Start(1);
            C.Start(2);
            B.Start();
            

        }
    }
}
// end-programs-3