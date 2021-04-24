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
        ///<summary>
        ///Represents more simple way to wait all or any task completion and accumulating results
        ///</summary>
        public static async Task TaskRunningChaining(){
            var tasks = new List<Task<int>>();
            for(int i = 0;i<5;i++){
                tasks.Add(Task.Run(Run));
            }
            //awaits end of execution of 5 tasks from tasks List then
            //recieve all ended tasks,
            //prints how much tasks ended,
            //summarize all results of ended tasks in one sum varable and prints it.
            await Task.Factory.ContinueWhenAll(tasks.ToArray(),
            (Task<int>[] ended_tasks)=>{
                System.Console.WriteLine("{0} tasks ended",ended_tasks.Length);
                var sum = 
                    ended_tasks.Aggregate(
                        (Task<int> t1, Task<int> t2)=>{
                            var _sum = t1.Result+t2.Result;
                            return Task.FromResult(_sum);
                        }).Result;
                System.Console.WriteLine("Sum of the all results is {0}",sum);
            });
            
            return;
            int Run(){
                var rand = new Random();
                System.Console.WriteLine("Run {0} begin execution",Task.CurrentId);
                Thread.Sleep(rand.Next()%1500);
                var result = rand.Next()%100;
                System.Console.WriteLine("Run {0} end execution with result {1}",Task.CurrentId,result);
                return result;
            }
        }
    }
}