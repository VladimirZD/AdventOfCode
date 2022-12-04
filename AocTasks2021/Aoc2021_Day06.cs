using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;
using System.Diagnostics;

namespace AdventOfCode.AocTasks2021
{
    [AocTask(2021, 6)]
    public class Aoc2021_Day06 : IAocTask
    {
        public string FilePath { get; set; }
        public List<int> Lanterns { get; set; }
        public Aoc2021_Day06(string filePath)
        {
            FilePath= filePath;
        }
        public void PrepareData()
        {
            Lanterns = File.ReadAllText(FilePath).ToString().Split(",").Select(n => int.Parse(n)).ToList();
        }
        string IAocTask.Solve1()
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();
            var currentDay = 0;
            var days = 80;
            var lanterns = new List<int>(Lanterns);
            while (currentDay < days)
            {
                var newFishCnt = 0;
                for (var i = 0; i < lanterns.Count; i++)
                {
                    lanterns[i] = lanterns[i] - 1;
                    if (lanterns[i] < 0)
                    {
                        newFishCnt++;
                        lanterns[i] = 6;
                    }
                }
                for (var i = 0; i < newFishCnt; i++)
                {
                    lanterns.Add(8);
                }
                for (var i = 0; i < lanterns.Count; i++)
                {
                    if (lanterns[i] < 0)
                    {
                        lanterns[i] = 6;
                    }
                }
                currentDay++;
            }
            return lanterns.Count.ToString();
        }

        string IAocTask.Solve2()
        {
            var days = 256;
            var lantersByCycle = new List<long> { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            for (var i = 0; i < 8; i++)
            {
                lantersByCycle[i] = Lanterns.Where(l => l == i).Count();
            }
            var currentDay = 0;
            while (currentDay < days)
            {
                var fishToSpawn = lantersByCycle[0];
                //shift array to the left
                for (var j = 0; j < 8; j++)
                {
                    lantersByCycle[j] = lantersByCycle[j + 1];

                }
                /*Each day, a 0 becomes a 6 and adds a new 8 to the end of the list,*/
                lantersByCycle[6] += fishToSpawn;
                lantersByCycle[8] = fishToSpawn;
                currentDay++;
            }
            return lantersByCycle.Sum().ToString();
        }
    }
}