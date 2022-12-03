using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;

namespace AdventOfCode.AocTasks2021
{
    public record Octopus
    {
        public int X;
        public int Y;
        public bool Flashed;
        public int EnergyLevel;
        public int Distance;
        public bool Visited;
        public Octopus(int x, int y, int energyLevel)
        {
            X = x;
            Y = y;
            EnergyLevel = energyLevel;
        }
    }

    [AocTask(2021, 11)]
    public class Aoc2021_Day11 : IAocTask
    {
        public List<Octopus> Octopuses{ get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Result2 { get; set; }
        public Aoc2021_Day11(string filePath)
        {
            Octopuses = LoadTaskInput(filePath, out int width, out int height);
            Width = width;
            Height= height;
        }
        static void ResetFlashStatus(List<Octopus> octopuses)
        {
            for (var i = 0; i < octopuses.Count; i++)
            {
                var octopus = octopuses[i];
                octopus.Flashed = false;
            }
        }
        string IAocTask.Solve1()
        {
            var flashes = 0;
            var retValue = 0;
            var currentRound = 1;
            var stop = false;
            while (!stop)
            {
                ResetFlashStatus(Octopuses);
                IncreaseEnergyLevels(Octopuses);
                var flashing = Octopuses.Where(o => o.EnergyLevel > 9).ToList();
                var flashedThisRound = 0;

                while (flashing.Count > 0)
                {
                    flashes += flashing.Count;
                    flashedThisRound += flashing.Count;
                    foreach (var octopus in flashing)
                    {
                        octopus.EnergyLevel = 0;
                        octopus.Flashed = true;
                        var effected = GetCellsInRadius(Octopuses, octopus, 2, Width, Height);
                        foreach (var item in effected)
                        {
                            if (!item.Flashed)
                            {
                                item.EnergyLevel++;
                            }

                        }
                    }
                    flashing = Octopuses.Where(o => o.EnergyLevel > 9 && !o.Flashed).ToList();
                }
                if (currentRound == 100)
                {
                    retValue = flashes;
                }
                if (flashedThisRound == Octopuses.Count)
                {
                    Result2 = currentRound;
                    break;
                }
                currentRound++;
            }
            return retValue.ToString();
        }
        string IAocTask.Solve2()
        {
            return Result2.ToString();
        }

        static void IncreaseEnergyLevels(List<Octopus> octopuses)
        {
            for (var i = 0; i < octopuses.Count; i++)
            {
                var item = octopuses[i];
                item.EnergyLevel++;
            }
        }
        static bool IsValidPos(int x, int y, int width, int height)
        {
            var retValue = x >= 0 && x < width && y >= 0 && y < height;
            return retValue;
        }
        static List<Octopus> LoadTaskInput(string filePath, out int Width, out int Height)
        {
            var retValue = new List<Octopus>();
            var data = System.IO.File.ReadAllLines(filePath).ToList();

            Height = data.Count;
            Width = data[0].Length;

            for (var y = 0; y < Height; y++)
            {
                var rowData = data[y].ToCharArray();
                for (var x = 0; x < Width; x++)
                {
                    var item = new Octopus(x, y, int.Parse(rowData[x].ToString()));
                    retValue.Add(item);
                }
            }
            return retValue;
        }
        static List<Octopus> GetCellsInRadius(List<Octopus> octopuses, Octopus start, int radius, int width, int height)
        {
            var retValue = new List<Octopus>();
            int[] dx = { 0, 0, -1, 1, -1, -1, 1, 1 };
            int[] dy = { 1, -1, 0, 0, 1, -1, 1, -1 };
            for (var i = 0; i < dx.Length; i++)
            {
                var newX = start.X + dx[i];
                var newY = start.Y + dy[i];
                if (IsValidPos(newX, newY, width, height))
                {
                    var newCell = octopuses[GetCellIndex(width, newX, newY)];
                    retValue.Add(newCell);
                }
            }
            var t = retValue;
            return t;
        }
        static int GetCellIndex(int Width, int x, int y)
        {
            return (Width * y) + x;
        }

    }
}





