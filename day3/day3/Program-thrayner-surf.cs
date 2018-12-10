using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;

namespace day3
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter the file path");
            //string FilePath = Console.ReadLine();
            string FilePath = @"C:\Users\thrayner\Desktop\day3-input.txt";
            string[] Lines = File.ReadAllLines(FilePath);

            int CanvasSize = 4096;
            int[,] MasterLayout = new int[CanvasSize, CanvasSize];
            int OverlapCount = 0;

            for (int i = 0; i < CanvasSize; i++)
            {
                for (int j = 0; j < CanvasSize; j++)
                {
                    MasterLayout[i, j] = 0;
                }
            }

            foreach (string Line in Lines)
            {
                string[] OffsetValues = Regex.Match(Line, @"(?<=\s)\d+?,\d+?").Value.Split(',');
                string[] AreaValues = Regex.Match(Line, @"(?<=\s)\d+?x\d+?$").Value.Split('x');
                int[] Offset = new int[] { Convert.ToInt32(OffsetValues[0]), Convert.ToInt32(OffsetValues[1]) };
                int[] Area = new int[] { Convert.ToInt32(AreaValues[0]), Convert.ToInt32(AreaValues[1]) };

                for (int i = Offset[0]; i < Offset[0] + Area[0]; i++)
                {
                    for (int j = Offset[1]; j < Offset[1] + Area[1]; j++)
                    {
                        if (MasterLayout[i, j] == 1)
                            MasterLayout[i, j] = 2;
                        else
                            MasterLayout[i, j] = 1;
                    }
                }
            }

            for (int i = 0; i < CanvasSize; i++)
            {
                for (int j = 0; j < CanvasSize; j++)
                {
                    if (MasterLayout[i, j] == 2)
                        OverlapCount++;
                }
            }

            Console.WriteLine($"Overlaps: {OverlapCount}");
            Console.ReadKey();
        }
    }
}
