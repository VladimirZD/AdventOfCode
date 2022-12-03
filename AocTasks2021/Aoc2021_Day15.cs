///*

//using AdventOfCode2021;
//using System.Diagnostics;

//string filePath = $"{AppContext.BaseDirectory}\\Input.txt";
//var mapGrowth = 5;

//var data = File.ReadAllLines(filePath).ToList();
//var stopWatch = new Stopwatch();

//stopWatch.Start();
//var cells = LoadTaskInput(data, out int width, out int height);
//Solve(cells, width, height);

//var extendedCells = ExtendMap(cells, mapGrowth, width, height);
//width *= mapGrowth;
//height *= mapGrowth;

//Solve(extendedCells, width, height);
//stopWatch.Stop();
//Console.WriteLine($"Elapsed {stopWatch.ElapsedMilliseconds} ms");

//static Dictionary<Cell, int> ExtendMap(Dictionary<Cell, int> cells, int mapGrowth, int width, int height)
//{

//    var retValue = new Dictionary<Cell, int>();

//    var newWidth = width * mapGrowth;
//    var newHeight = height * mapGrowth;

//    for (var y = 0; y < newHeight; y++)
//    {
//        for (var x = 0; x < newWidth; x++)
//        {
//            var originalX = x % width;
//            var originalY = y % height;
//            var value = cells[new Cell(originalX, originalY)] + x / width + y / height;
//            while (value > 9)
//            {
//                value -= 9;
//            }
//            retValue.Add(new Cell(x, y), value);
//        }
//    }
//    return retValue;
//}

//static void Solve(Dictionary<Cell, int> cells, int width, int height)
//{
//    var start = new Cell(0, 0);
//    var end = new Cell(width - 1, height - 1);
//    var priorityQueue = new PriorityQueue<Cell, int>();
//    var processedCells = new Dictionary<Cell, int>();
//    processedCells.Add(start, 0);
//    priorityQueue.Enqueue(start, 0);

//    var stop = false;
//    while (!stop)
//    {
//        var currentCell = priorityQueue.Dequeue();
//        stop = currentCell == end;
//        if (!stop)
//        {
//            foreach (var cell in GetCellsInRadius(currentCell, width, height))
//            {
//                var totalRisk = processedCells[currentCell] + cells[cell];
//                if (totalRisk < processedCells.GetValueOrDefault(cell, int.MaxValue))
//                {
//                    processedCells[cell] = totalRisk;
//                    priorityQueue.Enqueue(cell, totalRisk);
//                }

//            }
//        }
//    }
//    var result = processedCells[end];
//    Console.WriteLine($"Solution = {result}");
//}

//static List<Cell> GetCellsInRadius(Cell start, int width, int height)
//{
//    var retValue = new List<Cell>();
//    int[] dx = { 0, 0, -1, 1, };
//    int[] dy = { 1, -1, 0, 0, };
//    for (var i = 0; i < dx.Length; i++)
//    {
//        var newX = start.X + dx[i];
//        var newY = start.Y + dy[i];
//        if (IsValidPos(newX, newY, width, height))
//        {
//            retValue.Add(new Cell(newX, newY));
//        }
//    }

//    return retValue;
//}

//static bool IsValidPos(int x, int y, int width, int height)
//{
//    var retValue = x >= 0 && x < width && y >= 0 && y < height;
//    return retValue;
//}

//static Dictionary<Cell, int> LoadTaskInput(List<string> data, out int Width, out int Height)
//{
//    var retValue = new Dictionary<Cell, int>();

//    Height = data.Count;
//    Width = data[0].Length;

//    for (var y = 0; y < Height; y++)
//    {
//        var rowData = data[y].ToCharArray();
//        for (var x = 0; x < Width; x++)
//        {
//            var cell = new Cell(x, y);
//            retValue.Add(cell, int.Parse(rowData[x].ToString()));
//        }
//    }
//    return retValue;
//}



// * */
//using AdventOfCode.Attributes;
//using AdventOfCode.Interfaces;

//namespace AdventOfCode.AocTasks2021
//{
//    public record Cell
//    {
//        public int X;
//        public int Y;

//        public Cell(int x, int y)
//        {
//            X = x;
//            Y = y;

//        }
//    }

//    [AocTask(2021, 15)]
//    public class Day15 : IAocTask
//    {
//        public Day15(string filePath)
//        {
//            LoadTaskInput(filePath);
//        }
//        string IAocTask.Solve1()
//        {
//            var retValue= 0L;
//            var step = 0;
//            var maxStep = 40;

//            ElementPairs = new();
//            var elements = StartTemplate.ToCharArray();
//            for (var i = 0; i < elements.Length - 1; i++)
//            {
//                var key = elements[i].ToString() + elements[i + 1].ToString();
//                ElementPairs[key] = ElementPairs.GetValueOrDefault(key) + 1;
//            }

//            while (step < maxStep)
//            {
//                var newElementPairs = new Dictionary<string, long>();

//                foreach (var pair in ElementPairs)
//                {
//                    //if (Transformations.ContainsKey(pair.Key))
//                    if (Transformations.TryGetValue(pair.Key, out string value))
//                    {
//                        var newElement = value;
//                        var newPair1 = pair.Key[0] + newElement;
//                        var newPair2 = newElement + pair.Key[1];
//                        newElementPairs[newPair1] = newElementPairs.GetValueOrDefault(newPair1) + pair.Value;
//                        newElementPairs[newPair2] = newElementPairs.GetValueOrDefault(newPair2) + pair.Value;
//                    }
//                    else
//                    {
//                        throw new Exception("OOPS");
//                    }
//                }
//                ElementPairs = newElementPairs;
//                step++;
//                if (step == 10)
//                {
//                    retValue = GetMaxMinusMin(ElementPairs, StartTemplate);
//                }
//            }
//            return retValue.ToString();
//        }
//        string IAocTask.Solve2()
//        {
//            var retValue= GetMaxMinusMin(ElementPairs, StartTemplate);
//            return retValue.ToString();
//        }
//        static long GetMaxMinusMin(Dictionary<string, long> elementPairs, string startTemplate)
//        {
//            var lettersCnt = new Dictionary<char, long>();
//            foreach (var pair in elementPairs)
//            {
//                var letter = pair.Key[0];
//                lettersCnt[letter] = lettersCnt.GetValueOrDefault(letter) + pair.Value;
//                letter = pair.Key[1];
//                lettersCnt[letter] = lettersCnt.GetValueOrDefault(letter) + pair.Value;
//            }
//            lettersCnt[startTemplate[0]]++;
//            lettersCnt[startTemplate[^1]]++;
//            var sortedLetters = lettersCnt.Select(l => l.Value / 2).OrderByDescending(l => l).ToList();
//            return sortedLetters.First() - sortedLetters.Last();
//        }
//        private Dictionary<Cell, int> LoadTaskInput(List<string> data, out int Width, out int Height)
//        {
//            var retValue = new Dictionary<Cell, int>();

//            Height = data.Count;
//            Width = data[0].Length;

//            for (var y = 0; y < Height; y++)
//            {
//                var rowData = data[y].ToCharArray();
//                for (var x = 0; x < Width; x++)
//                {
//                    var cell = new Cell(x, y);
//                    retValue.Add(cell, int.Parse(rowData[x].ToString()));
//                }
//            }
//            return retValue;
//        }

//    }
//}





