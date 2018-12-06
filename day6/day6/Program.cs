using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day6
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter the file path");
            //string FilePath = Console.ReadLine();
            string FilePath = @"C:\Users\thrayner\Desktop\day6-input.txt";
            string[] Lines = File.ReadAllLines(FilePath);

            var Points = new List<List<int>>();
            var Matrix = new List<List<int>>();
            int BestDistance = -1;
            var BoundaryIndexes = new List<int>();
            var AreaCovered = new Hashtable();

            int MinX = 100;
            int MinY = 100;
            int MaxX = 0;
            int MaxY = 0;
            int LargestArea = 0;

            foreach (string Line in Lines)
            {
                var Point = new List<int>();
                foreach (string Element in Line.Split(','))
                {
                    Point.Add(Convert.ToInt32(Element.Trim()));
                }
                Points.Add(Point);
            }

            for (int i = 0; i < Points.Count(); i++)
            {
                if (Points[i][0] < MinX)
                    MinX = Points[i][0];

                if (Points[i][0] > MaxX)
                    MaxX = Points[i][0];

                if (Points[i][1] < MinY)
                    MinY = Points[i][1];

                if (Points[i][1] > MaxY)
                    MaxY = Points[i][1];
            }

            for (int i = 0; i < MaxX; i++)
            {
                var Column = new List<int>();
                for (int j = 0; j < MaxY; j++)
                {
                    int MinMan = 500;
                    var Equals = new List<int>();
                    for (int k = 0; k < Points.Count(); k++)
                    {
                        int PointDistance = CalculateManDistance(i, j, Points[k][0], Points[k][1]);
                        if (PointDistance <= MinMan)
                        {
                            MinMan = PointDistance;
                            BestDistance = k;

                            if (Equals.Contains(MinMan))
                                Equals.Add(MinMan);
                            else
                                Equals = new List<int> { MinMan };
                        }
                    }

                    if (Equals.Count() > 1)
                        Column.Add(-1);
                    else
                        Column.Add(BestDistance);
                }

                Matrix.Add(Column);
            }

            for (int i = 0; i < Matrix.Count(); i++)
            {
                for (int j = 0; j < Matrix[0].Count(); j++)
                {
                    if (i == 0 || i == Matrix[0].Count() || j == 0 || j == Matrix.Count())
                    {
                        BoundaryIndexes.Add(Matrix[i][j]);
                    }
                }
            }

            for (int i = 0; i < Matrix.Count(); i++)
            {
                for (int j = 0; j < Matrix[0].Count(); j++)
                {
                    if (!BoundaryIndexes.Contains(Matrix[i][j]))
                    {
                        if (AreaCovered[Matrix[i][j]] != null)
                            AreaCovered[Matrix[i][j]] = (int)AreaCovered[Matrix[i][j]] + 1;
                        else
                            AreaCovered[Matrix[i][j]] = 1;
                    }
                }
            }

            foreach (DictionaryEntry Row in AreaCovered)
            {
                if (((int)Row.Value) > LargestArea)
                {
                    LargestArea = ((int)Row.Value);
                }
            }
            Console.WriteLine($"The largest size that isn't infinite is {LargestArea}");

            Console.ReadKey();
        }

        public static int CalculateManDistance(int X1, int Y1, int X2, int Y2)
        {
            int Distance = Math.Abs(X1 - X2) + Math.Abs(Y1 - Y2);
            return Distance;
        }
    }
}
