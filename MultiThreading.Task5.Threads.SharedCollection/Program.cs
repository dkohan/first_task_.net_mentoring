/*
 * 5. Write a program which creates two threads and a shared collection:
 * the first one should add 10 elements into the collection and the second should print all elements
 * in the collection after each adding.
 * Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.
 */
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MultiThreading.Task5.Threads.SharedCollection
{
    class Program
    {

        private static readonly object LockObj = new object();
        private static List<int> Collection { get; set; } = new List<int>();

        static void Main(string[] args)
        {
            Console.WriteLine("5. Write a program which creates two threads and a shared collection:");
            Console.WriteLine("the first one should add 10 elements into the collection and the second should print all elements in the collection after each adding.");
            Console.WriteLine("Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.");
            Console.WriteLine();


            Run();
            Console.ReadLine();
        }

        private static void Run()
        {
            for (var i = 0; i < 10; i++)
            {
                var element = i;
                Task.Run(() => Add(element));
            }
        }

        private static void Print()
        {
            string toPrint;
            lock (LockObj)
            {
                toPrint = string.Join(";", Collection);
            }
            Console.WriteLine(toPrint);
        }

        private static void Add(int element)
        {
            lock (LockObj)
            {
                Console.WriteLine($"{element} has been added to collection");
                Collection.Add(element);
            }

            Task.Run((Action)Print);
        }
    }
}
