using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;
using System.IO;

namespace AdventOfCode.AocTasks2022
{
    [AocTask(2019, 1)]
    public class The_Tyranny_of_the_Rocket_Equation : IAocTask
    {
        public string FilePath { get; set; }
        public List<decimal> Modules { get; set; }
        public The_Tyranny_of_the_Rocket_Equation(string filePath)
        {
            FilePath = filePath;
        }
        public void PrepareData()
        {
            Modules = File.ReadAllText(FilePath).Split("\n").Where(i=>!string.IsNullOrEmpty(i)).Select(i=>decimal.Parse(i.Trim())).ToList();
        }
        string IAocTask.Solve1()
        {
            var totalFuel = 0;
            foreach (var module in Modules) 
            {
                totalFuel += (int)Math.Floor(module / 3) - 2;
            }
            return totalFuel.ToString();
        }
        string IAocTask.Solve2()
        {
            decimal totalFuel = 0;
            foreach (var module in Modules)
            {
                var fuel = (int)Math.Floor(module / 3M) - 2;
                while (fuel>0)
                {
                    totalFuel += fuel;
                    fuel = (int)Math.Floor(fuel/ 3M) - 2;
                    
                }
            }
            return totalFuel.ToString();
        }
    }
}





