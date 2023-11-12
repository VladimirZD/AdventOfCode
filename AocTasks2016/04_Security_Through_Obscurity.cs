using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;
using System.Linq.Expressions;

namespace AdventOfCode.AocTasks2022
{
    [AocTask(2016, 4)]
    public class Security_Through_Obscurity : IAocTask
    {
        private int Sol1;
        private int Sol2;
        public string FilePath { get; set; }
        public string[] Input { get; set; }
        public List<string> ValidRooms { get;set; }

        public Security_Through_Obscurity(string filePath)
        {
            FilePath = filePath;
            Input = File.ReadAllLines(filePath);
            //Input = "aaaaa-bbb-z-y-x-123[abxyz]\r\na-b-c-d-e-f-g-h-987[abcde]\r\nnot-a-real-room-404[oarel]\r\ntotally-real-room-200[decoy]".Split("\r\n");
            //Input = "ULL\r\nRRDDD\r\nLURDL\r\nUUUUD".Split("\r\n");
        }
        public void PrepareData()
        {

        }
        string IAocTask.Solve1()
        {
            Solve();
            return Sol1.ToString();
        }
        string IAocTask.Solve2()
        {

            return Sol2.ToString();
        }
        private void Solve()
        {
            var sectorSum = 0;
            ValidRooms= new List<string>();
            foreach (var line in Input)
            {
                var letters = new Dictionary<char, int>();
                var hashStartIndex = line.IndexOf('[');
                var hash = line[(hashStartIndex + 1)..^1];
                var sectorID = int.Parse(line[(hashStartIndex - 3)..hashStartIndex]);
                for (var i = 0; i < line.Length - hash.Length - 5; i++)
                {
                    var item = line[i];
                    if (item != '-')
                    {
                        if (letters.ContainsKey(item))
                        {
                            letters[item]++;
                        }
                        else
                        {
                            letters.Add(item, 1);
                        }
                    }
                }
                var orderedLetters = letters.OrderByDescending(l => l.Value).ThenBy(l => l.Key).Select(i => i.Key).ToArray();
                string expectedHash= new string(orderedLetters)[0..5];
                if (expectedHash == hash)
                {
                    sectorSum += sectorID;
                    ValidRooms.Add(line);
                }
            }
            Sol1 = sectorSum;
        }
    }
}