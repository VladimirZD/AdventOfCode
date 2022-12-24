using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;
using System.Diagnostics;

namespace AdventOfCode.AocTasks2022
{
    [AocTask(2022, 14)]
    public class Regolith_Reservoir : IAocTask
    {
        record struct ParseResult(string value, int position);
        private string[] Input { get; set; }
        private int Result1 { get; set; }
        private int Result2 { get; set; }
        private char[,] Cells { get; set; }
        
        const char SAND = 'o';
        const char ROCK = '#';
        const int  SAND_START = 500;
        const char FREE = '\0';


        public Regolith_Reservoir(string filePath)
        {
            Input = File.ReadAllText(filePath).Split("\n");
            Input = "498,4 -> 498,6 -> 496,6\r\n503,4 -> 502,4 -> 502,9 -> 494,9".Replace("\r\n", "\n").Split("\n\n").ToArray();
        }
        public void PrepareData()
        {
            //var mapData = Input
        }
        string IAocTask.Solve1()
        {
            //Debug.Assert(Result1 == 5252);
            return Result1.ToString();
        }
        string IAocTask.Solve2()
        {
          //  Debug.Assert(Result1 == 5252);
            return Result2.ToString();
        }
    }
}



