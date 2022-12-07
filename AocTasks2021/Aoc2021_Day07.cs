using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;

namespace AdventOfCode.AocTasks2021
{
    [AocTask(2021, 7)]
    public class Aoc2021_Day07 : IAocTask
    {
        public string FilePath { get; set; }
        public List<int> Crabs { get; set; }

        public Aoc2021_Day07(string filePath)
        {
            FilePath = filePath;
        }
        public void PrepareData()
        {
            Crabs = File.ReadAllText(FilePath).ToString().Split(",").Select(n => int.Parse(n)).ToList();
        }
        string IAocTask.Solve1()
        {

            var minFuelUsed = int.MaxValue;
            int position;
            var destinations = Enumerable.Range(1, Crabs.Max());
            foreach (var destination in destinations)
            {
                var fuelUsed = 0;
                foreach (var crab in Crabs)
                {
                    var distance = Math.Abs(crab - destination);
                    fuelUsed += distance;
                }
                if (fuelUsed < minFuelUsed)
                {
                    minFuelUsed = fuelUsed;
                    position = destination;
                }
            }
            return minFuelUsed.ToString();
        }
        string IAocTask.Solve2()
        {
            var minFuelUsed = int.MaxValue;
            var position = 0;
            var destinations = Enumerable.Range(1, Crabs.Max());
            foreach (var destination in destinations)
            {
                var fuelUsed = 0;
                foreach (var crab in Crabs)
                {
                    var distance = Math.Abs(crab - destination);
                    fuelUsed += (distance * (distance + 1) / 2);
                }
                if (fuelUsed < minFuelUsed)
                {
                    minFuelUsed = fuelUsed;
                    position = destination;
                }
            }
            return minFuelUsed.ToString();
        }
    }
}