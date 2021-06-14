/*
 * 5. Write a program which creates two threads and a shared collection:
 * the first one should add 10 elements into the collection and the second should print all elements
 * in the collection after each adding.
 * Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.
 */
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task5.Threads.SharedCollection
{
    class Program
    {
        private static List<int> sharedList = new List<int>();
        private static Random _random = new Random();
        private static EventWaitHandle _eventWaitHandle1 = new EventWaitHandle(false, EventResetMode.AutoReset);
        private static EventWaitHandle _eventWaitHandle2 = new EventWaitHandle(false, EventResetMode.AutoReset);
        static void Main(string[] args)
        {
            Console.WriteLine("5. Write a program which creates two threads and a shared collection:");
            Console.WriteLine("the first one should add 10 elements into the collection and the second should print all elements in the collection after each adding.");
            Console.WriteLine("Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.");
            Console.WriteLine();

            Task.Factory.StartNew(AddElement);
            Task.Factory.StartNew(PrintAllElements);

            Console.ReadLine();
        }

        private static void AddElement()
        {
            for (int i = 0; i < 10; i++)
            {
                var element = _random.Next(100);

                sharedList.Add(element);

                Console.WriteLine("Added ===>" + element);
                _eventWaitHandle1.Set();
                _eventWaitHandle2.WaitOne();
            }
        }

        private static void PrintAllElements()
        {
            while (true)
            {
                _eventWaitHandle1.WaitOne();
                foreach (var element in sharedList)
                {
                    Console.WriteLine("element: " + element);
                }

                _eventWaitHandle2.Set();
            }

        }
    }
}
