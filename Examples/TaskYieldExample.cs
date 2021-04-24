using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TPLLearn.Examples
{
    public static partial class TPLExamples
    {
        public static async Task TaskYieldExample(){
            var StartTime = DateTime.UtcNow.Second;
            Func<Task> someWork = async ()=>{
                System.Console.WriteLine("SomeWork beggining "+TimeOfExec());
                System.Console.WriteLine("0 "+TimeOfExec());
                var task = LogRunningProccess();
                //do some stuff
                System.Console.WriteLine("1 "+TimeOfExec());
                Task.Delay(1000).Wait();
                await task;
                return;
            };
            var c =  someWork();
            System.Console.WriteLine("After Invoking SomeWork() "+TimeOfExec());
            await c;
            System.Console.WriteLine("After awaiting SomeWork() "+TimeOfExec());
            return;
            async Task LogRunningProccess(){
                //some long proccess that will make thread bussy
                //It will block thread until it finish execution
                //but what if we need to be sure that this method will not block 
                //a thread, e.g another initialization need to be done etc?
                await Task.Yield();
                //this await will return execution to TaskYieldExample method until
                //it will be awaited.
                Task.Delay(2000).Wait();
                System.Console.WriteLine("2 "+TimeOfExec());
                //in this case we already at line --- await c; ---
                //and this awaiting will do nothing. You may comment it and re-run
                await Task.Yield();
                Task.Delay(1000).Wait();
                System.Console.WriteLine("3 "+TimeOfExec());
                return;
            }
            int TimeOfExec(){
                return DateTime.UtcNow.Second-StartTime;
            }
        }
    }
}