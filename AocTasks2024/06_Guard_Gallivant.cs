using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;
using Microsoft.CodeAnalysis;
using System.Diagnostics;
using System.Threading;

namespace AdventOfCode.AocTasks2024
{
    [AocTask(2024, 6)]
    public class Guard_Gallivant(string filePath) : IAocTask
    {
        public long Sol1 { get; set; }
        public long Sol2 { get; set; }
        private char[][] TaskData { get; set; }
        private int GuardX;
        private int GuardY;
        private List<(int, int)> Directions = new List<(int, int)> { (0, -1), (1, 0), (0, 1), (-1, 0) };
        HashSet<(int, int)> UniqueLocations = new HashSet<(int, int)>();
        HashSet<(int, int)> Obstacles = new HashSet<(int, int)>();

        public void PrepareData()
        {
            var inputLines = File.ReadAllText(filePath);
            //inputLines = "....#.....\r\n.........#\r\n..........\r\n..#.......\r\n.......#..\r\n..........\r\n.#..^.....\r\n........#.\r\n#.........\r\n......#...".Replace("\r\n", "\n");
            TaskData = inputLines.Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(line => line.ToCharArray()).ToArray();
            for (var y = 0; y < TaskData.Length; y++)
            {
                for (var x = 0; x < TaskData[y].Length; x++)
                {
                    if (TaskData[y][x] == '^')
                    {
                        GuardX = x;
                        GuardY = y;
                    }
                }
            }
            Sol1 = 0;
            Sol2 = 0;
        }
        string IAocTask.Solve1()
        {
            var dirIndex = 0;
            UniqueLocations.Add((GuardX, GuardY));
            DoTheWalk(GuardX, GuardY, dirIndex, false, null);
            Sol1 = UniqueLocations.Count();
            Debug.Assert(Sol1 == 41 || Sol1 == 5131);
            return Sol1.ToString();
        }

        private bool DoTheWalk(int startX, int startY, int dirIndex, bool loopCheck, HashSet<(int, int, int)> visited)
        {
            var isLoop = false;
            while (true)
            {
                var direction = Directions[dirIndex];
                var newX = startX + direction.Item1;
                var newY = startY + direction.Item2;
                
                if (!IsValidPos(newX, newY))
                {
                    break;
                }
                if (TaskData[newY][newX] == '#')
                {
                    dirIndex = (dirIndex + 1) % 4;
                }
                else
                {
                    if (!loopCheck)
                    {
                        UniqueLocations.Add((newX, newY));
                        //CheckForLoop(startX, startY, dirIndex);
                    }
                    if (loopCheck)
                    {
                        isLoop = !visited.Add((newX, newY, dirIndex));
                        if (isLoop)
                        {
                            return true;
                        }
                    }
                    startX = newX;
                    startY = newY;
                }
                
            }
            return isLoop;
        }
        private bool CheckForLoop(int startX, int startY, int dirIndex)
        {
            //76.79
            var direction = Directions[dirIndex];
            var newX = startX + direction.Item1;
            var newY = startY + direction.Item2;

            if (!IsValidPos(newX, newY))
            {
                return false;
            }
            var oldValue = TaskData[newY][newX];
            if (newX == GuardX &&  newY == GuardY  )
            {
                return false;
            }

            TaskData[newY][newX] = '#';
            HashSet<(int, int, int)> visitedLocations = new();
            visitedLocations.Add((startX, startY, dirIndex));
            if (DoTheWalk(startX, startY, dirIndex, true, visitedLocations))
            {
                Obstacles.Add((newX, newY));
            }
            TaskData[newY][newX] = oldValue;
            return true;
        }
        private bool IsValidPos(int x, int y)
        {
            var retValue = true;
            if (x>= TaskData[0].Length ||y >= TaskData.Length || x < 0 || y < 0)
            {
                retValue=false;
            }
            return retValue;
        }
        string IAocTask.Solve2()
        {
            //Sol2 = Obstacles.Count();
            HashSet<(int, int)> Obstacles2 = new HashSet<(int, int)>();

            Sol2 = 0;
            foreach (var item in UniqueLocations)
            {
                if (item.Item1 != GuardX || item.Item2 != GuardY)
                {
                    var oldValue = TaskData[item.Item2][item.Item1];
                    TaskData[item.Item2][item.Item1] = '#';
                    if (FindLoop())
                    {
                        Obstacles2.Add((item.Item1, item.Item2));
                        Sol2++;
                    }
                    TaskData[item.Item2][item.Item1] = oldValue;

                }
            }
               
            Debug.Assert(Sol2 == 6 || Sol2 == 1784);
            return Sol2.ToString();
        }
        private bool FindLoop()
        {
            HashSet<(int,int,int)> visited = [];
            int dirIndex = 0;
            var currentDirection = Directions[dirIndex];
            var curX = GuardX;
            var curY = GuardY;

            while (true)
            {
                if (visited.Contains((curX, curY, dirIndex)))
                {
                    return true;
                }
                visited.Add((curX, curY, dirIndex));
                var nextX = curX + currentDirection.Item1;
                var nextY = curY + currentDirection.Item2;
                if (!IsValidPos(nextX, nextY))
                {
                    break;
                }
                if (TaskData[nextY][nextX] == '#')
                {
                    dirIndex = (dirIndex + 1) % 4;
                    currentDirection = Directions[dirIndex];
                    nextX = curX;
                    nextY = curY;
                }
                if (!IsValidPos(nextX,nextY))
                {
                    break;
                }
                curX = nextX;
                curY = nextY;
            }
            return false;
        }
    }
}
