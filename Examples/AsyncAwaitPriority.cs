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
        ///run two cycles in async for 5 seconds with priority. The more priority the more time lends to method
        ///</summary>
        public static async Task AsyncAwaitPriority(int priority_for_method_one,int priority_for_method_two){
            var token = new CancellationTokenSource();            
            
            var task1 = Method(token.Token,"One",priority_for_method_one);
            var task2 = Method(token.Token,"Two",priority_for_method_two);
            await Task.Delay(5000);
            token.Cancel();
            await task1;
            await task2;
            return;
            //the more prioprity the more often this method will work
            async Task Method(CancellationToken token,string str, int priority){
                int c = 0;
                while(!token.IsCancellationRequested){
                    c++;
                    System.Console.WriteLine($"Method {str} {c}");
                    if(c%priority==0)
                        await Task.Delay(100);
                    }
            }
        }
    }
}