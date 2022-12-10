using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.X509;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace AdventOfCode.AocTasks2022
{
    [AocTask(2022, 10)]
    public class Cathod_Ray_Tube : IAocTask
    {
        const int WIDTH = 40;
        const int HEIGHT = 6;

        private string[] Input { get; set; }
        private int Result1 { get; set; }
        private Char[,] Crt { get; set; }

        public Cathod_Ray_Tube(string filePath)
        {
            Input = File.ReadAllLines(filePath).Select(i => i.Split(' ')).SelectMany(i => i).ToArray(); ; ;
            //Input = "addx 15\r\naddx -11\r\naddx 6\r\naddx -3\r\naddx 5\r\naddx -1\r\naddx -8\r\naddx 13\r\naddx 4\r\nnoop\r\naddx -1\r\naddx 5\r\naddx -1\r\naddx 5\r\naddx -1\r\naddx 5\r\naddx -1\r\naddx 5\r\naddx -1\r\naddx -35\r\naddx 1\r\naddx 24\r\naddx -19\r\naddx 1\r\naddx 16\r\naddx -11\r\nnoop\r\nnoop\r\naddx 21\r\naddx -15\r\nnoop\r\nnoop\r\naddx -3\r\naddx 9\r\naddx 1\r\naddx -3\r\naddx 8\r\naddx 1\r\naddx 5\r\nnoop\r\nnoop\r\nnoop\r\nnoop\r\nnoop\r\naddx -36\r\nnoop\r\naddx 1\r\naddx 7\r\nnoop\r\nnoop\r\nnoop\r\naddx 2\r\naddx 6\r\nnoop\r\nnoop\r\nnoop\r\nnoop\r\nnoop\r\naddx 1\r\nnoop\r\nnoop\r\naddx 7\r\naddx 1\r\nnoop\r\naddx -13\r\naddx 13\r\naddx 7\r\nnoop\r\naddx 1\r\naddx -33\r\nnoop\r\nnoop\r\nnoop\r\naddx 2\r\nnoop\r\nnoop\r\nnoop\r\naddx 8\r\nnoop\r\naddx -1\r\naddx 2\r\naddx 1\r\nnoop\r\naddx 17\r\naddx -9\r\naddx 1\r\naddx 1\r\naddx -3\r\naddx 11\r\nnoop\r\nnoop\r\naddx 1\r\nnoop\r\naddx 1\r\nnoop\r\nnoop\r\naddx -13\r\naddx -19\r\naddx 1\r\naddx 3\r\naddx 26\r\naddx -30\r\naddx 12\r\naddx -1\r\naddx 3\r\naddx 1\r\nnoop\r\nnoop\r\nnoop\r\naddx -9\r\naddx 18\r\naddx 1\r\naddx 2\r\nnoop\r\nnoop\r\naddx 9\r\nnoop\r\nnoop\r\nnoop\r\naddx -1\r\naddx 2\r\naddx -37\r\naddx 1\r\naddx 3\r\nnoop\r\naddx 15\r\naddx -21\r\naddx 22\r\naddx -6\r\naddx 1\r\nnoop\r\naddx 2\r\naddx 1\r\nnoop\r\naddx -10\r\nnoop\r\nnoop\r\naddx 20\r\naddx 1\r\naddx 2\r\naddx 2\r\naddx -6\r\naddx -11\r\nnoop\r\nnoop\r\nnoop".Split("\r\n").Select(i => i.Split(' ')).SelectMany(i => i).ToArray(); ;
            //Input= "noop\r\naddx 3\r\naddx -5".Split("\r\n").Select(i=>i.Split(' ')).SelectMany(i=>i).ToArray();
        }
        public void PrepareData()
        {
            Crt = new char[HEIGHT, WIDTH];
            var x = 1;
            double totalValue = 0;

            var instructions = Input.AsSpan();
            for (var i = 0; i < instructions.Length; i++)
            {
                double cycle = i + 1;
                int currentRow = (int)((i / WIDTH) % HEIGHT);
                int currentCol = (int)(i % WIDTH);
                if ((x == currentCol || (x - 1) == currentCol) || (x + 1) == currentCol)
                {
                    Crt[currentRow, currentCol] = '#';
                }
                var cmd = instructions[i];
                if (cycle == 20 || (cycle + 20) % 40 == 0)
                {
                    totalValue += cycle * x;
                }
                if (int.TryParse(cmd, out int value))
                {
                    x += value;
                }
            }
            Result1 = (int)totalValue;
        }
        string IAocTask.Solve1()
        {
            return Result1.ToString();
        }
        string IAocTask.Solve2()
        {
            var retValue = GetPrintData(Crt);
            return retValue.ToString();
        }
        private string GetPrintData(char[,] data)
        {
            var line = Environment.NewLine;
            for (var y = 0; y < HEIGHT; y++)
            {
                for (var x = 0; x < WIDTH; x++)
                {
                    line += data[y, x] == '#' ? '#' : '.';
                }
                line += Environment.NewLine;
            }
            return line;
        }

    }
}

