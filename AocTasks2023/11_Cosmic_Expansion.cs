using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;
using Microsoft.CodeAnalysis;
using System.Diagnostics;
using System.Drawing;
using AdventOfCode;
using System.Collections.Generic;

namespace AdventOfCode.AocTasks2023
{
    [AocTask(2023, 11)]

    public class Cosmic_Expansion(string filePath) : IAocTask
    {
        public string FilePath { get; set; } = filePath;
        public string Sol1 { get; set; }
        public string Sol2 { get; set; }
        public List<string> GalaxyMap { get; set; }
        public List<Point> GalaxyLocations { get; set; }
        public HashSet<int> ColHasGalaxy = new HashSet<int>();
        public HashSet<int> RowHasGalaxy = new HashSet<int>();

        public void PrepareData()
        {
            var textData = File.ReadAllLines(FilePath);
            GalaxyLocations=new List<Point>();
            //textData = "...#......\r\n.......#..\r\n#.........\r\n..........\r\n......#...\r\n.#........\r\n.........#\r\n..........\r\n.......#..\r\n#...#.....".Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
            var GalaxyMap = new List<string>();
            for (int col= 0; col < textData[0].Length; col++)
            {
                for (int row= 0;row<textData.Length; row++)
                {
                    if (textData[row][col]=='#')
                    {
                        ColHasGalaxy.Add(col);
                        RowHasGalaxy.Add(row);
                        GalaxyLocations.Add(new Point(col,row));
                    }
                }
            }
        }
        string IAocTask.Solve1()
        {
            var distance = CalculatTotalDistance(1);
            Sol1 = distance.ToString();
            Debug.Assert((Sol1 == "374") || (Sol1 == "10231178"));
            return Sol1;
        }
        private long CalculatTotalDistance(int expansion)
        {
            var galaxyLocations = GalaxyLocations.OrderBy(l => l.Y).ToList();
            var distances = new List<long>();
            var totalDistance = 0L;
            for (var i = 0; i < galaxyLocations.Count - 1; i++)
            {
                for (int j = i + 1; j < galaxyLocations.Count; j++)
                {
                    var sourceX = galaxyLocations[i].X;
                    sourceX += ((sourceX - ColHasGalaxy.Count(i => i < sourceX)) * expansion);

                    var sourceY = galaxyLocations[i].Y;
                    sourceY += ((sourceY - RowHasGalaxy.Count(i => i < sourceY)) * expansion);

                    var destX = galaxyLocations[j].X;
                    destX += ((destX - ColHasGalaxy.Count(i => i < destX)) * expansion);

                    var destY = galaxyLocations[j].Y;
                    destY += ((destY - RowHasGalaxy.Count(i => i < destY)) * expansion);

                    long distance = Helpers.CalcManhattanDistance(sourceX, sourceY, destX, destY);
                    totalDistance+= distance;
                    distances.Add((long)distance);
                }
            }
            return totalDistance;
        }
        string IAocTask.Solve2()
        {
            var distance = CalculatTotalDistance((1000000-1));
            Sol2 = distance.ToString();
            Debug.Assert((Sol2 == "1030") || (Sol2 == "8410") || (Sol2 == "622120986954"));
            return Sol2;
        }
    }
}

