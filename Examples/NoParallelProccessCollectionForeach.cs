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
        public static void NoParallelProccessCollectionForeach(CancellationToken token){
            Stopwatch time = new();
            var rand = new Random();
            var source = Enumerable.Range(0,100000).ToArray();
            var result = new string[source.Length];
            var buff = new StringBuilder();
            const int string_size = 2000;

            time.Start();
            for(int i = 0;i<source.Length && !token.IsCancellationRequested;i++){
                buff.Clear();
                for(int c = 0;c<string_size;c++){
                    char cr = Convert.ToChar(rand.Next()%25+97);
                    buff.Append(cr);
                }
                result[i] = source[i]+buff.ToString().ToLower();
            }
            time.Stop();

            Console.WriteLine(nameof(NoParallelProccessCollectionForeach) 
                            +" executed with "+time.ElapsedMilliseconds 
                            + " Milliseconds");
        }
    }
}