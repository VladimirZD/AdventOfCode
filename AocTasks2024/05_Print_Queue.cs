using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;
using BenchmarkDotNet.Validators;
using Microsoft.Diagnostics.Tracing.Parsers.AspNet;
using System.Diagnostics;
using System.Xml.Linq;

namespace AdventOfCode.AocTasks2024
{
    [AocTask(2024, 5)]
    public class Print_Queue(string filePath) : IAocTask
    {
        public string FilePath { get; set; } = filePath;
        public long Sol1 { get; set; }
        public long Sol2 { get; set; }
        private Dictionary<int,List<int>> OrderingRules = new Dictionary<int, List<int>>();
        private List<int[]> Updates= new List<int[]>();

        public void PrepareData()
        {
            var inputLines = File.ReadAllText(filePath);
            //inputLines = "47|53\r\n97|13\r\n97|61\r\n97|47\r\n75|29\r\n61|13\r\n75|53\r\n29|13\r\n97|29\r\n53|29\r\n61|53\r\n97|53\r\n61|29\r\n47|13\r\n75|47\r\n97|75\r\n47|61\r\n75|61\r\n47|29\r\n75|13\r\n53|13\r\n\r\n75,47,61,53,29\r\n97,61,53,29,13\r\n75,29,13\r\n75,97,47,61,53\r\n61,13,29\r\n97,13,75,29,47".Replace("\r\n", "\n");
            var inputData = inputLines.Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
            var rules = inputData[0].Split("\n", StringSplitOptions.RemoveEmptyEntries);
            Updates = inputData[1].Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(line => line.Split(',').Select(int.Parse).ToArray()).ToList();

            foreach (var  item in rules)
            {
                var pages=item.Split("|");
                var key=int.Parse(pages[1]);
                if (OrderingRules.ContainsKey(key))
                {
                    OrderingRules[key].Add(int.Parse(pages[0]));
                }
                else
                {
                    OrderingRules.Add(key, new List<int>() {int.Parse(pages[0]) });
                }
            }
            Sol1 = 0;
            Sol2 = 0;
        }
        string IAocTask.Solve1()
        {
            foreach (var update in Updates)
            {
                var index = update.Length / 2;
                if (OrderIt(update))
                {
                    Sol1 += update[index];
                }
                else
                {
                    Sol2 += update[index];
                }
            }
            Debug.Assert(Sol1 == 143 || Sol1 == 5762);
            return Sol1.ToString();
        }
        private bool OrderIt(int[] pages)
        {
            var retValue = true;
            for (var i = 0; i < pages.Length-1; i++)
            {
                for (var j=i+1;j<pages.Length; j++)
                {
                    if (OrderingRules.TryGetValue(pages[i], out var rule))
                    {
                        if (rule.Contains(pages[j]))
                        {
                            (pages[i], pages[j]) = (pages[j], pages[i]);
                            retValue = false;
                        }
                    }
                }
            }
            return retValue;
        }
        string IAocTask.Solve2()
        {
            Debug.Assert(Sol2 == 123 || Sol2 == 4130);
            return Sol2.ToString();
        }
    }
}
