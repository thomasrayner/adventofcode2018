using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

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

            string IdPattern = @"^\#\d+";
            string OffsetPattern = @"\d+,\d+";
            string AreaPattern = @"\d+x\d+$";

            int CanvasSize = 2048;
            string[,] Canvas = new string[CanvasSize, CanvasSize];
            int OverlapCount = 0;
            var ClaimTracker = new HashSet<string>();

            foreach (string Line in Lines)
            {
                string Id = Regex.Match(Line, IdPattern).Value;
                string[] OffsetString = Regex.Match(Line, OffsetPattern).Value.Split(',');
                string[] AreaString = Regex.Match(Line, AreaPattern).Value.Split('x');
                int[] Offset = new int[] { Convert.ToInt32(OffsetString[0]), Convert.ToInt32(OffsetString[1]) };
                int[] Area = new int[] { Convert.ToInt32(AreaString[0]), Convert.ToInt32(AreaString[1]) };

                ClaimTracker.Add(Id);

                for (int i = Offset[0]; i < Offset[0] + Area[0]; i++)
                {
                    for (int j = Offset[1]; j < Offset[1] + Area[1]; j++)
                    {
                        if (Canvas[i,j] != null)
                        {
                            ClaimTracker.Remove(Id);
                            foreach (string Overlap in Canvas[i,j].Split(',').Where(x => x != String.Empty))
                            {
                                ClaimTracker.Remove(Overlap);
                            }
                        }
                        Canvas[i, j] += $"{Id},";
                    }
                }
            }

            for (int i = 0; i < CanvasSize; i++)
            {
                for (int j = 0; j < CanvasSize; j++)
                {
                    if (!String.IsNullOrEmpty(Canvas[i,j]))
                    {
                        string[] Ids = Canvas[i, j].Split(',');
                        if (Ids.Where(x => x != String.Empty).Count() > 1)
                        {
                            OverlapCount++;
                        }
                    }
                }
            }

            Console.WriteLine($"OverlapCount: {OverlapCount}. Clean claim: {string.Join("", ClaimTracker)}");
            Console.ReadKey();
        }
    }
}
