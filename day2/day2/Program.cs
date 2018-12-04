using System;
using System.IO;
using System.Linq;

namespace day2
{
    class Program
    {
        static void Main(string[] args)
        {
            int TwoCount = 0;
            int ThreeCount = 0;
            int CheckSum = 0;

            string Line = String.Empty;

            Console.WriteLine("Enter the file path");
            string FilePath = Console.ReadLine();

            StreamReader File = new StreamReader(FilePath);
            while ((Line = File.ReadLine()) != null)
            {
                var CharFrequency = Line.ToCharArray();
                var Doubles = CharFrequency.GroupBy(x => x)
                             .Where(g => g.Count() == 2)
                             .Select(g => g.Key);
                var Triples = CharFrequency.GroupBy(x => x)
                             .Where(g => g.Count() == 3)
                             .Select(g => g.Key);

                if (Doubles.Count() > 0)
                    TwoCount++;

                if (Triples.Count() > 0)
                    ThreeCount++;
            }

            CheckSum = ThreeCount * TwoCount;
            Console.WriteLine($"ThreeCount: {ThreeCount} \r\nTwoCount: {TwoCount} \r\nChecksum: {CheckSum}");
            Console.ReadKey();
        }
    }
}
