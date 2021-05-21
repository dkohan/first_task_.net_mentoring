/*
 * 4.	Write a program which recursively creates 10 threads.
 * Each thread should be with the same body and receive a state with integer number, decrement it,
 * print and pass as a state into the newly created thread.
 * Use Thread class for this task and Join for waiting threads.
 * 
 * Implement all of the following options:
 * - a) Use Thread class for this task and Join for waiting threads.
 * - b) ThreadPool class for this task and Semaphore for waiting threads.
 */

using System;
using System.Threading;

namespace MultiThreading.Task4.Threads.Join
{
    class Program
    {
        private static int _counter = 0;
        private static Semaphore _sem = new Semaphore(3, 3);

        static void Main(string[] args)
        {
            Console.WriteLine("4.	Write a program which recursively creates 10 threads.");
            Console.WriteLine("Each thread should be with the same body and receive a state with integer number, decrement it, print and pass as a state into the newly created thread.");
            Console.WriteLine("Implement all of the following options:");
            Console.WriteLine();
            Console.WriteLine("- a) Use Thread class for this task and Join for waiting threads.");
            Console.WriteLine("- b) ThreadPool class for this task and Semaphore for waiting threads.");


            ThreadPool.QueueUserWorkItem(Run, 100);
            
            Console.ReadLine();
        }


        private static void Run(object begin)
        {
            if (_counter++ == 10)
                return;

            int number;
            if (!Int32.TryParse(begin.ToString(), out number))
                throw new ArgumentException();

            _sem.WaitOne();

            Console.WriteLine(number--);
            ThreadPool.QueueUserWorkItem(Run, number);

            _sem.Release();
        }
    }
}
