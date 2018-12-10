using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace day10
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter the file path");
            //string FilePath = Console.ReadLine();
            string FilePath = @"C:\Users\thrayner\Desktop\day10-input.txt";
            var Input = File.OpenText(FilePath).ReadToEnd();

            var Regex = new Regex(@"position=<\s?(-?\d+), \s?(-?\d+)> velocity=<\s?(-?\d+), \s?(-?\d+)>");
            var Points = Input.Split('\n').Select(x =>
                {
                    var Match = Regex.Match(x);
                    return (posx: int.Parse(Match.Groups[1].Value), posy: int.Parse(Match.Groups[2].Value),
                            velx: int.Parse(Match.Groups[3].Value), vely: int.Parse(Match.Groups[4].Value));
                })
                .ToList();

            var MinX = Points.Min(x => x.posx);
            var MinY = Points.Min(x => x.posy);
            var MaxX = Points.Max(x => x.posx);
            var MaxY = Points.Max(x => x.posy);

            var Seconds = 0;
            while (true)
            {
                var Holding = Points.Select(x => x).ToList();
                for (int i = 0; i < Points.Count; i++)
                {
                    var Point = Points[i];
                    Points[i] = (Point.posx + Point.velx, Point.posy + Point.vely, Point.velx, Point.vely);
                }

                var NewMinX = Points.Min(x => x.posx);
                var NewMinY = Points.Min(x => x.posy);
                var NewMaxX = Points.Max(x => x.posx);
                var NewMaxY = Points.Max(x => x.posy);

                if ((NewMaxX - NewMinX) > (MaxX - MinX) || (NewMaxY - NewMinY) > (MaxY - MinY))
                {
                    Console.WriteLine(Seconds);
                    for (var i = MinY; i <= MaxY; i++)
                    {
                        for (var j = MinX; j <= MaxX; j++)
                        {
                            Console.Write(Holding.Any(x => x.posy == i && x.posx == j) ? '#' : '.');
                        }

                        Console.WriteLine();
                    }

                    Console.ReadKey();
                }

                MinX = NewMinX;
                MinY = NewMinY;
                MaxX = NewMaxX;
                MaxY = NewMaxY;
                Seconds++;
            }
        }
    }
}
