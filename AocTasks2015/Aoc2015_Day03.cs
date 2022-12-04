using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;
using System.IO;
using System.Runtime.InteropServices;
using static AdventOfCode.AocTasks2021.Aoc2021_Day09;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AdventOfCode.AocTasks2015
{
    [AocTask(2015, 3)]
    public class Aoc2015_Day03 : IAocTask
    {
        public string Directions{ get; set; }
        public Aoc2015_Day03(string filePath)
        {
            Directions= LoadTaskinput(filePath);
        }
        static string LoadTaskinput(string filePath)
        {
            var input = System.IO.File.ReadAllText(filePath);
            var data = input;
            return data;
        }
        
        string IAocTask.Solve1()
        {
            HashSet<(int, int)> houses = VisitHouses(Directions);
            return houses.Count.ToString();
        }

        static private HashSet<(int, int)> VisitHouses(string directions)
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
            HashSet<(int, int)> housesSanta = VisitHouses(new string(Directions.Where((c, i) => i % 2 !=0).ToArray()));
            HashSet<(int, int)> housesRobo = VisitHouses(new string(Directions.Where((c, i) => i % 2 == 0).ToArray()));
            return (housesSanta.Union(housesRobo)).Count().ToString();
        }
    }
}





