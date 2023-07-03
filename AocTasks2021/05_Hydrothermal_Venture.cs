using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;
namespace AdventOfCode.AocTasks2021
{
    [AocTask(2021, 5)]
    public class Hydrothermal_Venture : IAocTask
    {
        public string FilePath { get; set; }
        public List<List<Point>> Points1 { get; set; }
        public List<List<Point>> Points2 { get; set; }

        public struct Point
        {
            public int X;
            public int Y;

            public Point(int x, int y)
            {
                X = x;
                Y = y;
            }
        }
        public Hydrothermal_Venture(string filePath)
        {
            FilePath = filePath;
        }
        public void PrepareData()
        {
            Points1 = LoadTaskinput(FilePath, false);
            Points2 = LoadTaskinput(FilePath, true);
        }
        static List<List<Point>> LoadTaskinput(string filePath, bool includeDiagonals)
        {
            List<List<Point>> retValue = new();
            var data = System.IO.File.ReadAllLines(filePath).Where(i => !string.IsNullOrEmpty(i));

            foreach (var item in data)
            {
                var pointData = item.Split(", ->".ToArray(), StringSplitOptions.RemoveEmptyEntries).Select(i => int.Parse(i)).ToArray();
                {
                    var start = new Point(pointData[0], pointData[1]);
                    var end = new Point(pointData[2], pointData[3]);
                    var slopeX = end.X - start.X;
                    var slopeY = end.Y - start.Y;
                    var dirX = Math.Sign(slopeX);
                    var dirY = Math.Sign(slopeY);
                    var numOfPoints = 1 + Math.Max(Math.Abs(slopeX), Math.Abs(slopeY));

                    if (dirX == 0 || dirY == 0 || includeDiagonals)
                    {
                        var pointsInRange = Enumerable.Range(0, numOfPoints).Select(i => new Point(start.X + i * dirX, start.Y + i * dirY)).ToList();
                        retValue.Add(pointsInRange);
                    }
                }

            }
            return retValue;
        }
        static int Solve(List<List<Point>> pointData)
        {
            var retValue = pointData.AsEnumerable().SelectMany(pt => pt).GroupBy(pt => pt).Where(g => g.Count() > 1).Select(g => g.Key).Count();
            return retValue;
        }
        string IAocTask.Solve1()
        {
            var retValue = Solve(Points1);
            return retValue.ToString();

        }
        string IAocTask.Solve2()
        {
            var retValue = Solve(Points2);
            return retValue.ToString();
        }
    }
}