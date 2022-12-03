using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;

namespace AdventOfCode.AocTasks2021
{
    [AocTask(2021, 14)]
    public class Aoc2021_Day14 : IAocTask
    {
        private Dictionary<string, string> Transformations { get; set; }
        private Dictionary<string, long> ElementPairs { get; set; }
        private string StartTemplate { get; set; }
        public Aoc2021_Day14(string filePath)
        {
            LoadTaskInput(filePath);
        }
        string IAocTask.Solve1()
        {
            var retValue = 0L;
            var step = 0;
            var maxStep = 40;

            ElementPairs = new();
            var elements = StartTemplate.ToCharArray();
            for (var i = 0; i < elements.Length - 1; i++)
            {
                var key = elements[i].ToString() + elements[i + 1].ToString();
                ElementPairs[key] = ElementPairs.GetValueOrDefault(key) + 1;
            }
            while (step < maxStep)
            {
                var newElementPairs = new Dictionary<string, long>();
                foreach (var pair in ElementPairs)
                {
                    if (Transformations.TryGetValue(pair.Key, out string value))
                    {
                        var newElement = value;
                        var newPair1 = pair.Key[0] + newElement;
                        var newPair2 = newElement + pair.Key[1];
                        newElementPairs[newPair1] = newElementPairs.GetValueOrDefault(newPair1) + pair.Value;
                        newElementPairs[newPair2] = newElementPairs.GetValueOrDefault(newPair2) + pair.Value;
                    }
                    else
                    {
                        throw new Exception("OOPS");
                    }
                }
                ElementPairs = newElementPairs;
                step++;
                if (step == 10)
                {
                    retValue = GetMaxMinusMin(ElementPairs, StartTemplate);
                }
            }
            return retValue.ToString();
        }
        string IAocTask.Solve2()
        {
            var retValue = GetMaxMinusMin(ElementPairs, StartTemplate);
            return retValue.ToString();
        }
        static long GetMaxMinusMin(Dictionary<string, long> elementPairs, string startTemplate)
        {
            var lettersCnt = new Dictionary<char, long>();
            foreach (var pair in elementPairs)
            {
                var letter = pair.Key[0];
                lettersCnt[letter] = lettersCnt.GetValueOrDefault(letter) + pair.Value;
                letter = pair.Key[1];
                lettersCnt[letter] = lettersCnt.GetValueOrDefault(letter) + pair.Value;
            }
            lettersCnt[startTemplate[0]]++;
            lettersCnt[startTemplate[^1]]++;
            var sortedLetters = lettersCnt.Select(l => l.Value / 2).OrderByDescending(l => l).ToList();
            return sortedLetters.First() - sortedLetters.Last();
        }
        private void LoadTaskInput(string filePath)
        {
            var data = File.ReadLines(filePath).ToList();
            var retValue = new Dictionary<string, string>();
            var splitLineIndex = data.IndexOf(data.Where(l => l == "").Single());
            StartTemplate = data[0].ToString();
            Transformations = data.Skip(splitLineIndex + 1).Take(data.Count - splitLineIndex).Select(i => i.Split(" -> ")).ToDictionary(i => i[0], i => i[1]);
        }
    }
}





