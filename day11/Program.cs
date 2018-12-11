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
        }
    }
}
