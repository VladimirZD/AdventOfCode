using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;
using BenchmarkDotNet.Disassemblers;
using System.Diagnostics;

namespace AdventOfCode.AocTasks2023
{
    [AocTask(2023, 2)]
    public class Cube_Conundrum(string filePath) : IAocTask
    {
        public string FilePath { get; set; } = filePath;
        public string Sol1 { get; set; }
        public string Sol2 { get; set; }
        public string[] GameData { get; set; }
        public void PrepareData()
        {
            GameData = File.ReadAllLines(FilePath);
        }
        string IAocTask.Solve1()
        {
            var maxRed = 12;
            var maxGreen = 13;
            var maxBlue = 14;
            bool isGameValid;
            var totalSum = 0;
            var totalGamePower = 0;
            foreach (var game in GameData)
            {
                var data = game.Split(":");
                var gameID = int.Parse(data[0].Replace("Game", ""));
                var rounds = data[1].Split(";");
                isGameValid = true;
                var minRed = 0;
                var minGreen = 0;
                var minBlue = 0;
                int redCnt;
                int blueCnt;
                int greenCnt ;
                foreach (var round in rounds)
                {
                    var colors = round.Split(",");
                    redCnt = 0;
                    blueCnt = 0;
                    greenCnt = 0;
                    foreach (var color in colors)
                    {
                        var colorData = color.Trim().Split(" ");
                        var colValue = colorData[1];
                        var cubesPlayed = int.Parse(colorData[0]);
                        switch (colValue)
                        {
                            case "red":
                                redCnt += cubesPlayed;
                                minRed = int.Max(minRed, cubesPlayed);
                                break;
                            case "green":
                                greenCnt += cubesPlayed;
                                minGreen = int.Max(minGreen, cubesPlayed);
                                break;
                            case "blue":
                                blueCnt += cubesPlayed;
                                minBlue = int.Max(minBlue, cubesPlayed);
                                break;
                        }
                    }
                    if (blueCnt > maxBlue || redCnt > maxRed || greenCnt > maxGreen)
                    {
                        isGameValid = false;
                    }
                }
                
                totalGamePower +=minRed*minBlue*minGreen;
                if (isGameValid)
                {
                    totalSum += gameID;
                }
            }
            Sol1 = totalSum.ToString();
            Sol2 = totalGamePower.ToString();
            Debug.Assert(totalSum == 1867);
            Debug.Assert(totalGamePower == 84538);
            return Sol1;
        }
        string IAocTask.Solve2()
        {
            return Sol2;
        }
    }
}
