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
    [AocTask(2022, 9)]
    public class Rope_Bridge : IAocTask
    {

        private string[] Input { get; set; }
        private string[] Instructions { get; set; }
        private Cell[] Knots { get; set; }
        private int Solution2 { get; set; }
        public HashSet<string> VisitedPositions1 { get; set; }
        public HashSet<string> VisitedPositions10 { get; set; }

        record struct Cell(int X, int Y);

        public Rope_Bridge(string filePath)
        {
            Input = File.ReadAllLines(filePath);
            //Input = "R 4\nU 4\nL 3\nD 1\nR 4\nD 1\nL 5\nR 2".Split("\n").ToArray();
            //Input = "R 5\r\nU 8\r\nL 8\r\nD 3\r\nR 17\r\nD 10\r\nL 25\r\nU 20".Split("\r\n").ToArray();
        }
        public void PrepareData()
        {
            VisitedPositions1 = new HashSet<string>();
            VisitedPositions10 = new HashSet<string>();
            Instructions = Input;
            Knots = Enumerable.Repeat(new Cell(0, 0), 10).ToArray();
        }
        string IAocTask.Solve1()
        {
            DoTheWalk(Knots);
            var retValue = VisitedPositions1.Count();
            Debug.Assert(retValue == 6044);
            return retValue.ToString();
        }

        private void DoTheWalk(Cell[] tails)
        {
            
            VisitedPositions1.Add($"{Knots[1].X}:{Knots[1].Y}");
            VisitedPositions10.Add($"{Knots[1].X}:{Knots[1].Y}");
            
            var span = Instructions.AsSpan();
            for(var i=0;i<span.Length;i++)
            {
                var direction = span[i][0];
                var distance = int.Parse(span[i][2..]);
                int dx=0;
                int dy=0;

                switch (direction) 
                {
                    case 'U':
                        dy = 1;
                        break;
                    case 'D':
                        dy = -1;
                        break;
                    case 'L':
                        dx = -1;
                        break;
                    case 'R':
                        dx = 1;
                        break;
                }
                for (var d = 0; d < distance; d++)
                {
                    Knots[0].X = Knots[0].X + dx;
                    Knots[0].Y = Knots[0].Y + dy;
                    for (var j= 1;j < Knots.Length;j++)
                    {
                        var distX = Knots[j-1].X - Knots[j].X;
                        var distY = Knots[j-1].Y - Knots[j].Y;
                        if (Math.Abs(distX) > 1 || Math.Abs(distY) > 1)
                        {
                            Knots[j].X += Math.Sign(distX);
                            Knots[j].Y += Math.Sign(distY);
                            if (j == 1)
                            {
                                VisitedPositions1.Add($"{Knots[j].X}:{Knots[j].Y}");
                            }
                            if (j==9)
                            {
                                VisitedPositions10.Add($"{Knots[j].X}:{Knots[j].Y}");
                            }
                        }
                    }
                }
            }
        }
        private static double GetDistance(Cell start,Cell end)
        {
            var retValue = Math.Pow(end.X - start.X, 2);
            retValue += Math.Pow(end.Y - start.Y, 2);
            return retValue;
        }

        string IAocTask.Solve2()
        {
            var retValue = VisitedPositions10.Count();
            Debug.Assert(retValue == 2384);
            return retValue.ToString();
        }

    }
}

