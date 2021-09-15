namespace TPLExamples.Examples
{
    public static partial class TPLExamples
    {
        static void CountDownEventExample()
        {
            CountdownEvent countObject = new CountdownEvent(10);
            int[] result = new int[10];


            for (int i = 0; i < 10; ++i)
            {
                int j = i;
                Task.Factory.StartNew(() =>
                    {

                        Thread.Sleep(TimeSpan.FromSeconds(3));
                        result[j] = j * 10;

                        countObject.Signal();
                    });
            }

            countObject.Wait();

            foreach (var r in result)
            {
                Console.WriteLine(r);
            }

            Console.ReadLine();

            /* Result
                * 0
                * 10
                * 20
                * 30
                * 40
                * 50
                * 60
                * 70
                * 80
                * 90 */
        }
    }
}