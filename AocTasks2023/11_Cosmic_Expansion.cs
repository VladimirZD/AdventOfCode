using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;
using Microsoft.CodeAnalysis;
using System.Diagnostics;
using System.Drawing;
using AdventOfCode;
using System.Collections.Generic;
using Iced.Intel;

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
        public Dictionary<int,int> ColumnExpansion = new Dictionary<int,int>();
        public Dictionary<int, int> RowExpansion = new Dictionary<int, int>();

        //340 ms
        public void PrepareData()
        {
            var textData = File.ReadAllLines(FilePath);
            GalaxyLocations=new List<Point>();
            //textData = "...#......\r\n.......#..\r\n#.........\r\n..........\r\n......#...\r\n.#........\r\n.........#\r\n..........\r\n.......#..\r\n#...#.....".Split("\r\n", StringSplitOptions.RemoveEmptyEntries);

            var width = textData[0].Length;
            var height = textData.Length;
            //ColumnExpansion = Enumerable.Range(0, width-1).ToDictionary(key => key, value => 0);
            RowExpansion = Enumerable.Range(0, height-1).ToDictionary(key => key, value => 0);
            for (int col = 0; col < width; col++)
            {
                for (int row= 0;row<height; row++)
                {
                    if (textData[row][col]=='#')
                    {
                        ColHasGalaxy.Add(col);
                        RowHasGalaxy.Add(row);
                        GalaxyLocations.Add(new Point(col,row));
                    }
                }
            }

            //RowExpansion = Enumerable.Range(0, height-1).ToDictionary(key => key, value => 0);
            ColumnExpansion = CalculeExpansion(ColHasGalaxy, width);
            //for (var col= 0;col < width; col++)
            //{
            //    if (!ColHasGalaxy.Contains(col))
            //    {
            //        for (var j=col+1;j< width; j++)
            //        {
            //            if (!ColumnExpansion.ContainsKey(j))
            //            {
            //                ColumnExpansion.Add(j, 0);
            //            }
            //            ColumnExpansion[j] = ColumnExpansion[j] + 1;
            //        }
            //    }
            //}
            RowExpansion = CalculeExpansion(RowHasGalaxy, height);
            //for (var row = 0; row < height; row++)
            //{
            //    if (!RowHasGalaxy.Contains(row))
            //    {
            //        for (var j = row + 1; j < height; j++)
            //        {
            //            if (!RowExpansion.ContainsKey(j))
            //            {
            //                RowExpansion.Add(j, 0);
            //            }
            //            RowExpansion[j] = RowExpansion[j] + 1;
            //        }
            //    }
            //    }
            }
        private static Dictionary<int,int> CalculeExpansion(HashSet<int> galaxyLocations,int dictSize)
        {
            var dict = Enumerable.Range(0, dictSize- 1).ToDictionary(key => key, value => 0);
            for (var col = 0; col < dictSize; col++)
            {
                if (!galaxyLocations.Contains(col))
                {
                    for (var j = col + 1; j < dictSize; j++)
                    {
                        if (!dict.ContainsKey(j))
                        {
                            dict.Add(j, 0);
                        }
                        dict[j] = dict[j] + 1;
                    }
                }
            }
            return dict;
        }
        string IAocTask.Solve1()
        {
            var totalDistance1 = 0L;
            var totalDistance2 = 0L;
            for (var i = 0; i < GalaxyLocations.Count - 1; i++)
            {
                for (int j = i + 1; j < GalaxyLocations.Count; j++)
                {
                    var origin = GalaxyLocations[i];
                    var destination = GalaxyLocations[j];
                    var distance = CalculateDsitance(origin,destination,1);
                    totalDistance1 += distance;
                    distance = CalculateDsitance(origin, destination, (1000000 - 1));
                    totalDistance2 += distance;
                }
            }
            Sol1 = totalDistance1.ToString();
            Sol2 = totalDistance2.ToString();
            Debug.Assert((Sol1 == "374") || (Sol1 == "10231178"));
            return Sol1;
        }
        private long CalculateDsitance(Point origin,Point desitnation,int expansion)
        {
            var sourceX = origin.X;
            var sourceY = origin.Y;
            var destX = desitnation.X;
            var destY = desitnation.Y;
            var totalExpansion = Math.Abs(RowExpansion[destY] - RowExpansion[sourceY]) + Math.Abs(ColumnExpansion[destX] - ColumnExpansion[sourceX]);
            long distance = Helpers.CalcManhattanDistance(sourceX, sourceY, destX, destY) + (totalExpansion * expansion);
            return distance;
        }
        string IAocTask.Solve2()
        {
            Debug.Assert((Sol2 == "1030") || (Sol2 == "8410") || (Sol2 == "622120986954"));
            return Sol2;
        }
    }
}

