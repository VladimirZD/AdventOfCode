using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;
using System.Linq.Expressions;

namespace AdventOfCode.AocTasks2022
{
    [AocTask(2016, 2)]
    public class Bathroom_Security : IAocTask
    {
        private int Sol1;
        private int Sol2;
        public string FilePath { get; set; }
        public string[] Input { get; set; }
        public string Message1 { get; set; }
        public string Message2 { get; set; }
        public Bathroom_Security(string filePath)
        {
            FilePath = filePath;
            Input = File.ReadAllText(filePath).Split(", ");
            Input = "ULLRRDDDLURDLUUUUD".Split("");
        }
        public void PrepareData()
        {
            
        }
        string IAocTask.Solve1()
        {
            DoTheWalk();
            return Sol1.ToString();
        }
        private static int GetDistance(int x, int y)
        {
            return Math.Abs(0 - x) + Math.Abs(0 - y);
        }
        string IAocTask.Solve2()
        {
            return Sol2.ToString();
        }
        private void DoTheWalk()
        {
            var (currentX, currentY) = (0,0);
            var currentDirection = 0;
            var visitedLocations = new HashSet<(int, int)>();
            visitedLocations.Add((0, 0));
            foreach (var move in Input) 
            {
                var distance = int.Parse(move[1..].ToString());
                var turn = move[0].ToString();
                currentDirection+=(turn == "R" ? 1 : -1);
                currentDirection = (currentDirection < 0 ? 3 : currentDirection)%4;
                for (var  i = 1; i <= distance;i++)
                {
                    //currentX += dx[currentDirection];
                    //currentY += dy[currentDirection];
                    if (visitedLocations.Contains((currentX, currentY)) && Sol2 == 0)
                    {
                        Sol2 = GetDistance(currentX, currentY);
                    }
                    else
                    {
                        visitedLocations.Add((currentX, currentY));
                    }
                }
            }
            Sol1=GetDistance(currentX, currentY);
        }
    }
}
