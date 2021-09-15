using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TPLLearn.Examples
{
    public static partial class TPLExamples{
        public static void AutoResetEventExample(){
            var autoResetEvent = new AutoResetEvent(false);
            var tasks = new List<Task>();
            //we run 5 tasks
            for(int i = 0;i<5;i++)
                tasks.Add(Task.Run(DoStuff));
            //we wait until they do something
            Task.Delay(1000).Wait();
            System.Console.WriteLine("autoResetEvent set");

            //we release one task per second until they all finished
            while(!Task.WaitAll(tasks.ToArray(),TimeSpan.FromSeconds(1))){
                System.Console.WriteLine("autoResetEvent set again");
                autoResetEvent.Set();
            }
            return;

            async Task DoStuff(){
                System.Console.WriteLine("Begin");
                await Task.Delay(new Random().Next(1000));
                System.Console.WriteLine("Waiting autoResetEvent");
                autoResetEvent.WaitOne();
                System.Console.WriteLine("End");
            }
        }
    }
}