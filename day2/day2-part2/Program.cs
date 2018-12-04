using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace day2_part2
{
    class Program
    {
        static void Main(string[] args)
        {
            bool FoundKey = false;

            Console.WriteLine("Enter the file path");
            string FilePath = Console.ReadLine();
            string[] Lines = File.ReadAllLines(FilePath);

            for (int i = 0; i < Lines.Count(); i++)                 // get a line
            {
                if (!FoundKey)
                {
                    for (int j = 0; j < Lines.Count(); j++)             // compare it to the other lines
                    {
                        var SameChars = new List<char>();
                        for (int k = 0; k < Lines[i].Count(); k++)      // go through the chars in the line
                        {
                            if (Lines[i][k] == Lines[j][k])
                                SameChars.Add(Lines[i][k]);
                        }

                        if (SameChars.Count() == Lines[i].Count() - 1)
                        {
                            Console.WriteLine($"The key is {SameChars.ToString()} (from {Lines[i]} and {Lines[j]}");
                            FoundKey = true;
                        }
                    }
                }
            }

            Console.ReadKey();
        }
    }
}
