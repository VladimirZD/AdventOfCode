using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.Diagnostics.Runtime.Utilities;
using Microsoft.Diagnostics.Tracing.Parsers;
using Microsoft.Diagnostics.Tracing.Parsers.Kernel;
using System.Diagnostics;

namespace AdventOfCode.AocTasks2023
{
    [AocTask(2023, 6)]
    public class Wait_For_It(string filePath) : IAocTask
    {
        public string FilePath { get; set; } = filePath;
        public string Sol1 { get; set; }
        public string Sol2 { get; set; }
        private List<RaceData> Races;
        private RaceData Race; 
        private struct RaceData{ public long Time, Distance,WaysToWin; }
        public void PrepareData()
        {
            var textData = File.ReadAllLines(FilePath).Where(l => !string.IsNullOrEmpty(l)).ToArray();
            Races = new List<RaceData>();
            //textData = "Time:      7  15   30\nDistance:  9  40  200".Split("\n", StringSplitOptions.RemoveEmptyEntries);
            var times= textData[0].Replace("Time:      ", "").Split(" ",StringSplitOptions.RemoveEmptyEntries).Select(i=>int.Parse(i)).ToArray();
            var distances= textData[1].Replace("Distance:  ", "").Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(i => int.Parse(i)).ToArray();
            var time = long.Parse(textData[0].Replace("Time:      ", "").Replace(" ", ""));
            var distance = long.Parse(textData[1].Replace("Distance: ", "").Replace(" ", ""));
            Race = new RaceData { Time = time, Distance = distance };
            for (var i=0;i<times.Length;i++)
            {
                var raceData = new RaceData{ Time = times[i], Distance = distances[i] };
                Races.Add(raceData);
            }
            
        }
        string IAocTask.Solve1()
        {
            var result = 1;
            
            for (int i = 0; i < Races.Count; i++)
            {
                RaceData race = Races[i];
                int winCnt = GetWinVariantsCount(race);
                if (winCnt > 0)
                {
                    result = result * winCnt;
                }
            }
            Sol1 = result.ToString();
            Debug.Assert((Sol1 == "288") || (Sol1 == "2449062"));
            return Sol1;
        }
        private static int GetWinVariantsCount(RaceData race)
        {
            
            var winCnt = 0;
            for (var t = 1; t <= race.Time; t++)
            {
                if (t * (race.Time - t) > race.Distance)
                {
                    winCnt++;
                }
                else if (winCnt > 0)
                {
                    //There is point when we start winning, and we are winning for time x, after we loose first one we can stop search...
                    break;
                }

                //Console.WriteLine($"Time passed {t}, Result: {t * (race.Time - t) > race.Distance} ");
            }
            return winCnt;
        }
        string IAocTask.Solve2()
        {
            var wincnt = GetWinVariantsCount(Race);
            Sol2 = wincnt.ToString();
            Debug.Assert((Sol2 == "71503") || (Sol2 == "33149631"));
            return Sol2;
        }
    }
}
