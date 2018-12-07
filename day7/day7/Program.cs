using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace day7
{
    public class Step
    {
        public string Id;
        public List<string> DependsOn;
        public bool IsDone;
        public int Duration;
        public int SecondsDone;

        public Step(string Id)
        {
            this.Id = Id;
            Duration = (int)Id.ToCharArray()[0] - 4;   // Each step takes 60 seconds + A=1, B=2 etc and since A = ASCII 65, minus 4 to get it's value of 61
            SecondsDone = 0;
            DependsOn = new List<string>();
            IsDone = false;
        }

        public void AddDependency (string Dependency)
        {
            DependsOn.Add(Dependency);
        }

        public void WorkSecond()
        {
            SecondsDone++;
            if (SecondsDone == Duration)
            {
                IsDone = true;
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter the file path");
            //string FilePath = Console.ReadLine();
            string FilePath = @"C:\Users\thrayner\Desktop\day7-input.txt";
            string[] Lines = File.ReadAllLines(FilePath);

            List<Step> Steps = new List<Step>();
            HashSet<string> StepIds = new HashSet<string>();
            StringBuilder AnswerPt1 = new StringBuilder();
            StringBuilder AnswerPt2 = new StringBuilder();
            int WorkerCount = 5;
            int Duration = 0;

            foreach (string Line in Lines)
            {
                string[] Elements = Line.Split();
                StepIds.Add(Elements[1]);
                var SeenStep = Steps.Where(x => x.Id == Elements[7]);
                if (SeenStep.Count() > 0)
                {
                    SeenStep.First().AddDependency(Elements[1]);
                }
                else
                {
                    Step AddStep = new Step(Elements[7]);
                    AddStep.AddDependency(Elements[1]);
                    Steps.Add(AddStep);
                }
            }

            #region pt1
            foreach (string Id in StepIds)
            {
                if (Steps.Where(x => x.Id == Id).Count() < 1)
                {
                    Step AddStep = new Step(Id);
                    Steps.Add(AddStep);
                }
            }

            foreach (Step step in Steps)
            {
                Console.WriteLine($"Step: {step.Id} depends on {string.Join(", ", step.DependsOn)}");
            }

            while (Steps.Where(x => x.IsDone == true).Count() < Steps.Count())
            {
                List<Step> DependsDone = new List<Step>();

                foreach (Step Step in Steps)
                {
                    if (Step.IsDone == false)
                    {
                        int UnfinishedDependsCount = 0;
                        foreach (string Depend in Step.DependsOn)
                        {
                            UnfinishedDependsCount += Steps.Where(x => x.Id == Depend && x.IsDone == false).Count();
                        }

                        if (UnfinishedDependsCount == 0)
                        {
                            DependsDone.Add(Step);
                        }
                    }
                }

                List<Step> SortedDependsDone = DependsDone.OrderBy(x => x.Id).ToList();
                SortedDependsDone.First().IsDone = true;
                Console.WriteLine($"Finishing {SortedDependsDone.First().Id}");
                AnswerPt1.Append(SortedDependsDone.First().Id);
            }

            Console.WriteLine($"The part 1 answer is {AnswerPt1.ToString()}");
            #endregion

            #region pt2
            // Reset for pt 2
            foreach (Step Step in Steps)
            {
                Step.IsDone = false;
            }

            while (Steps.Where(x => x.IsDone == true).Count() < Steps.Count())
            {
                List<Step> InProgress = Steps.Where(x => x.SecondsDone < x.Duration && x.SecondsDone > 0).ToList();
                
                if (InProgress.Count() != WorkerCount)
                {
                    List<Step> DependsDone = new List<Step>();
                    List<Step> StartSteps = new List<Step>();

                    foreach (Step Step in Steps)
                    {
                        if (Step.SecondsDone > 0)
                        {
                            continue;
                        }
                        int UnfinishedDependsCount = 0;
                        foreach (string Depend in Step.DependsOn)
                        {
                            UnfinishedDependsCount += Steps.Where(x => x.Id == Depend && x.IsDone == false).Count();
                        }

                        if (UnfinishedDependsCount == 0)
                        {
                            DependsDone.Add(Step);
                        }
                    }
                    List<Step> SortedDependsDone = DependsDone.OrderBy(x => x.Id).ToList();

                    if (SortedDependsDone.Count() >= WorkerCount - InProgress.Count())
                    {
                        
                        StartSteps = SortedDependsDone.Take(WorkerCount - InProgress.Count()).ToList();   
                    }
                    else
                    {
                        StartSteps = SortedDependsDone;
                    }
                    foreach (Step Step in StartSteps)
                    {
                        Step.WorkSecond();
                    }
                }

                foreach (Step Step in InProgress)
                {
                    Step.WorkSecond();
                    if (Step.IsDone)
                    {
                        AnswerPt2.Append(Step.Id);
                    }
                }
                Duration++;
            }

            Console.WriteLine($"The part 2 answer is {Duration}, new order: {AnswerPt2.ToString()}");

            #endregion

            Console.ReadKey();
        }
    }
}
