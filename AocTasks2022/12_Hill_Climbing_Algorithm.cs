using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;
using System.Diagnostics;
using System.Drawing;

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

            public Cell(int x, int y, int height)
            {
                X = x;
                Y = y;
                Height = height;
                Distance = -1;
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
            var data = Input;
            Height = data.Length;
            Width = data[0].Length;

            for (var y = 0; y < Height; y++)
            {
                var rowData = data[y].ToCharArray();
                for (var x = 0; x < Width; x++)
                {
                    var cellElevation = rowData[x];
                    var cell = new Cell(x, y, cellElevation);
                    if (cell.Height == START)
                    {
                        cell.Height = 'a';
                        StartCell = cell;
                    }
                    if (cell.Height == END)
                    {
                        cell.Height = 'z';
                        EndCell = cell;
                    }
                    Cells.Add(cell);
                }
            }
        }
        string IAocTask.Solve1()
        {
            WalkToTheStart(Cells, EndCell);
            Result1 = StartCell.Distance;
            Debug.Assert(Result1 == 484);
            return Result1.ToString();
        }
        string IAocTask.Solve2()
        {
            Result2 = Cells.Where(c => c.Height == StartCell.Height && c.Visited).Min(c => c.Distance);
            Debug.Assert(Result2 == 478);
            return Result2.ToString();
        }
        private void WalkToTheStart(List<Cell> cells, Cell start)
        {
            var retValue = new List<Cell>();
            var queue = new Queue<Cell>();
            HashSet<Cell> queeued = new();
            start.Distance = 0;
            queue.Enqueue(start);
            while (queue.Count > 0)
            {
                var currentCell = queue.Dequeue();
                var cellsToAdd = ProcessCell(currentCell, cells).Where(c => !c.Visited && ((currentCell.Height - c.Height) <= 1) && !queeued.Contains(c)).ToList();
                for (var i = 0; i < cellsToAdd.Count; i++)
                {
                    var cell = cellsToAdd[i];
                    queue.Enqueue(cell);
                    queeued.Add(cell);
                }
            }
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
                    if (!newCell.Visited && ((StartCell.Height - newCell.Height) <= 1))
                    {
                        retValue.Add(newCell);
                        newCell.Distance = startCell.Distance + 1;
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


