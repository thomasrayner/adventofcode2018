using System;
using System.IO;
using System.Linq;

namespace day5
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter the file path");
            //string FilePath = Console.ReadLine();
            string FilePath = @"C:\Users\thrayner\Desktop\day5-input.txt";
            string[] Lines = File.ReadAllLines(FilePath);

            if (Lines.Count() > 1)
            {
                Console.WriteLine("Something went wrong with your input");
                Console.ReadKey();
                Environment.Exit(1);
            }

            foreach (string Line in Lines)
            {
                string Reduced = ReduceString(Line);
                int Before = 0;
                int After = 1;
                while (Before != After)
                {
                    Before = Reduced.Length;
                    Reduced = ReduceString(Reduced);
                    After = Reduced.Length;
                }
                Console.WriteLine($"Reduced String Length: {Reduced.Length}");
            }

            Console.ReadKey();
        }

        public static string ReduceString(string Reduced)
        {
            int PositionAhead = 1;
            foreach (char c in Reduced)
            {
                if (PositionAhead < Reduced.Length)
                {
                    if (char.IsLower(c) && (char.ToUpper(c) == Reduced[PositionAhead]))
                    {
                        Reduced = Reduced.Remove(PositionAhead - 1, 2);
                        break;
                    }
                    else if (char.IsUpper(c) && (char.ToLower(c) == Reduced[PositionAhead]))
                    {
                        Reduced = Reduced.Remove(PositionAhead - 1, 2);
                        break;
                    }
                    PositionAhead++;
                }
            }
            return Reduced;
        }
    }
}
