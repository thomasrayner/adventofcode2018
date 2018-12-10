// Thank you reddit user Cppl_Lee for the C# linked list example - I knew I needed them but wasn't sure how to implement right

using System;
using System.Collections.Generic;
using System.Linq;

namespace day9
{
    class Program
    {
        static int PlayerCount = 423; // From input
        static int MarblePoint = 71944; // From input

        static long[] Scores = new long[PlayerCount];
        static LinkedList<int> PlacedMarbles = new LinkedList<int>();
        static LinkedListNode<int> CurrentMarble = PlacedMarbles.AddFirst(0);

        public static void NextMarble()
        {
            CurrentMarble = CurrentMarble.Next ?? PlacedMarbles.First;
        }
        public static void PreviousMarble()
        {
            CurrentMarble = CurrentMarble.Previous ?? PlacedMarbles.Last;
        }
        static void Main(string[] args)
        {
            for (int i = 0; i < MarblePoint; ++i)
            {
                if (((i + 1) % 23) == 0)
                {
                    PreviousMarble(); PreviousMarble(); PreviousMarble(); PreviousMarble();
                    PreviousMarble(); PreviousMarble(); PreviousMarble();

                    int j = i % PlayerCount;
                    Scores[j] += i + 1 + CurrentMarble.Value;

                    LinkedListNode<int> tmp = CurrentMarble;
                    NextMarble();
                    PlacedMarbles.Remove(tmp);
                }
                else
                {
                    NextMarble();
                    CurrentMarble = PlacedMarbles.AddAfter(CurrentMarble, i + 1);
                }
            }

            Console.WriteLine($"The max value is {Scores.Max()}");

            Console.ReadKey();
        }

        
    }
}
