using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;

namespace AdventOfCode.AocTasks2021
{
    public record Point
    {
        public int X;
        public int Y;
        public string Value;
        public Point(int x, int y, string value)
        {
            X = x;
            Y = y;
            Value = value;
        }
    }
    [AocTask(2021, 13)]
    public class Transparent_Origami : IAocTask
    {
        public string FilePath { get; set; }
        public List<Point> Points { get; set; }
        public List<string> Instructions { get; set; }
        public Transparent_Origami(string filePath)
        {
            FilePath = filePath;
        }
        public void PrepareData()
        {
            var data = File.ReadLines(FilePath).ToList();
            var splitLineIndex = data.IndexOf(data.Where(l => l == "").Single());
            Instructions = data.Skip(splitLineIndex + 1).Take(data.Count - splitLineIndex).Select(i => i.Replace("fold along ", "")).ToList();
            Points = data.Take(splitLineIndex).SelectMany(i => i.Split("\r\n")).Select(p => new Point(int.Parse(p.Split(',')[0]), int.Parse(p.Split(',')[1]), "#")).OrderBy(p => p.Y).ThenBy(p => p.X).ToList();
        }
        string IAocTask.Solve1()
        {
            var retValue = 0;
            for (var i = 0; i < Instructions?.Count; i++)
            {
                var stepData = Instructions[i].Split('=');
                var foldX = stepData[0] == "x" ? int.Parse(stepData[1]) : 0;
                var foldY = stepData[0] == "y" ? int.Parse(stepData[1]) : 0;
                DoFold(foldX, foldY, Points);
                if (i == 0)
                {
                    retValue = Points.Count(p => p.Value == "#");
                }
            }
            return retValue.ToString();
        }
        string IAocTask.Solve2()
        {
            var printData = GetPrintData();
            return Environment.NewLine + string.Join(Environment.NewLine, printData).ToString() + Environment.NewLine;
        }
        private List<string> GetPrintData()
        {
            var retValue = new List<string>();
            var width = Points.Max(p => p.X);
            var height = Points.Max(p => p.Y);
            for (var y = 0; y <= height; y++)
            {
                var line = "";
                for (var x = 0; x <= width; x++)
                {
                    var point = Points.Where(p => p.X == x && p.Y == y).FirstOrDefault();
                    line += (point == null || point.Value == "") ? "." : "#";
                }
                retValue.Add(line);
            }
            return retValue;
        }

        static void DoFold(int foldX, int foldY, List<Point> points)
        {
            List<Point> pointsToFold;
            if (foldY != 0)
            {
                pointsToFold = points.Where(p => p.Y > foldY).ToList();
            }
            else
            {
                pointsToFold = points.Where(p => p.X > foldX).ToList();
            }
            foreach (var point in pointsToFold)
            {
                var newX = Math.Abs(foldX * 2 - point.X);
                var newY = Math.Abs(foldY * 2 - point.Y);
                var newPos = points.Where(p => p.X == newX && p.Y == newY).FirstOrDefault();
                {
                    if (newPos == null)
                    {
                        point.X = newX;
                        point.Y = newY;
                    }
                    else
                    {
                        point.Value = "";
                    }
                }
            }
            points.RemoveAll(p => p.Value != "#");
        }

    }
}





