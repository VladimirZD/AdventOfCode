using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;

namespace AdventOfCode.AocTasks2022
{
    [AocTask(2022, 4, "Camp Cleanup")]
    public class Camp_Cleanup : IAocTask
    {
        private string[][] ZonesRanges { get; set; }
        private int PartialOverLap { get; set; }
        public string FilePath { get; set; }
        public Camp_Cleanup(string filePath)
        {
            FilePath = filePath;
        }
        public void PrepareData()
        {
            //var input= "2-4,6-8\r\n2-3,4-5\r\n5-7,7-9\r\n2-8,3-7\r\n6-6,4-6\r\n2-6,4-8".Split("\r\n");
            var input = System.IO.File.ReadAllText(FilePath).Split("\n");
            ZonesRanges = input.Where(l => !string.IsNullOrEmpty(l)).Select(l => l.Split(",")).ToArray();
        }
        string IAocTask.Solve1()
        {
            var fullOverLap = 0;
            var zoneRangesAsSpan = ZonesRanges.AsSpan();
            foreach (var zoneRange in zoneRangesAsSpan)
            {
                var zone1Data = zoneRange[0].Split("-").Select(i => int.Parse(i)).ToArray();
                var zone2Data = zoneRange[1].Split("-").Select(i => int.Parse(i)).ToArray();
                if ((zone1Data[0] <= zone2Data[0] && zone1Data[1] >= zone2Data[1]) || (zone2Data[0] <= zone1Data[0] && zone2Data[1] >= zone1Data[1]))
                {
                    fullOverLap++;
                }
                if ((zone1Data[0] <= zone2Data[0] && zone1Data[1] >= zone2Data[0]) || (zone2Data[0] <= zone1Data[0] && zone2Data[1] >= zone1Data[0]))
                {
                    PartialOverLap++;
                }
            }
            return fullOverLap.ToString();
        }
        string IAocTask.Solve2()
        {
            return PartialOverLap.ToString();
        }
    }
}





