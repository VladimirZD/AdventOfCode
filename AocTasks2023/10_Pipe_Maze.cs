using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;
using Microsoft.CodeAnalysis;
using System.Diagnostics;

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
            ////textData = ".....\nLS-7.\n.|.|.\n.L-J.\n.....".Split("\n", StringSplitOptions.RemoveEmptyEntries);
            ////textData = "..F7.\r\n.FJ|.\r\nSJ.L7\r\n|F--J\r\nLJ...".Replace("\r","").Split("\n", StringSplitOptions.RemoveEmptyEntries);
            ////textData= "7-F7-\r\n.FJ|7\r\nSJLL7\r\n|F--J\r\nLJ.LJ".Replace("\r", "").Split("\n", StringSplitOptions.RemoveEmptyEntries);
            //textData = "...........\r\n.S-------7.\r\n.|F-----7|.\r\n.||.....||.\r\n.||.....||.\r\n.|L-7.F-J|.\r\n.|..|.|..|.\r\n.L--J.L--J.\r\n...........".Replace("\r", "").Split("\n", StringSplitOptions.RemoveEmptyEntries);
            //textData= ".F----7F7F7F7F-7....\r\n.|F--7||||||||FJ....\r\n.||.FJ||||||||L7....\r\nFJL7L7LJLJ||LJ.L-7..\r\nL--J.L7...LJS7F-7L7.\r\n....F-J..F7FJ|L7L7L7\r\n....L7.F7||L7|.L7L7|\r\n.....|FJLJ|FJ|F7|.LJ\r\n....FJL-7.||.||||...\r\n....L---J.LJ.LJLJ...".Replace("\r", "").Split("\n", StringSplitOptions.RemoveEmptyEntries);
            //textData = "FF7FSF7F7F7F7F7F---7\r\nL|LJ||||||||||||F--J\r\nFL-7LJLJ||||||LJL-77\r\nF--JF--7||LJLJ7F7FJ-\r\nL---JF-JLJ.||-FJLJJ7\r\n|F|F-JF---7F7-L7L|7|\r\n|FFJF7L7F-JF7|JL---7\r\n7-L-JL7||F7|L7F-7F7|\r\nL.L7LFJ|||||FJL7||LJ\r\nL7JLJL-JLJLJL--JLJ.L".Replace("\r", "").Split("\n", StringSplitOptions.RemoveEmptyEntries);
            Width = textData[0].Length;
            Height = textData.Length;
            for (var y = 0; y < textData.Length; y++)
            {
                for (var x = 0; x < textData[y].Length; x++)
                {
                    var cell = new Cell { X = x, Y = y, Value = textData[y][x], Distance = -1 };
                    MapData.Add(cell);
                }
            }
        }
        private void FollowThePipes()
        {
            var animalCell = MapData.First(c => c.Value == 'S');
            var queue = new Queue<Cell>();
            animalCell.Distance = 0;
            queue.Enqueue(animalCell);
            while (queue.Count > 0)
            {
                var currentCell = queue.Dequeue();
                var validConnections = Connections[currentCell.Value];
                for (int i = 0; i < validConnections.Count; i++)
                {
                    CellOfset connection = validConnections[i];
                    var newX = currentCell.X + connection.Dx;
                    var newY = currentCell.Y + connection.Dy;
                    if (IsValidPos(newX, newY))
                    {
                        var nextCell = MapData[newX + newY * Width];
                        if (nextCell != null && nextCell.Distance<1 && nextCell.Value!='.')
                        {
                            //Is cell conecting to me
                            var cellDirectionAllowed = nextCell != null && Connections[nextCell.Value].Where(c => c.Dx == -connection.Dx && c.Dy == -connection.Dy).Any();
                            if (cellDirectionAllowed && nextCell.Value != 'S')
                            {
                                nextCell.Distance = currentCell.Distance + 1;
                                queue.Enqueue(nextCell);
                            }
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
            FollowThePipes();
            var row = 0;
            Sol1 = MapData.Max(c => c.Distance).ToString();
            Debug.Assert((Sol1 == "4") || (Sol1 == "8") || (Sol1 == "6599") || (Sol1 == "23"));
            return Sol1;
        }
        string IAocTask.Solve2()
        {
            var cnt = CountTilesInTheLoop();
            Sol2 = cnt.ToString();
            Debug.Assert((Sol2 == "4") || (Sol2 == "477"));
            return Sol2;
        }

        private int CountTilesInTheLoop()
        {
            int cnt = 0;
            for (int i = 0; i < MapData.Count; i++)
            {
                Cell cell = MapData[i];
                var currentCellIndex = i;
                if (cell.Distance != -1)
                {
                    continue;
                }
                var clear_right = true;
                var clear_left = true;
                while (currentCellIndex>= 0)
                {
                    var nextCell = MapData[currentCellIndex];
                    
                    if (nextCell.Distance != -1 && nextCell.Value != 'S' && (nextCell.Value=='L' || nextCell.Value == 'F' || nextCell.Value == '-'))
                    {
                        clear_right = !clear_right;
                    }
                    if (nextCell.Distance != -1 && nextCell.Value != 'S' && (nextCell.Value == '7' || nextCell.Value == 'J' || nextCell.Value == '-'))
                    {
                        clear_left = !clear_left;
                    }
                    currentCellIndex = currentCellIndex - Width;
                }
                if (!(clear_right || clear_left))
                {
                    cnt++;
                }
            }
            return cnt;
        }
    }
}

