// I'm going to actually add comments to this one to prove I know how it works, since I depended heavily on translating a Python example from CallMeTarush
// Yes it is **WAY** over-commented

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
            // Retrieve and load the input - normally ask for a file path, but I've hardcoded it for convenience while testing since I can get away with that here
            Console.WriteLine("Enter the file path");
            //string FilePath = Console.ReadLine();
            string FilePath = @"C:\Users\thrayner\Desktop\day6-input.txt";
            string[] Lines = File.ReadAllLines(FilePath);

            var Points = new List<List<int>>();         // Holds all of the points, parsed out of the file - each point itself is an x and y, stored in a list inside this list
            var Matrix = new List<List<int>>();         // The matrix we're building, putting points in and calculating which spots in the matrix are closest to which point
            int BestDistance = -1;                      // The "best distance" is used in loops to determine which point is closest to a given square on the matrix
            var BoundaryIndexes = new List<int>();      // Holds the coordinates of the edges of the matrix
            var AreaCovered = new Hashtable();          // Maps each point to the area of the matrix where that point "owns" it outright

            // Contain information about the size of the matrix before we really know it
            int MinX = 100;
            int MinY = 100;
            int MaxX = 0;
            int MaxY = 0;

            int LargestArea = 0;                        // The largest non-infinate area of squares on the matrix owned by one point
            int RegionSize = 0;                         // The size of the region that is considered "safe" - has less than 10000 squares to all coordinates

            // Load up the points into the Points list - split up the line into it's x and y components (stored as x, y in the input so it also needs to be trimmed)
            // and then add those to a list (the individual point) which is then added to the list of all points
            foreach (string Line in Lines)
            {
                var Point = new List<int>();
                foreach (string Element in Line.Split(','))
                {
                    Point.Add(Convert.ToInt32(Element.Trim()));
                }
                Points.Add(Point);
            }

            // Go through all the points to determine the largest and smallest X and Y coords
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

            // Now it's time to build the matrix - going through all the rows and columns to create a correctly sized matrix
            for (int i = 0; i < MaxX; i++)
            {
                var Column = new List<int>();                   // I think this should actually be named "Row" but I named it "Column" based off the Python example I translated
                for (int j = 0; j < MaxY; j++)
                {
                    int MinMan = 500;
                    var Equals = new List<int>();

                    // For each square on the matrix (the i and j loops), go through each point and find which points are closest to that square
                    // If the point being examined is closer than the "MinMan" distance, then that's the closest point to the square (becomes the new min distance)
                    for (int k = 0; k < Points.Count(); k++)
                    {
                        int PointDistance = CalculateManDistance(i, j, Points[k][0], Points[k][1]);
                        if (PointDistance <= MinMan)
                        {
                            MinMan = PointDistance;
                            BestDistance = k;

                            // If there's a tie, then add them both, and we'll sort it out in a few lines
                            if (Equals.Contains(MinMan))
                                Equals.Add(MinMan);
                            else
                                Equals = new List<int> { MinMan };
                        }
                    }

                    // Here's where we sort out squares that are closest to more than one point - we're going to ignore them
                    if (Equals.Count() > 1)
                        Column.Add(-1);
                    else
                        Column.Add(BestDistance);
                }

                Matrix.Add(Column);
            }

            // We need to go through the matrix we just built and find the boundaries so that we can detect which points have an infinate area
            for (int i = 0; i < Matrix.Count(); i++)
            {
                for (int j = 0; j < Matrix[0].Count(); j++)
                {
                    // If we're on the 0 index of a row or column, or the max value of a row or a column, then that's the edge of the matrix
                    if (i == 0 || i == Matrix[0].Count() || j == 0 || j == Matrix.Count())
                    {
                        BoundaryIndexes.Add(Matrix[i][j]);
                    }
                }
            }

            // Time to walk through the matrix again to find the size of each area owned by a point - literally just counting all the squares with the point ID in it
            for (int i = 0; i < Matrix.Count(); i++)
            {
                for (int j = 0; j < Matrix[0].Count(); j++)
                {
                    // We want to ignore squares that are along the edge of the matrix since we know those areas are infinite
                    if (!BoundaryIndexes.Contains(Matrix[i][j]))
                    {
                        // If we haven't seen this point yet, we'll set it's area count equal to 1, otherwise we'll increment it by 1
                        // We have to do some funky casting to work with int values in a hashtable
                        if (AreaCovered[Matrix[i][j]] != null)
                            AreaCovered[Matrix[i][j]] = (int)AreaCovered[Matrix[i][j]] + 1;
                        else
                            AreaCovered[Matrix[i][j]] = 1;
                    }
                }
            }

            // Looking through the hashtable of points and the areas they cover to identify which one has the largest area
            foreach (DictionaryEntry Row in AreaCovered)
            {
                if (((int)Row.Value) > LargestArea)
                {
                    LargestArea = ((int)Row.Value);
                }
            }

            // Walking the entire matrix again to find squares that are less than 10000 squares away from all points
            // When we find a square that's less than 10000 squares from all points, we increment RegionSize since
            // we don't care about the coords of that region, just the size of it
            // The rules for this in the challenge are super arbitrary (as they all are)
            for (int i = 0; i < Matrix.Count(); i++)
            {
                for (int j = 0; j < Matrix[0].Count(); j++)
                {
                    // For each square on the matrix, calculate it's total distance from all points, and if it's less than 10000, that one counts
                    int TotalDistance = 0;
                    for (int k = 0; k < Points.Count(); k++)
                    {
                        TotalDistance += CalculateManDistance(i, j, Points[k][0], Points[k][1]);
                    }
                    if (TotalDistance < 10000)
                        RegionSize++;
                }
            }

            // Declare victory
            Console.WriteLine($"The largest size that isn't infinite is {LargestArea}. The safe region size is {RegionSize}");
            Console.ReadKey();
        }

        // All this thing does is take in X and Y coords of two locations and calculates the Manhattan Distance between them
        // which is the absolute value of the difference between the X and Y added together according to Wikipedia
        public static int CalculateManDistance(int X1, int Y1, int X2, int Y2)
        {
            int Distance = Math.Abs(X1 - X2) + Math.Abs(Y1 - Y2);
            return Distance;
        }
    }
}
