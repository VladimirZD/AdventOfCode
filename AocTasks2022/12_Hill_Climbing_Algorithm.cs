using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;
using System.Diagnostics;
using System.Drawing;
using static AdventOfCode.AocTasks2021.Aoc2021_Day09;

namespace AdventOfCode.AocTasks2022
{
    [AocTask(2022, 12)]


    public class Hill_Climbing_Algorithm : IAocTask
    {
        public record Cell
        {
            public int X;
            public int Y;
            public bool Visited;
            public int Height;
            public int Distance;
            public String Value;

            public Cell(int x, int y, int height, string value)
            {
                X = x;
                Y = y;
                Height = height;
                Distance = -1;
                Value = value;
                
            }
        }

        const char START = 'S';
        const char END = 'E';
        private string[] Input { get; set; }
        private long Result1 { get; set; }
        private long Result2 { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        private List<Cell> Cells { get; set; }
        private Cell StartCell { get; set; }
        private Cell EndCell { get; set; }

        public Hill_Climbing_Algorithm(string filePath)
        {
            Input = File.ReadAllLines(filePath); 
            //Input = "Sabqponm\r\nabcryxxl\r\naccszExk\r\nacctuvwj\r\nabdefghi".Split("\r\n").ToArray();
        }
        public void PrepareData()
        {
            Cells = new List<Cell>();

            //var data = Input.ToList();
            var data = Input;
            Height = data.Length;
            Width = data[0].Length;

            for (var y = 0; y < Height; y++)
            {
                var rowData = data[y].ToCharArray();
                for (var x = 0; x < Width; x++)
                {
                    var cellElevation = rowData[x];
                    var cell = new Cell(x, y, cellElevation,cellElevation.ToString());
                    
                    if (cell.Height==START)
                    {
                        cell.Height = 'a';
                        StartCell = cell;
                    }
                    if (cell.Height == END)
                    {
                        cell.Height = 'z';
                        EndCell= cell;
                    }
                    Cells.Add(cell);
                }
            }
        }
        string IAocTask.Solve1()
        {
            WalkToTheEnd(Cells, EndCell,StartCell);
            Result1 = StartCell.Distance;
            return Result1.ToString();
        }
        string IAocTask.Solve2()
        {

            var data = Cells.Where(c => c.Height == StartCell.Height && c.Visited).ToList().OrderBy(c => c.Distance).ToList().First();
            Result2 = data.Distance;
            return Result2.ToString();
        }

        static void ResetCellStatus(List<Cell> cells)
        {
            for (var i = 0; i < cells.Count; i++)
            {
                var cell = cells[i]; ;
                cell.Distance = -1;
                cell.Visited = false;
            }
        }

        private void WalkToTheEnd(List<Cell> cells, Cell start, Cell end)
        {
            var retValue = new List<Cell>();
            var queue = new Queue<Cell>();
            HashSet<Cell> queeued = new HashSet<Cell>();
            //ResetCellStatus(cells);
            start.Distance = 0;
            queue.Enqueue(start);
            while (queue.Count > 0)
            {
                /*
                 * ou'd like to reach E, but to save energy, you should do it in as few steps as possible. During each step, you can move exactly one square up, down, left, or right. 
                 * To avoid needing to get out your climbing gear, the elevation of the destination square can be at most one higher than the elevation of your current square; that is, if your current elevation is m, you could step to elevation n, but not to elevation o. 
                 * (This also means that the elevation of the destination square can be much lower than the elevation of your current square.)
                 * */
                var currentCell = queue.Dequeue();
                var cellsToAdd = ProcessCell(currentCell, cells).Where(c => !c.Visited && ((currentCell.Height-c.Height) <= 1) && !queeued.Contains(c)).ToList();
                for (var i = 0; i < cellsToAdd.Count; i++)
                {
                    var cell = cellsToAdd[i];
                    queue.Enqueue(cell);
                    queeued.Add(cell);
                }
            }
            var retValue2 = cells.Where(c => c.Distance > -1).ToList();
        }

        private bool IsValidPos(int x, int y)
        {
            var retValue = x >= 0 && x < Width && y >= 0 && y < Height;
            return retValue;
        }

        private List<Cell> ProcessCell(Cell startCell, List<Cell> cells)
        {
            var retValue = new List<Cell>();
            int[] dx = { 0, 0, -1, 1 };
            int[] dy = { 1, -1, 0, 0 };
            startCell.Visited = true;
            for (var i = 0; i < 4; i++)
            {
                var newX = startCell.X + dx[i];
                var newY = startCell.Y + dy[i];
                if (IsValidPos(newX, newY))
                {
                    var newCell = cells[GetCellIndex(Width, newX, newY)]; 
                    if (!newCell.Visited)
                    {
                        newCell.Distance = startCell.Distance + 1;
                        retValue.Add(newCell);
                    }
                }
            }
            return retValue;
        }
    private static int GetCellIndex(int Width, int x, int y)
        {
            return (Width * y) + x;
        }
    }
}


