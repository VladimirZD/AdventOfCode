using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;

namespace AdventOfCode.AocTasks2022
{
    [AocTask(2022, 1)]
    public class Aoc2022_Day01 : IAocTask
    {
        public List<int> Numbers { get; set; }
        public Aoc2022_Day01(string filePath)
        {
            Numbers = LoadTaskinput(filePath);
        }
        static List<int> LoadTaskinput(string filePath)
        {
            var numbers = new List<int>();
            var data = System.IO.File.ReadAllText(filePath).Split("\n\n").Select(l => l.Split("\n").Where(i=>!string.IsNullOrEmpty(i)).Select(int.Parse).Sum(i=>i)).ToList();
            return data;
        }
        string IAocTask.Solve1()
        {
            return Numbers.Max().ToString();
        }
        string IAocTask.Solve2()
        {
            
            return Numbers.OrderByDescending(i => i).Take(3).Sum().ToString();
        }
    }
}





