using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;

namespace AdventOfCode.AocTasks2022
{
    [AocTask(2022, 1)]
    public class Calorie_Counting : IAocTask
    {
        public string FilePath { get; set; }
        public List<int> Numbers { get; set; }
        public Calorie_Counting(string filePath)
        {
            FilePath = filePath;
        }
        public void PrepareData()
        {
            Numbers = System.IO.File.ReadAllText(FilePath).Split("\n\n").Select(l => l.Split("\n").Where(i => !string.IsNullOrEmpty(i)).Select(int.Parse).Sum(i => i)).ToList();
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





