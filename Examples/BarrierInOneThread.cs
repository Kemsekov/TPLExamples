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
        public static void BarrierInOneThread(){
            var barrier = new Barrier(3);
            var tasks = new List<Task>();
            for(int i = 0;i<3;i++){
                tasks.Add(Do());
            }
            Task.WaitAll(tasks.ToArray());
            return;
            async Task Do(){
                await Task.Yield();
                System.Console.WriteLine("Begin");
                barrier.SignalAndWait();
                System.Console.WriteLine("End");
            }
        }
    }
}