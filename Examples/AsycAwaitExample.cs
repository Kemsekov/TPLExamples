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
        public async static Task AsycAwaitExample(){
            System.Console.WriteLine("Beginning main");
            await Task.Delay(100);
            var task1 = Method();
            System.Console.WriteLine("Main do some work while Method is executing");
            System.Console.WriteLine("Main Did some work and awaits Method");
            await task1;
            System.Console.WriteLine("End main");
            return;
            async Task Method(){
                System.Console.WriteLine("Method Beginning");
                System.Console.WriteLine("Method awaits some work and return thread execution to main");
                await Task.Delay(100);
                System.Console.WriteLine("Method awaited and received executiong thread");
            }
        }
    }
}