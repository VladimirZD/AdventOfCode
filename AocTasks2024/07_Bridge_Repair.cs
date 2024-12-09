using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;
using Microsoft.CodeAnalysis;
using System.Diagnostics;
using System.Threading;

namespace AdventOfCode.AocTasks2024
{
    [AocTask(2024, 7)]
    public class Bridge_Repair(string filePath) : IAocTask
    {
        public long Sol1 { get; set; }
        public long Sol2 { get; set; }
        private char[][] TaskData { get; set; }
        

        public void PrepareData()
        {
            var inputLines = File.ReadAllText(filePath);
            //inputLines = "....#.....\r\n.........#\r\n..........\r\n..#.......\r\n.......#..\r\n..........\r\n.#..^.....\r\n........#.\r\n#.........\r\n......#...".Replace("\r\n", "\n");
            TaskData = inputLines.Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(line => line.ToCharArray()).ToArray();
            Sol1 = 0;
            Sol2 = 0;
        }
        string IAocTask.Solve1()
        {
            Debug.Assert(Sol1 == 41 || Sol1 == 5131);
            return Sol1.ToString();
        }

        string IAocTask.Solve2()
        {
               
            Debug.Assert(Sol2 == 6 || Sol2 == 1784);
            return Sol2.ToString();
        }
    }
}
