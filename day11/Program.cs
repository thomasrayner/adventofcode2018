using System;
using System.Collections.Generic;
using System.Linq;

namespace day11
{
    public class FuelCell
    {
        public int X { get; set; }
        public int Y { get; set; }
        
        public FuelCell(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            int MaxPower = int.MinValue;
            int MaxLocX = 0;
            int MaxLocY = 0;
            int SerialNumber = 7400;

            for (int y = 1; y < 300; y++)
            {
                for (int x = 1; x < 300; x++)
                {
                    int CurrentPower = 0;
                    for (int j = 0; j < 3; j++)
                    {
                        for (int k = 0; k < 3; k++)
                        {
                            FuelCell Cell = new FuelCell(x + k, y + j);
                            CurrentPower += (((((Cell.X + 10) * Cell.Y) +  SerialNumber) * (Cell.X + 10)) / 100) % 10 - 5;
                        }
                    }

                    if (CurrentPower > MaxPower)
                    {
                        MaxPower = CurrentPower;
                        MaxLocX = x;
                        MaxLocY = y;
                    }
                }
            }

            Console.WriteLine($"The highest value is {MaxLocX},{MaxLocY} at {MaxPower}");

            // Part 2 - My math for detecting edgest wasn't working right - this is heavily influenced by a solution on reddit by andrewsredditstuff
            (int x, int y, int size, int power) maxSquare = (0, 0, 0, 0);
            (int[] squares, int[] rows, int[] cols)[,] grid = new (int[], int[], int[])[300, 300];
            
            for (int x = 300; x > 0; x--)
                for (int y = 300; y > 0; y--)
                {
                    int score = ((((((x + 10) * y) + SerialNumber) * (x + 10)) / 100) % 10) - 5;
                    int[] squares = new int[300], rows = new int[300], cols = new int[300];
                    for (int size = 0; size < 300; size++)
                    {
                        if (x-1 + size < 300)
                            rows[size] = size == 0 ? score : grid[x, y-1].rows[size - 1] + score;
                        if (y-1 + size < 300)
                            cols[size] = size == 0 ? score : grid[x-1, y].cols[size - 1] + score;
                        if (x-1 + size < 300 && y-1 + size < 300)
                            squares[size] = size == 0 ? score : grid[x, y].squares[size - 1] + rows[size] + cols[size] - score;
                        grid[x-1, y-1] = (squares, rows, cols);
                        if (squares[size] > maxSquare.power)
                            maxSquare = (x, y, size + 1, squares[size]);
                    }
                }

            Console.WriteLine($"Part 2: {maxSquare.x},{maxSquare.y},{maxSquare.size}");

            // My attempt at making the pt 2 code work with the way I *was* doing things
            // Seems to work, and just by having re-written it, I understand it way better... so I'm calling it a win
            FuelCell MaxCell = new FuelCell(0,0);
            int MaxSizeSquare = 0;
            int MaxSizePower = 0;
            (int[] Squares, int[] Rows, int[] Cols)[,] Grid = new (int[], int[], int[])[300, 300];
            for (int i = 300; i > 0; i--)
            {
                for (int j = 300; j > 0; j--)
                {
                    FuelCell Cell = new FuelCell(i, j);
                    int CurrentPower = (((((Cell.X + 10) * Cell.Y) +  SerialNumber) * (Cell.X + 10)) / 100) % 10 - 5;
                    int[] Squares = new int[300];
                    int[] Rows = new int[300];
                    int[] Cols = new int[300];

                    for (int Size = 0; Size < 300; Size++)
                    {
                        if (i - 1 + Size < 300)
                            Rows[Size] = Size == 0 ? CurrentPower : Grid[i, j - 1].Rows[Size - 1] + CurrentPower;
                        if (j - 1 + Size < 300)
                            Cols[Size] = Size == 0 ? CurrentPower : Grid[i - 1, j].Cols[Size - 1] + CurrentPower;
                        if (i - 1 + Size < 300 && j - 1 + Size < 300)
                            Squares[Size] = Size == 0 ? CurrentPower : Grid[i, j].Squares[Size - 1] + Rows[Size] + Cols[Size] - CurrentPower;

                        Grid[i - 1, j - 1] = (Squares, Rows, Cols);
                        if (Squares[Size] > MaxSizePower)
                        {   MaxCell = new FuelCell(i, j);
                            MaxSizeSquare = Size + 1;
                            MaxSizePower = Squares[Size];
                        }
                    }
                }
            }
             Console.WriteLine($"Part 2: {MaxCell.X},{MaxCell.Y},{MaxSizeSquare} at {MaxSizePower}");
        }
    }
}
