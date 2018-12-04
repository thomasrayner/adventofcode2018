using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace day4
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter the file path");
            //string FilePath = Console.ReadLine();
            string FilePath = @"C:\Users\thrayner\Desktop\day4-input.txt";
            string[] Lines = File.ReadAllLines(FilePath);
            SortedDictionary<DateTime, string> OrderedLines = new SortedDictionary<DateTime, string>();
            Hashtable GuardAnalysis = new Hashtable();

            string DateTimePattern = @"(?<=\[)[^]]+";
            string MessagePattern = @"(?<=\]\s)[\w\#\s]+$";
            string GuardNumberPattern = @"(?<=Guard\s\#)\d+";
            string MessageIdPattern = @"\w+?$";

            string GuardNumber = String.Empty;
            int StartMinute = -1;
            int EndMinute = -1;
            int MostMinutes = 0;
            int SleepiestGuard = -1;
            int MostSleptMinute = -1;

            foreach (string Line in Lines)
            {
                string TimestampString = Regex.Match(Line, DateTimePattern).Value;
                DateTime Timestamp = DateTime.Parse(TimestampString);
                string Message = Regex.Match(Line, MessagePattern).Value;

                OrderedLines.Add(Timestamp, Message);
            }

            foreach (var Line in OrderedLines)
            {
                string GuardCheck = Regex.Match(Line.Value, GuardNumberPattern).Value;
                if (GuardCheck != String.Empty)                    // This is a guard starting their shift
                {
                    GuardNumber = GuardCheck;
                    StartMinute = -1;
                    EndMinute = -1;

                    if (!GuardAnalysis.Contains(GuardNumber))
                    {
                        GuardAnalysis[GuardNumber] = new List<int>();
                    }
                }
                else                                               // Guard is either waking up or falling asleep
                {
                    string MessageId = Regex.Match(Line.Value, MessageIdPattern).Value;
                    
                    switch (MessageId)
                    {
                        case "asleep":
                            StartMinute = Line.Key.Minute;
                            break;

                        case "up":
                            EndMinute = Line.Key.Minute;
                            break;

                        case "shift":
                            Console.WriteLine($"This should not be possible: {Line.Key} - {Line.Value}");
                            break;
                        
                        default:
                            Console.WriteLine($"This one is broken: {Line.Key} - {Line.Value}");
                            break;
                    }

                    if (StartMinute > 0 && EndMinute > 0)   // If we have both a start and an end minute, the guard has fallen asleep and woken up
                    {
                        for (int i = StartMinute; i < EndMinute; i++)
                        {
                            ((List<int>)GuardAnalysis[GuardNumber]).Add(i);
                        }
                        StartMinute = -1;
                        EndMinute = -1;
                    }
                }
            }

            foreach (DictionaryEntry row in GuardAnalysis)
            {
                int MostSleptThisGuard = ((List<int>)row.Value).GroupBy(x => x).OrderByDescending(y => y.Count()).Select(z => z.Key).FirstOrDefault();
                int TimesSlept = ((List<int>)row.Value).Where(x => x == MostSleptThisGuard).Count();
                if (((List<int>)row.Value).Count > MostMinutes)
                {
                    MostMinutes = ((List<int>)row.Value).Count;
                    SleepiestGuard = Convert.ToInt32(row.Key);
                    MostSleptMinute = MostSleptThisGuard;
                }

                Console.WriteLine($"Guard: {row.Key} Number of minutes: {((List<int>)row.Value).Count} - their most slept minute: {MostSleptThisGuard} at {TimesSlept} (product: {Convert.ToInt32(SleepiestGuard) * MostSleptThisGuard})");
            }

            Console.WriteLine($"The sleepiest guard is {SleepiestGuard} with {MostMinutes} slept. Their most slept minute is {MostSleptMinute}. The product of these numbers is {Convert.ToInt32(SleepiestGuard) * MostSleptMinute}");

            Console.ReadKey();
        }
    }
}
