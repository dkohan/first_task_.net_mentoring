/*
 * 2.	Write a program, which creates a chain of four Tasks.
 * First Task – creates an array of 10 random integer.
 * Second Task – multiplies this array with another random integer.
 * Third Task – sorts this array by ascending.
 * Fourth Task – calculates the average value. All this tasks should print the values to console.
 */
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MultiThreading.Task2.Chaining
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(".Net Mentoring Program. MultiThreading V1 ");
            Console.WriteLine("2.	Write a program, which creates a chain of four Tasks.");
            Console.WriteLine("First Task – creates an array of 10 random integer.");
            Console.WriteLine("Second Task – multiplies this array with another random integer.");
            Console.WriteLine("Third Task – sorts this array by ascending.");
            Console.WriteLine("Fourth Task – calculates the average value. All this tasks should print the values to console");
            Console.WriteLine();

            Console.WriteLine("Starting...");
            var t = Run();
            t.Wait();
            Console.WriteLine("Completed");

            Console.ReadLine();
        }

        private static Task Run()
        {
            return Task.Factory.StartNew(Get10RandomDigits)
              .ContinueWith(t => MultipleArray(t.Result))
              .ContinueWith(t => SortArray(t.Result))
              .ContinueWith(t => GetAvgValue(t.Result))
              .ContinueWith(t => Console.WriteLine($"AVG: {t.Result}"));

        }

        private static int[] Get10RandomDigits()
        {
            var array = new int[10];
            var r = new Random();
            for (var i = 0; i < 10; i++)
            {
                array[i] = r.Next(1, 20);
            }
            PrintArrayToConsol("Random numbers", array);
            return array;
        }

        private static int[] MultipleArray(int[] numbers)
        {
            var array = new int[numbers.Length];
            var r = new Random().Next(1, 10);
            Console.WriteLine($"Random number: {r}");

            for (var i = 0; i < array.Length; i++)
            {
                array[i] = numbers[i] * r;
            }
            PrintArrayToConsol("After multipling", array);
            return array;
        }

        private static int[] SortArray(int[] numbers)
        {
            Array.Sort(numbers);
            PrintArrayToConsol("Sorted", numbers);
            return numbers;
        }

        private static double GetAvgValue(int[] numbers)
        {
            var avg = numbers.Average();
            return avg;
        }

        private static void PrintArrayToConsol(string caption, int[] numbers)
        {
            var str = String.Join(";", numbers);
            Console.WriteLine($"{caption} : {str}");
        }
    }
}
