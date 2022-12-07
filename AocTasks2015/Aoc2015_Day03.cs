using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;

namespace AdventOfCode.AocTasks2015
{
    [AocTask(2015, 3)]
    public class Aoc2015_Day03 : IAocTask
    {
        public string FilePath { get; set; }
        public string Directions { get; set; }
        public Aoc2015_Day03(string filePath)
        {
            FilePath = filePath;
        }
        public void PrepareData()
        {
            Directions = System.IO.File.ReadAllText(FilePath);
        }
        string IAocTask.Solve1()
        {
            HashSet<(int, int)> houses = VisitHouses(Directions);
            return houses.Count.ToString();
        }
        private static HashSet<(int, int)> VisitHouses(string directions)
        {
            int x = 0;
            int y = 0;
            var houses = new HashSet<(int, int)> { (x, y) };

            for (var i = 0; i < directions.Length; i++)
            {
                var direction = directions[i];
                switch (direction)
                {
                    case '>':
                        x++;
                        break;
                    case '<':
                        x--;
                        break;
                    case '^':
                        y++;
                        break;
                    case 'v':
                        y--;
                        break;
                }
                houses.Add((x, y));
            }

            return houses;
        }


        string IAocTask.Solve2()
        {
            HashSet<(int, int)> housesSanta = VisitHouses(new string(Directions.Where((c, i) => i % 2 != 0).ToArray()));
            HashSet<(int, int)> housesRobo = VisitHouses(new string(Directions.Where((c, i) => i % 2 == 0).ToArray()));
            return (housesSanta.Union(housesRobo)).Count().ToString();
        }
    }
}