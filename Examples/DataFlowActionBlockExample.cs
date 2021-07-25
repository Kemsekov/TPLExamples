using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace TPLLearn.Examples
{
    public static partial class TPLExamples
    {
        public static async Task DataFlowActionBlockExample(){
            var rand = new ThreadLocal<Random>(()=>new Random());
            var options = new ExecutionDataflowBlockOptions();

            options.MaxDegreeOfParallelism = Environment.ProcessorCount;
            options.EnsureOrdered = false;
            options.BoundedCapacity=Environment.ProcessorCount*8;

            var actionBlock = new ActionBlock<int>(async request=>{
                System.Console.WriteLine("Request {0} is proccessed in {1} thread",request,Task.CurrentId);
                await Task.Yield();
                System.Console.WriteLine("After {0} is proccessed in {1} thread",request,Task.CurrentId);
            },options);
            
            var end = 100;
            var sum = 0;

            for(int i = 0;i<end;i++){
                sum+=actionBlock.InputCount;
                if(actionBlock.InputCount<=options.BoundedCapacity/2){
                    actionBlock.Post(i);
                    continue;
                }
                //System.Console.WriteLine("action block is now bussy");
                await actionBlock.SendAsync(i);
            }
            System.Console.WriteLine("Avarange InputCount {0}",sum/end);
        }
    }
}