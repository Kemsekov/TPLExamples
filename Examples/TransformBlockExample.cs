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
        public static async Task TransformBlockExample(){
            var options = new ExecutionDataflowBlockOptions();
            options.BoundedCapacity = Environment.ProcessorCount*4;
            options.MaxDegreeOfParallelism = Environment.ProcessorCount;
            options.EnsureOrdered = false;
            var transformBlock = new TransformBlock<int,string>(async sizeOfWord=>{
                System.Console.WriteLine("Request {0} with {1} size of word",Task.CurrentId,sizeOfWord);
                StringBuilder buff = new();
                var rand = new Random();
                for(int j = 0; j<sizeOfWord;j++){
                    if(rand.Next()%10>7){
                        buff.Append(rand.Next()%10);
                        continue;
                    }
                    buff.Append(Convert.ToChar(rand.Next()%25+65));
                }
                return buff.ToString();
            },options);
            var actionBlock = new ActionBlock<string>(async request=>{
                var rand = new Random();
                Thread.Sleep(rand.Next()%1000+500);
                int sum = 0;
                foreach(char c in request){
                    if(char.IsLetter(c)){
                        sum++;
                        continue;
                    }
                    if(char.IsDigit(c)){
                        sum+=int.Parse($"{c}");
                    }
                }
                System.Console.WriteLine("Result of {0} request {1} is {2}",request,Task.CurrentId,sum);
            },options);
            transformBlock.LinkTo(actionBlock,new DataflowLinkOptions{
                PropagateCompletion = true
            });

            for(int i = 200;i>0;--i){
                if(transformBlock.InputCount<options.BoundedCapacity/2)
                   transformBlock.Post(20);
                await transformBlock.SendAsync(20);
            }
            transformBlock.Complete();
            Task.WaitAll(transformBlock.Completion);
        } 
    }
}