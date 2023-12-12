using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;
using Microsoft.CodeAnalysis;
using System.Diagnostics;
using static System.Reflection.Metadata.BlobBuilder;
using System.Numerics;

namespace AdventOfCode.AocTasks2023
{
    [AocTask(2023, 10)]

    public class Pipe_Maze(string filePath) : IAocTask
    {
        public class Cell()
        {
            public int X { get; set; }
            public int Y { get; set; }
            public char Value { get; set; }
            public bool HasAnimal { get; set; }
            public int Distance { get; set; }
        }
        public string FilePath { get; set; } = filePath;
        public string Sol1 { get; set; }
        public string Sol2 { get; set; }
        
        private List<Cell> MapData { get; set; }
        private int Width { get; set; }
        private int Height { get; set; }
        private struct CellOfset { public int Dx; public int Dy; }
        private static Dictionary<char, List<CellOfset>> Connections = new Dictionary<char, List<CellOfset>>
        {
            {'S', new List<CellOfset>{ new CellOfset {Dx = 1, Dy = 0 }, new CellOfset { Dx = -1, Dy = 0 }, new CellOfset {Dx=0,Dy=-1 }, new CellOfset { Dx = 0, Dy = 1 }}}, //Right,Left,Up,Down
            {'.', new List<CellOfset>{}}, //None
            {'|', new List<CellOfset>{ new CellOfset {Dx=0,Dy=-1 },new CellOfset { Dx = 0, Dy = 1 }}}, //Up,down
            {'-', new List<CellOfset>{ new CellOfset {Dx=-1,Dy=0}, new CellOfset { Dx = 1, Dy = 0 }}}, //Left,Right
            {'7', new List<CellOfset>{ new CellOfset {Dx=-1,Dy=0}, new CellOfset { Dx = 0,Dy = 1 }}}, //Left,Down
            {'J', new List<CellOfset>{ new CellOfset {Dx=0,Dy=-1 },new CellOfset { Dx = -1,Dy = 0 }}}, //Up,Left
            {'F', new List<CellOfset>{ new CellOfset {Dx=1,Dy=0 }, new CellOfset { Dx = 0, Dy = 1 }}}, //Right,Down
            {'L', new List<CellOfset>{ new CellOfset {Dx=0,Dy=-1 },new CellOfset { Dx = 1, Dy = 0  }}} //Up,right
        };

        public void PrepareData()
        {
            MapData = new List<Cell>();
            var textData = File.ReadAllLines(FilePath);
            //textData = ".....\n.S-7.\n.|.|.\n.L-J.\n.....".Split("\n", StringSplitOptions.RemoveEmptyEntries);
            //textData = "..F7.\r\n.FJ|.\r\nSJ.L7\r\n|F--J\r\nLJ...".Replace("\r","").Split("\n", StringSplitOptions.RemoveEmptyEntries);
            //textData= "7-F7-\r\n.FJ|7\r\nSJLL7\r\n|F--J\r\nLJ.LJ".Replace("\r", "").Split("\n", StringSplitOptions.RemoveEmptyEntries);
            Width = textData[0].Length;
            Height = textData.Length;
            for (var y = 0; y < textData.Length; y++)
            {
                for (var x = 0; x < textData[y].Length; x++)
                {
                    var hasAnimal = textData[y][x] == 'S' ? true : false;
                    var cell = new Cell { X = x, Y = y, Value = textData[y][x], HasAnimal = hasAnimal,Distance=-1 };
                    MapData.Add(cell);
                }
            }
            DoTheWalk();
        }
        private void DoTheWalk()
        {
            var animalCell = MapData.Where(c => c.HasAnimal).Single();
            var queue = new Queue<Cell>();
            animalCell.Distance= 0;
            queue.Enqueue(animalCell);
            while (queue.Count > 0)
            {
                var currentCell = queue.Dequeue();
                var validConnections= Connections[currentCell.Value];
                foreach (var connection in validConnections)
                {
                    var newX = currentCell.X+connection.Dx;
                    var newY = currentCell.Y+connection.Dy;
                    if (IsValidPos(newX,newY))
                    {
                        var nextCell = MapData.Where(c => c.X == newX && c.Y == newY && c.Distance==-1 && c.Value!='.').SingleOrDefault();
                        //Is cell conecting to me
                        var cellDirectionAllowed = nextCell!=null && Connections[nextCell.Value].Where(c => c.Dx == -connection.Dx && c.Dy == -connection.Dy).Any();
                        if (cellDirectionAllowed)
                        {
                            nextCell.Distance = currentCell.Distance + 1;
                            queue.Enqueue(nextCell);
                        }
                    }
                }
            }
        }
        private bool IsValidPos(int x, int y)
        {
            var retValue = x >= 0 && x < Width && y >= 0 && y < Height;
            return retValue;
        }
        string IAocTask.Solve1()
        {
            var row = 0;
            Sol1 = MapData.Max(c => c.Distance).ToString();
            var d = MapData.OrderByDescending(c => c.Distance).Where(c=>c.Distance!=-1).ToList();
            Debug.Assert((Sol1 == "4") || (Sol1 == "8") || (Sol1 == "6599"));
            return Sol1;
        }
        string IAocTask.Solve2()
        {
            Debug.Assert((Sol2 == "2") || (Sol2 == "1066"));
            return Sol2;
        }
    }
}

