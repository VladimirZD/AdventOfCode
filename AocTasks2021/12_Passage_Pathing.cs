using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;

namespace AdventOfCode.AocTasks2021
{
    [AocTask(2021, 12)]
    public class Passage_Pathing : IAocTask
    {
        public string FilePath { get; set; }
        private const string START = "start";
        private const string END = "end";
        public Dictionary<string, string[]> Caves { get; set; }
        public Passage_Pathing(string filePath)
        {
            FilePath = filePath;
        }
        public void PrepareData()
        {
            Caves = File.ReadAllLines(FilePath)
                    .Select(i => i.Split("\r\n -".ToArray()))
                    .Select(i => new[] { new { StartCave = i[0], EndCave = i[1] }, new { StartCave = i[1], EndCave = i[0] } })
                    .SelectMany(x => x)
                    .GroupBy(x => x.StartCave)
                    .ToDictionary(s => s.Key, e => e.Select(p => p.EndCave).ToArray());
        }
        string IAocTask.Solve1()
        {
            var retValue = 0;
            var visitedCaves = new HashSet<string>
            {
                START
            };
            if (Caves != null)
            {
                retValue = WalkThePath(START, Caves, visitedCaves, true);
            }
            return retValue.ToString();
        }
        string IAocTask.Solve2()
        {
            var retValue = 0;
            var visitedCaves = new HashSet<string>
            {
                START
            };
            if (Caves != null)
            {
                retValue = WalkThePath(START, Caves, visitedCaves, false);
            }
            return retValue.ToString();
        }
        static int WalkThePath(string currentCave, Dictionary<string, string[]> caves, HashSet<string> visitedCaves, bool smallCaveVisitsMaxed)
        {
            var retValue = 0;
            if (currentCave == END)
            {
                return 1;
            }
            foreach (var nextCave in caves[currentCave])
            {
                var visited = visitedCaves.Contains(nextCave);
                var isSmallCave = !char.IsUpper(nextCave[0]);
                if (!visited)
                {
                    visitedCaves.Add(nextCave);
                    retValue += WalkThePath(nextCave, caves, visitedCaves, smallCaveVisitsMaxed);
                    visitedCaves.Remove(nextCave);
                }
                else if (visited && !isSmallCave)
                {
                    retValue += WalkThePath(nextCave, caves, visitedCaves, smallCaveVisitsMaxed);
                }
                else if (visited && isSmallCave && !smallCaveVisitsMaxed && nextCave != START)
                {
                    retValue += WalkThePath(nextCave, caves, visitedCaves, true);
                }
            }
            return retValue;
        }
    }
}





