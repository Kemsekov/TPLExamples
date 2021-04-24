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
        ///<summary>
        ///Run another task that runs 5 another task inside and waits when all of them will be end before ending himself
        ///</summary>
        public async static Task ParentWaitsWhenChildsEndsExecution(){
            var task = Task.Factory.StartNew(()=>{
                for(int i = 0;i<5;i++)
                    Task.Factory.StartNew(Run,CancellationToken.None,TaskCreationOptions.AttachedToParent,TaskScheduler.Default);
            },
                CancellationToken.None,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Default);
            await task;
            return;
            void Run(){
                var rand = new Random();
                System.Console.WriteLine("Child {0} begin execution",Task.CurrentId);
                Thread.Sleep(rand.Next()%1500);
                System.Console.WriteLine("Child {0} end execution",Task.CurrentId);
            }
        }
    }
}