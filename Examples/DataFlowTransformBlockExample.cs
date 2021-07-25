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
        public static async Task DataFlowTransformBlockExample(){
            var options = new ExecutionDataflowBlockOptions();
            options.BoundedCapacity = Environment.ProcessorCount*8;
            options.MaxDegreeOfParallelism = Environment.ProcessorCount;
            options.EnsureOrdered = false;
            var rand = new ThreadLocal<Random>(()=>new Random());
            var buff = new ThreadLocal<StringBuilder>(()=>new StringBuilder());
            //this block recieve a sizeOfWord and generate some random word
            //with A-Z chars and 0-9 symbols in it. I did'nt came up with a better
            //example. 
            var transformBlock = new TransformBlock<int,string>(sizeOfWord=>{
                System.Console.WriteLine("Request {0} with {1} size of word",Task.CurrentId,sizeOfWord);
                var buff_ = buff.Value;
                var rand_ = rand.Value; 
                buff_.Clear();
                for(int j = 0; j<sizeOfWord;j++){
                    //approximately 20% of word's chars is a digit
                    if(rand.Value.Next()%10>7){
                        buff_.Append(rand_.Next()%10);
                        continue;
                    }
                    buff_.Append(Convert.ToChar(rand_.Next()%25+65));
                }
                return buff.Value.ToString();
            },options);
            //Summarize count of letters and sum of digits
            var actionBlock = new ActionBlock<string>(request=>{
                //do some stuff...
                Thread.Sleep(rand.Value.Next()%1000+500);
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