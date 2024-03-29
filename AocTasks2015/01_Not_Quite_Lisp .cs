﻿using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;

namespace AdventOfCode.AocTasks2015
{
    [AocTask(2015, 1)]
    public class Not_Quite_Lisp  : IAocTask
    {
        public string FilePath { get; set; }
        public string FloorData { get; set; }
        public Not_Quite_Lisp(string filePath)
        {
            FilePath = filePath;
        }
        public void PrepareData()
        {
            FloorData = System.IO.File.ReadAllText(FilePath);
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
            while (floor >= 0)
            {
                floor += instructions[i] == '(' ? 1 : -1;
                i++;
            }
            return (i).ToString();
        }
    }
}





