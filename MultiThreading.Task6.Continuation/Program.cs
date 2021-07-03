/*
*  Create a Task and attach continuations to it according to the following criteria:
   a.    Continuation task should be executed regardless of the result of the parent task.
   b.    Continuation task should be executed when the parent task finished without success.
   c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation
   d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled
   Demonstrate the work of the each case with console utility.
*/
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task6.Continuation
{
    class Program
    {
        static void Main(string[] args)
        {
            RunRegardlessTask();
            RunSecondTaskWithoutSuccess();
            ReuseParentThreadIfFailed();
            SecondTaskOutSideThreadPoolAfterFirstCancelling();
            Console.ReadLine();
        }

        private static void RunRegardlessTask()
        {
            Console.WriteLine(new string('-', 20));
            Console.WriteLine("Regardless second task:");

            var t = Task.Run(() => Console.WriteLine("First success task"))
              .ContinueWith(task => Console.WriteLine("Second task"));
            Task.WaitAll(t);

            Console.WriteLine();

            t = Task.Run(() =>
            {
                Console.WriteLine(" First task - throw exception");
                throw new Exception();
            }).ContinueWith(task => Console.WriteLine("Second task after faulting first one"));
            Task.WaitAll(t);
        }

        private static void RunSecondTaskWithoutSuccess()
        {
            Console.WriteLine(new string('-', 20));
            Console.WriteLine("Second task will be started if first one isn't successed:");

            var t = Task.Run(() => Console.WriteLine("First task - success\n"));
            t.ContinueWith(task => Console.WriteLine("Second task after successing first one"),
              TaskContinuationOptions.NotOnRanToCompletion);

            Task.WaitAll(t);

            Console.WriteLine();

            t = Task.Run(() =>
            {
                Console.WriteLine("First task - throw exception");
                throw new Exception();
            }).ContinueWith(task => Console.WriteLine("Second task after foulting first one"),
              TaskContinuationOptions.NotOnRanToCompletion);
            if (t.Status != TaskStatus.Canceled)
                Task.WaitAll(t);
        }

        private static void ReuseParentThreadIfFailed()
        {
            Console.WriteLine(new string('-', 20));
            Console.WriteLine("Reuse parent thread if parent has been failed:");

            var t = Task.Run(() => Console.WriteLine("Success first task - second task won't be started\n"));
            t.ContinueWith(task => Console.WriteLine("Second task"),
              TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously);

            Task.WaitAll(t);

            Console.WriteLine();
            Console.WriteLine($"Main thread: {Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine();

            t = Task.Run(() =>
            {
                Console.WriteLine("Throw exception in firs task");
                Console.WriteLine($"Current thread: {Thread.CurrentThread.ManagedThreadId}");
                throw new Exception();
            }).ContinueWith(task =>
            {
                Console.WriteLine("Second task - if first task has been foulted");
                Console.WriteLine($"Current thread: {Thread.CurrentThread.ManagedThreadId}");
            }, TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously);

            if (t.Status != TaskStatus.Canceled)
                Task.WaitAll(t);
        }

        private static void SecondTaskOutSideThreadPoolAfterFirstCancelling()
        {
            Console.WriteLine(new string('-', 20));
            Console.WriteLine("Second task outside ThreadPool after first cancelling:");

            var cts = new CancellationTokenSource();
            var token = cts.Token;
            var t = Task.Run(() =>
            {
                Console.WriteLine("First Task - will be cancelled");
                while (true)
                {
                    if (token.IsCancellationRequested)
                        token.ThrowIfCancellationRequested();
                }
            }, token);

            Thread.Sleep(1000);

            cts.Cancel();

            var continuous = t.ContinueWith(task =>
            {
                var thread = new Thread(Print);
                thread.Start("SecondTask after cancelling");
            }, TaskContinuationOptions.OnlyOnCanceled);

            Task.WaitAll(continuous);
        }

        private static void Print(object s)
        {
            Console.WriteLine(s);
        }

    }
}
