using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;
using System.Diagnostics;
using System.IO;

namespace AdventOfCode.AocTasks2022
{
    [AocTask(2022, 4)]
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
            var input = System.IO.File.ReadAllText(FilePath).Split("\n");
            //var input2 = System.IO.File.ReadAllText(FilePath);
            //var inputAsSpan2 = input2.AsSpan();

            //var retValue3 = new List<string[]>();
            //var cnt= 0;
            //var itemstring = "";
            //var data = new string[2];
            //foreach (var item in inputAsSpan2)
            //{
            //    if (char.IsNumber(item) || item == '-')
            //    {
            //        itemstring += item;
            //        cnt++;
            //        if (cnt % 6==0)
            //        {
            //            data[1] = itemstring;
            //            retValue3.Add(data);
            //            cnt = 0;
            //        }
            //        else if (cnt % 3 == 0)
            //        {
            //            data[0]= itemstring;
            //        }
            //    }
                
            //}
            var inputAsSpan = input.AsSpan();
            var retValue = new List<string[]>();
            foreach (var inputLine in inputAsSpan)
            {
                if (!string.IsNullOrEmpty(inputLine))
                {
                    var lineData = inputLine.Split(',');
                    retValue.Add(lineData);
                }
            }
            ZonesRanges=retValue.ToArray();
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





