using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;
using System.Runtime.InteropServices;
using static AdventOfCode.AocTasks2021.Aoc2021_Day09;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AdventOfCode.AocTasks2015
{
    [AocTask(2015, 1)]
    public class Aoc2015_Day01 : IAocTask
    {
        public string FloorData{ get; set; }
        public Aoc2015_Day01(string filePath)
        {
            FloorData = LoadTaskinput(filePath);
        }
        static string LoadTaskinput(string filePath)
        {

            return System.IO.File.ReadAllText(filePath);
        }
        string IAocTask.Solve1()
        {
            var totalLen = FloorData.Length; //+1
            var newLen = FloorData.Where(x => x == ')').Count(); //-1
            var total = (totalLen - newLen) - (newLen);
            return total.ToString();
        }
        string IAocTask.Solve2()
        {
            var instructions = FloorData.AsSpan();
            var floor = 0;
            var i = 0;
            while (floor >=0)
            {
                floor += instructions[i] == '(' ? 1 : -1;
                i++;
            }
            return (i).ToString();
        }
    }
}





