using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;
using System.Diagnostics;
using System.IO;

namespace AdventOfCode.AocTasks2024
{
    [AocTask(2024, 1)]
    public class Historian_Hysteria(string filePath) : IAocTask
    {
        public string FilePath { get; set; } = filePath;
        private List<int> List1 { get; set; } = new List<int>();
        private List<int> List2 { get; set; } = new List<int>();
        public long  Sol1 { get; set; }
        public long  Sol2 { get; set; }
        private Dictionary<int, int> Dict { get; set; }

        public void PrepareData()
        {
            Dict = new Dictionary<int, int>();
            foreach (var line in File.ReadLines(FilePath))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                List1.Add(int.Parse(parts[0]));
                List2.Add(int.Parse(parts[1]));
                Dict.Add(int.Parse(parts[0]),0);
            }
        }
        string IAocTask.Solve1()
        {
            List1.Sort();
            List2.Sort();

            for (var i = 0; i < List1.Count; i++)
            {
                var distance = Math.Abs(List1[i] - List2[i]);
                Sol1 += distance;
            }
            Debug.Assert(Sol1 == 2000468);
            return Sol1.ToString();
        }
        string IAocTask.Solve2()
        {
            Sol2 = 0;
            foreach (var item in List2)
            {
                if (Dict.TryGetValue(item, out int value))
                {
                    Dict[item] = ++value;
                    Sol2 += item;
                }
            }
            Debug.Assert(Sol2 == 18567089);
            return Sol2.ToString();
        }
    }
}
