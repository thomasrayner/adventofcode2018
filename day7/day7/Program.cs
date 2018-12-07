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

        public Step(string Id)
        {
            this.Id = Id;
            this.DependsOn = new List<string>();
            this.IsDone = false;
        }

        public void AddDependency (string Dependency)
        {
            DependsOn.Add(Dependency);
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
            StringBuilder Answer = new StringBuilder();

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
                Answer.Append(SortedDependsDone.First().Id);
            }

            Console.WriteLine($"The part 1 answer is {Answer.ToString()}");
            Console.ReadKey();
        }
    }
}
