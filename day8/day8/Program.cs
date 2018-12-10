using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day8
{
    public class Node
    {
        public List<int> Metadata { get; set; } = new List<int>();
        public List<Node> Nodes { get; set; } = new List<Node>();

        public int MetadataSum ()
        {
            return Metadata.Sum() + Nodes.Sum(x => x.MetadataSum());
        }

        public int Value ()
        {
            if (!Nodes.Any())
            {
                return Metadata.Sum();
            }

            int Value = 0;

            foreach (int i in Metadata)
            {
                if (i <= Nodes.Count())
                {
                    Value += Nodes[i - 1].Value();
                }
            }

            return Value;
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter the file path");
            //string FilePath = Console.ReadLine();
            string FilePath = @"C:\Users\thrayner\Desktop\day8-input.txt";
            string[] Lines = File.ReadAllLines(FilePath);

            foreach (string Line in Lines)
            {
                List<int> Numbers = Line.Split(' ').Select(int.Parse).ToList();
                int Index = 0;
                Node Root = ReadNode(Numbers, ref Index);
                Console.WriteLine($"Part 1 metadata sum: {Root.MetadataSum()}");
            }

            Console.ReadKey();
        }

        public static Node ReadNode(List<int> Numbers, ref int Index)
        {
            Node Node = new Node();
            var Children = Numbers[Index++];
            var Metadata = Numbers[Index++];

            for (int i = 0; i < Children; i++)
            {
                Node.Nodes.Add(ReadNode(Numbers, ref Index));
            }

            for (int i = 0; i < Metadata; i++)
            {
                Node.Metadata.Add(Numbers[Index++]);
            }

            return Node;
        }
    }
}
