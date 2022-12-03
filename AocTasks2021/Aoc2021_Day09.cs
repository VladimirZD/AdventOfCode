
using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;

namespace AdventOfCode.AocTasks2021
{
    [AocTask(2021, 9)]
    public class Aoc2021_Day09 : IAocTask
    {
        public record  Cell
        {
            public int X;
            public int Y;
            public bool Visited;
            public int Value;
            public int Distance;

            public Cell(int x, int y, int value)
            {
                X = x;
                Y = y;
                Value = value;
            }
        }
        public List<Cell> Cells{ get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public List<Cell> MinPoints { get; set; }
        public Aoc2021_Day09(string filePath)
        {
            Cells= LoadTaskinput(filePath);
            MinPoints = new List<Cell>();
        }
        private List<Cell> LoadTaskinput(string filePath)
        {
            var retValue = new List<Cell>();
            var data = System.IO.File.ReadAllLines(filePath).ToList();

            Height = data.Count;
            Width = data[0].Length;

            for (var y = 0; y < Height; y++)
            {
                var rowData = data[y].ToCharArray();
                for (var x = 0; x < Width; x++)
                {
                    var cell = new Cell(x, y, int.Parse(rowData[x].ToString()));
                    retValue.Add(cell);
                }
            }
            return retValue;
        }
        private bool IsValidPos(int x, int y)
        {
            var retValue = x >= 0 && x < Width && y >= 0 && y < Height;
            return retValue;
        }
        string IAocTask.Solve1()
        {
            foreach (var cell in Cells)
            {
                var neighbours = GetValidNeighbours(Cells, cell, 1);
                if (neighbours.Count(n => n.Value > cell.Value && n.Distance == 1) == neighbours.Count(n => n.Distance == 1))
                {
                    MinPoints.Add(cell);
                }
            }
            var retValue= MinPoints.Sum(c => c.Value + 1);
            return retValue.ToString();
        }
        string IAocTask.Solve2()
        {
            var basins = new List<int>();
            foreach (var cell in MinPoints)
            {
                var neighbours = GetValidNeighbours(Cells, cell, int.MaxValue).OrderBy(n => n.X).ThenBy(n => n.Y);
                basins.Add(neighbours.Where(n => n.Value != 9).Count());
            }
            var retValue= basins.OrderByDescending(b => b).Take(3).ToList().Aggregate(1, (res, n) => res * n).ToString();
            return retValue.ToString();
        }
        private List<Cell> GetValidNeighbours(List<Cell> cells, Cell start, int maxDistance)
        {
            var retValue = new List<Cell>();
            var queue = new Queue<Cell>();
            ResetCellStatus(cells);
            start.Distance = 0;
            queue.Enqueue(start);
            while (queue.Count > 0)
            {
                var currentCell = queue.Dequeue();
                var cellsToAdd = ProcessCell(currentCell, cells).Where(c => !c.Visited && c.Distance < maxDistance).ToList();
                for (var i = 0; i < cellsToAdd.Count; i++)
                {
                    var cell = cellsToAdd[i];
                    {
                        if (cell.Value > currentCell.Value && cell.Value != 9)
                        {
                            queue.Enqueue(cell);
                        }
                    }
                }
            }
            return cells.Where(c => c.Distance > -1).ToList();
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
        static int GetCellIndex(int Width, int x, int y)
        {
            return (Width * y) + x;
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
    }
}





