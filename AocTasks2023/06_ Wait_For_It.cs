using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.Diagnostics.Runtime.Utilities;
using Microsoft.Diagnostics.Tracing.Parsers;
using Microsoft.Diagnostics.Tracing.Parsers.Kernel;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;

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
            var times = textData[0][13..].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(i => int.Parse(i)).ToArray();
            var distances = textData[1][12..].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(i => int.Parse(i)).ToArray();
            var time = long.Parse(textData[0][13..].Replace(" ", ""));
            var distance = long.Parse(textData[1][12..].Replace(" ", ""));
            Race = new RaceData { Time = time, Distance = distance };
            for (var i = 0; i < times.Length; i++)
            {
                var raceData = new RaceData { Time = times[i], Distance = distances[i] };
                Races.Add(raceData);
            }
        }
        string IAocTask.Solve1()
        {
            var result = 1L;
            for (int i = 0; i < Races.Count; i++)
            {
                RaceData race = Races[i];
                result *= GetWinCount(race);
            }
            Sol1 = result.ToString();
            Debug.Assert((Sol1 == "288") || (Sol1 == "2449062"));
            return Sol1;
        }
        private static long GetWinCount(RaceData race)
        {
            var high = race.Time;
            var low = 0L;
            var mid = 0L;
            while (true)
            {
                mid = low + (high - low) / 2;
                var dist = mid * (race.Time - mid);
                if (dist<=race.Distance && ((mid+1) * (race.Time - mid-1) > race.Distance))
                {
                    break;
                }
                else if (dist>race.Distance)
                {
                    high = mid;
                } 
                else
                {
                    low = mid;
                }
            }
            var start = mid + 1;
            var end = race.Time - start;
            return end-start+1;
        }
        string IAocTask.Solve2()
        {
            var wincnt = GetWinCount(Race); 
            Sol2 = wincnt.ToString();
            Debug.Assert((Sol2 == "71503") || (Sol2 == "33149631"));
            return Sol2;
        }
    }
}
