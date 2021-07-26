using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using TPLLearn.Examples;

namespace TPLLearn
{

    class Program
    {
        static void Main(string[] args)
        {
            TPLExamples.SemaphoreSlimExample();
        }
    }
}
