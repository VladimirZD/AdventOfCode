using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;
using Microsoft.CodeAnalysis;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Numerics;

namespace AdventOfCode.AocTasks2023
{
    [AocTask(2023, 8)]
    public class Haunted_Wasteland(string filePath) : IAocTask
    {
        public string FilePath { get; set; } = filePath;
        public string Sol1 { get; set; }
        public string Sol2 { get; set; }
        private struct Cave { public string L; public string R; }
        private new Dictionary<string, Cave> MapData { get; set; }
        //private new List<string> CurrentCaves1 { get; set; }
        private new List<string> CurrentCaves { get; set; }

        private string Instructions { get; set; }

        public void PrepareData()
        {

            var textData = File.ReadAllText(FilePath);
            //textData = "RL\nAAA = (BBB, CCC)\nBBB = (DDD, EEE)\nCCC = (ZZZ, GGG)\nDDD = (DDD, DDD)\nEEE = (EEE, EEE)\nGGG = (GGG, GGG)\nZZZ = (ZZZ, ZZZ)";
            //textData = "LLR\r\n\r\nAAA = (BBB, BBB)\r\nBBB = (AAA, ZZZ)\r\nZZZ = (ZZZ, ZZZ)".Replace("\r", "");
            //textData = "LR\r\n\r\n11A = (11B, XXX)\r\n11B = (XXX, 11Z)\r\n11Z = (11B, XXX)\r\n22A = (22B, XXX)\r\n22B = (22C, 22C)\r\n22C = (22Z, 22Z)\r\n22Z = (22B, 22B)\r\nXXX = (XXX, XXX)".Replace("\r", "");
            var caveData = textData.Replace("=", "").Replace("(", "").Replace(")", "").Replace(",", "").Split("\n", StringSplitOptions.RemoveEmptyEntries); ;
            CurrentCaves = new List<string>();
            Instructions = caveData[0];
            MapData = new Dictionary<string, Cave>();
            for (int i = 1; i < caveData.Length; i++)
            {
                var cave = caveData[i].Split(" ", StringSplitOptions.RemoveEmptyEntries);
                if (cave[0][^1] == 'A')
                {
                    CurrentCaves.Add(cave[0]);
                }
                MapData.Add(cave[0], new Cave { L = cave[1], R = cave[2] });
            }
        }
        string IAocTask.Solve1()
        {
            Func<string, bool> checkFunction = (cave) => cave == "ZZZ";
            var stepCount = DoTheWalk(CurrentCaves.Take(1).ToList(), checkFunction);
            Sol1 = stepCount.ToString();
            Debug.Assert((Sol1 == "6") || (Sol1 == "19637"));
            return Sol1;
        }
        private long DoTheWalk(List<string> caves, Func<string, bool> check)
        {
            var instructionIndex = 0;
            var stepCount = 0L;
            bool stopWalking = false;

            while (!stopWalking)
            {
                var newCaves = new List<string>();
                stopWalking = true;
                foreach (var cave in caves)
                {
                    var currentCave = MapData[cave];
                    var newCave = Instructions[instructionIndex] == 'L' ? currentCave.L : currentCave.R;
                    newCaves.Add((newCave));
                    stopWalking &= check(newCave);
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
            var individualSteps = new long[CurrentCaves.Count];
            for (var i = 0; i < CurrentCaves.Count; i++)
            {
                var stepCount = DoTheWalk(CurrentCaves.Skip(i).Take(1).ToList(), checkFunction);
                individualSteps[i]=stepCount;
            }
            BigInteger lcm = LCM(individualSteps);
            Sol2 = lcm.ToString();
            Debug.Assert((Sol2 == "5905") || (Sol2 == "8811050362409"));
            return Sol2;
        }
        static BigInteger LCM(long[] numbers)
        {
            BigInteger lcm = 1;
            for (int i = 0; i < numbers.Length; i++)
            {
                lcm = LCM(lcm, numbers[i]);
            }
            return lcm;
        }
        static BigInteger LCM(BigInteger a, BigInteger b)
        {
            return (a / BigInteger.GreatestCommonDivisor(a, b)) * b;
        }
    }
}

