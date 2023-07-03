using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;

namespace AdventOfCode.AocTasks2021
{
    [AocTask(2021, 7)]
    public class The_Treachery_of_Whales : IAocTask
    {
        public string FilePath { get; set; }
        public List<int> Crabs { get; set; }

        public The_Treachery_of_Whales(string filePath)
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
                }
            }
            return minFuelUsed.ToString();
        }
        string IAocTask.Solve2()
        {
            var minFuelUsed = int.MaxValue;
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
                }
            }
            return minFuelUsed.ToString();
        }
    }
}