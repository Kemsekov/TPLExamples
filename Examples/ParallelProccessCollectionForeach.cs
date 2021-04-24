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

        public static void ParallelProccessCollectionForeach(CancellationToken token){
            Stopwatch time = new();
            var source = Enumerable.Range(0,100000).ToArray();
            var rangePartitioner = Partitioner.Create(0,source.Length);
            var result = new string[source.Length];
            const int string_size = 2000;
            time.Start();
            Parallel.ForEach(rangePartitioner,(range,loopState)=>{
                var rand = new Random();
                var buff = new StringBuilder();
                for(int i = range.Item1;i<range.Item2 && !token.IsCancellationRequested;i++){
                    buff.Clear();
                    for(int c = 0;c<string_size;c++){
                        char cr = Convert.ToChar(rand.Next()%25+97);
                        buff.Append(cr);
                    }
                    result[i] = source[i]+buff.ToString().ToLower();
                }
            });
            time.Stop();
            Console.WriteLine(nameof(ParallelProccessCollectionForeach) 
                            +" executed with "+time.ElapsedMilliseconds 
                            + " Milliseconds");
        }
    }
}