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
    public static partial class TPLExamples
    {
        public static void BarrierExample(){
            var barier = new Barrier(5);
            
            var tasks = new List<Task>();
            for(int i = 0;i<5;i++){
                tasks.Add(Task.Run(DoSomeWork));
            }
            Task.WaitAll(tasks.ToArray());

            return;
            void DoSomeWork(){
                var rand = new Random();
                System.Console.WriteLine("Start SomeWork {0}",Task.CurrentId);
                Thread.Sleep(rand.Next()%1500);
                System.Console.WriteLine("Did first part of work {0}",Task.CurrentId);
                barier.SignalAndWait();
                Thread.Sleep(rand.Next()%1500);
                System.Console.WriteLine("Start second part of work {0}",Task.CurrentId);
                barier.SignalAndWait();
                Thread.Sleep(rand.Next()%1500);
                System.Console.WriteLine("Done {0}",Task.CurrentId);
            }
        }
       
    }
}