using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;

namespace AdventOfCode.AocTasks2021
{
    [AocTask(2021, 1)]
    public class Aoc2021_Day01 : IAocTask
    {
        public List<int> Numbers { get; set; }
        public Aoc2021_Day01(string filePath)
        {
            Numbers = LoadTaskinput(filePath);
        }

        static List<int> LoadTaskinput(string filePath)
        {
            var numbers = new List<int>();
            string[] lines = System.IO.File.ReadAllLines(filePath);
            foreach (var line in lines)
            {
                numbers.Add(int.Parse(line));
            }
            return numbers;
        }
        string IAocTask.Solve1()
        {
            var increses = 0;
            var prevValue = 199;
            for (int i = 1; i < Numbers.Count; i++)
            {
                var number = Numbers[i];
                if (number > prevValue)
                {
                    increses++;

                }
                prevValue = Numbers[i];
            }
            return increses.ToString();
        }
        string IAocTask.Solve2()
        {
            var increses = 0;
            int sum1;
            int sum2;
            for (int i = 1; i < Numbers.Count - 2; i++)
            {
                sum1 = Numbers[i - 1] + Numbers[i] + Numbers[i + 1];
                sum2 = Numbers[i] + Numbers[i + 1] + Numbers[i + 2];
                if (sum2 > sum1)
                {
                    increses++;
                }
            }
            return increses.ToString();
        }
    }
}





