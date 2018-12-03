using System;
using System.Collections.Generic;
using System.IO;

namespace day1
{
    class Program
    {
        static void Main(string[] args)
        {
            int Frequency = 0;
            var FrequencyHistory = new HashSet<int>();
            bool FoundRepeat = false;
            string FilePath;
            string Line;

            Console.WriteLine("Enter the file path");
            FilePath = Console.ReadLine();

            if (String.IsNullOrEmpty(FilePath))
            {
                Console.WriteLine("Error getting file");
                Console.ReadKey();
                Environment.Exit(1);
            }

            while (!FoundRepeat)
            {
                StreamReader File = new StreamReader(FilePath);
                while ((Line = File.ReadLine()) != null)
                {
                    if (!FrequencyHistory.Add(Frequency) && !FoundRepeat)
                    {
                        Console.WriteLine($"Already found {Frequency}");
                        FoundRepeat = true;
                    }
                    int ToAdd = Convert.ToInt32(Line);
                    Frequency += ToAdd;
                }
            }
            
            Console.WriteLine($"Final frequency: {Frequency}");
            Console.ReadKey();
        }
    }
}
