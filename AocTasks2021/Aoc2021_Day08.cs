
//using AdventOfCode2021Complete.Attributes;
//using AdventOfCode2021Complete.Interfaces;

//namespace AdventOfCode2021Complete.AocTasks2021
//{
//    [AocTask(2021, 8)]
//    public class Day8 : IAocTask
//    {
//        public List<string> SignalData{ get; set; }
//        public List<string> SegmentInfo { get; set; }
//        public Day8(string filePath)
//        {
//            SignalData= LoadTaskinput(filePath);
//            SegmentInfo = new List<string>
//            {
//                "abcefg",
//                "cf",
//                "acdg",
//                "acdfg",
//                "bcdf",
//                "abdfg",
//                "abdefg",
//                "acf",
//                "abcdefg",
//                "abcdfg"
//            };
//        }
//        static List<string> LoadTaskinput(string filePath)
//        {
//            return System.IO.File.ReadAllLines(filePath).ToList();
//        }
//        string IAocTask.Solve1()
//        {
//            var result = 0;
//            foreach (var item in SignalData)
//            {
//                result += item.Split("|")[1].Split(" ").Where(i => i.Length == SegmentInfo[1].Length || i.Length == SegmentInfo[4].Length || i.Length == SegmentInfo[7].Length || i.Length == SegmentInfo[8].Length).Count();
//            }
//            return result.ToString();
//        }
//        string IAocTask.Solve2()
//        {
//            var result = 0;
//            var inputs = new List<string>();

//            var index = 0;
//            while (index < SignalData.Count)
//            {
//                var newMapping = new List<string> { "", "", "", "", "", "", "", "", "", "" };
//                var patterns = SignalData[index].Split("|")[0].Split(" ");
//                /* first find 1,4,7,8*/
//                var item = patterns.Where(i => i.ToString().Length == SegmentInfo[1].Length).Single(); //mapping for 1
//                newMapping[1] = item;

//                item = patterns.Where(i => i.ToString().Length == SegmentInfo[4].Length).Single(); //mapping for 1
//                newMapping[4] = item;

//                item = patterns.Where(i => i.ToString().Length == SegmentInfo[7].Length).Single(); //mapping for 1
//                newMapping[7] = item;

//                item = patterns.Where(i => i.ToString().Length == SegmentInfo[8].Length).Single(); //mapping for 1
//                newMapping[8] = item;

//                /*Get 9 it it has all segments as 4&7 plus one more*/
//                var patterToSearch = (newMapping[4] + newMapping[7]).ToCharArray().OrderBy(c => c).Distinct().ToArray<char>();

//                var candidatesFor9 = patterns.Where(i => i.Length == SegmentInfo[9].Length && new string(i.ToCharArray().OrderBy(c => c).Distinct().ToArray<char>()).Except(patterToSearch).Count() == 1);
//                newMapping[9] = candidatesFor9.Single();

//                /* 0 =8 with 1 segment missing*/
//                patterToSearch = (newMapping[8]).ToCharArray().OrderBy(c => c).Distinct().ToArray<char>();
//                var candidatesFor0 = patterns.Where(i => i.Length == SegmentInfo[0].Length && patterToSearch.Except(i.ToCharArray().OrderBy(c => c).Distinct().ToArray<char>()).Count() == 1);
//                newMapping[0] = candidatesFor9.Single();

//                index++;
//            }
//            return result.ToString();
//        }
//    }
//}





