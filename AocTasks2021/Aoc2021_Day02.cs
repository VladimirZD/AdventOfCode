using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;

namespace AdventOfCode.AocTasks2021
{
    [AocTask(2021, 2)]
    public class Aoc2021_Day02 : IAocTask
    {
        public string FilePath { get; set; }
        public List<string> Commands { get; set; }
        public Aoc2021_Day02(string filePath)
        {
            FilePath=filePath;
        }
        public void PrepareData()
        {
            Commands = File.ReadAllLines(FilePath).ToList<string>();
        }
        string IAocTask.Solve1()
        {
            var horizontal = 0;
            var vertical = 0;

            foreach (var command in Commands)
            {
                var values = command.Split(' ');
                if (values[0] == "forward")
                {
                    horizontal += int.Parse(values[1]);
                }
                else if (values[0] == "up")
                {
                    vertical -= int.Parse(values[1]);
                }
                else
                {
                    vertical += int.Parse(values[1]);
                }
            }
            return (horizontal * vertical).ToString();
        }
        string IAocTask.Solve2()
        {
            var horizontal = 0;
            var vertical = 0;
            var aim = 0;

            foreach (var command in Commands)
            {
                var values = command.Split(' ');
                if (values[0] == "forward")
                {
                    var val = int.Parse(values[1]);
                    horizontal += val;
                    vertical += (aim * val);
                }
                aim += values[0] == "up" ? -1 * int.Parse(values[1]) : int.Parse(values[1]);
            }
            return (horizontal * vertical).ToString();
        }
    }
}





