using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;
using Microsoft.CodeAnalysis;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;

namespace AdventOfCode.AocTasks2023
{
    [AocTask(2023, 8)]
    public class Haunted_Wasteland(string filePath) : IAocTask
    {
        public string FilePath { get; set; } = filePath;
        public string Sol1 { get; set; }
        public string Sol2 { get; set; }
        private  struct Cave { public string L;public string R; }
        private new Dictionary<string, Cave> MapData { get; set; }
        private new List<string>  CurrentCaves1{ get; set; }
        private new List<string> CurrentCaves2 { get; set; }

        private string Instructions { get; set; }

        public void PrepareData()
        {
        
            var textData = File.ReadAllText(FilePath);
            //textData = "RL\nAAA = (BBB, CCC)\nBBB = (DDD, EEE)\nCCC = (ZZZ, GGG)\nDDD = (DDD, DDD)\nEEE = (EEE, EEE)\nGGG = (GGG, GGG)\nZZZ = (ZZZ, ZZZ)";
            //textData = "LLR\r\n\r\nAAA = (BBB, BBB)\r\nBBB = (AAA, ZZZ)\r\nZZZ = (ZZZ, ZZZ)".Replace("\r", "");
            //textData = "LR\r\n\r\n11A = (11B, XXX)\r\n11B = (XXX, 11Z)\r\n11Z = (11B, XXX)\r\n22A = (22B, XXX)\r\n22B = (22C, 22C)\r\n22C = (22Z, 22Z)\r\n22Z = (22B, 22B)\r\nXXX = (XXX, XXX)".Replace("\r", "");
            var caveData = textData.Replace("=", "").Replace("(", "").Replace(")", "").Replace(",", "").Split("\n", StringSplitOptions.RemoveEmptyEntries); ;
            CurrentCaves1= new List<string>();
            CurrentCaves2 = new List<string>();
            CurrentCaves1.Add("AAA");
            
            Instructions = caveData[0];
            
            MapData = new Dictionary<string, Cave>();
            for (int i = 1; i < caveData.Length; i++)
            {
                var cave = caveData[i].Split(" ", StringSplitOptions.RemoveEmptyEntries);
                if (cave[0][^1]=='A')
                {
                    CurrentCaves2.Add(cave[0]);
                }
                MapData.Add(cave[0], new Cave { L = cave[1], R = cave[ 2] });
            }
        }
        string IAocTask.Solve1()
        {
            Func<string, bool> checkFunction = (cave) => cave == "ZZZ";
            var stepCount = DoTheWalk(CurrentCaves1,checkFunction);
            Sol1 = stepCount.ToString();
            Debug.Assert((Sol1 == "6") || (Sol1 == "19637"));
            return Sol1;
        }
        private long DoTheWalk(List<string> caves, Func<string, bool> check)
        {
            //string currentCave = START_CAVE;
            var instructionIndex = 0;
            var stepCount = 0L;
            //while (currentCave != END_CAVE)
            //{
            //    var nextCave = MapData[currentCave];
            //    currentCave = Instructions[instructionIndex] == 'L' ? nextCave.L : nextCave.R;
            //    instructionIndex++;
            //    instructionIndex = instructionIndex % Instructions.Length;
            //    stepCount++;
            //}
            bool stopWalking = false;

            while (!stopWalking)
            {
                var newCaves = new List<string>();
                stopWalking = true;
                var goodCaves = 0;
                foreach (var cave in caves)
                {
                    var currentCave = MapData[cave];
                    var newCave = Instructions[instructionIndex] == 'L' ? currentCave.L : currentCave.R;
                    newCaves.Add((newCave));
                    stopWalking &= check(newCave);
                    if (check(newCave))
                    {
                        goodCaves++;
                    }
                }
                if (goodCaves > 2)
                {
                    Console.WriteLine($"Found good caves {goodCaves} at step {stepCount} all caves {string.Join(", ", newCaves.ToArray())}");
                }   
                instructionIndex++;
                instructionIndex %= Instructions.Length;
                stepCount++;
                caves = newCaves;
            }
            return stepCount;
        }

        string IAocTask.Solve2()
        {
            Func<string, bool> checkFunction = (cave) => cave[^1] == 'Z';
            var stepCount = DoTheWalk(CurrentCaves2,  checkFunction);
            Sol2 = stepCount.ToString();
            Debug.Assert((Sol2 == "5905") || (Sol2 == "251135960"));
            return Sol2;
        }
    }
}

