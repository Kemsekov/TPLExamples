using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Diagnostics;

namespace TPLLearn.Examples
{
    public static partial class TPLExamples
    {

        ///<summary>
        ///This example shows how lock is inefficient when it comes to a lot for sync and async parallel work
        ///</summary>
        public static void SemaphoreSlimExample(){
            int iterations = 1000;
            var watch = new Stopwatch();
            watch.Start();
            DoInLock(iterations);
            watch.Stop();
            System.Console.WriteLine("{0} in lock",watch.ElapsedMilliseconds);
            watch.Reset();
            watch.Start();
            DoInSemaphoreSlim(iterations);
            watch.Stop();
            System.Console.WriteLine("{0} in semaphore slim",watch.ElapsedMilliseconds);
        }
        ///<summary>
        ///Do some work in many threads with one piece of work that have to be done in sync
        ///</summary>
        static void DoInLock(int iterations){
            object mutex = new object();
            Parallel.For(0,iterations,(index,_)=>{
                //Do some work parallel
                Thread.Sleep(5);

                //here our current task will go to queue and waits release of lock.
                //in that wait in queue time task will do nothing. which will consume CPU time.
                lock(mutex){
                    //Do some work sync
                    Thread.Sleep(2);
                }
            });
        }
        static void DoInSemaphoreSlim(int iterations){
            var semaphore = new SemaphoreSlim(1);

            Parallel.For(0,iterations,async (index,_)=>{
                //Do some work parallel
                Thread.Sleep(5);
                //here our current task will see that it must wait until the task is finished
                //in that wait time we can do other staff with current task so now we
                //consume our CPU time properly
                await semaphore.WaitAsync();    
                    //Do some work sync
                    Thread.Sleep(2);
                semaphore.Release();
            });
        }
    }
}