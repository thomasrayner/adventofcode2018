using System;
using System.IO;

namespace day1
{
    class Program
    {
        static void Main(string[] args)
        {
            int Frequency = 0;
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

            StreamReader File = new StreamReader(FilePath);
            while ((Line = File.ReadLine()) != null)
            {
                int ToAdd = Convert.ToInt32(Line);
                Frequency += ToAdd;
            }

            Console.WriteLine($"Final frequency: {Frequency}");
            Console.ReadKey();
        }
    }
}
