using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;
using Microsoft.CodeAnalysis;
using System.Diagnostics;

namespace AdventOfCode.AocTasks2023
{
    [AocTask(2023, 11)]

    public class Cosmic_Expansion(string filePath) : IAocTask
    {
        public string FilePath { get; set; } = filePath;
        public string Sol1 { get; set; }
        public string Sol2 { get; set; }

        public void PrepareData()
        {
            var textData = File.ReadAllLines(FilePath);
            textData = "...#......\r\n.......#..\r\n#.........\r\n..........\r\n......#...\r\n.#........\r\n.........#\r\n..........\r\n.......#..\r\n#...#.....".Split("\n", StringSplitOptions.RemoveEmptyEntries);
        }
        string IAocTask.Solve1()
        {
            Sol1 = "N/A";
            //Debug.Assert((Sol1 == "4") || (Sol1 == "8") || (Sol1 == "6599") || (Sol1 == "23"));
            return Sol1;
        }
        string IAocTask.Solve2()
        {
            Sol2 = "N/A";
            //Debug.Assert((Sol2 == "4") || (Sol2 == "477"));
            return Sol2;
        }
    }
}

