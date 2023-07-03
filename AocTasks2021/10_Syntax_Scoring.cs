using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;

namespace AdventOfCode.AocTasks2021
{
    [AocTask(2021, 10)]
    public class Syntax_Scoring : IAocTask
    {
        public string FilePath { get; set; }
        public Dictionary<char, char> OpenClosePairLookup { get; set; }
        public Dictionary<char, int> CharScoresLookup { get; set; }
        public Dictionary<char, int> AutoCompleteScoresLookup { get; set; }
        public string[] Lines { get; set; }
        public List<long> AutoCompletes { get; set; }
        public Syntax_Scoring(string filePath)
        {
            FilePath = filePath;
        }
        public void PrepareData()
        {
            OpenClosePairLookup = new Dictionary<char, char>()
            {
                { '(',')' },
                { '[',']' },
                { '{','}' },
                { '<','>' },
            };
            CharScoresLookup = new Dictionary<char, int>()
            {
                {')',3 },
                {']',57 },
                {'}',1197 },
                {'>',25137 }
            };
            AutoCompleteScoresLookup = new Dictionary<char, int>()
            {
                {'(',1 },
                {'[',2 },
                {'{',3 },
                {'<',4 }
            };
            Lines = File.ReadAllLines(FilePath);
        }
        string IAocTask.Solve1()
        {
            var errors = new List<char>();
            var autoCompletes = new List<long>();
            var retValue = 0;
            foreach (var line in Lines)
            {
                var ignoreLine = false;
                var stack = new Stack<char>();
                foreach (var letter in line)
                {
                    //opening operations
                    if (OpenClosePairLookup.ContainsKey(letter))
                    {
                        stack.Push(letter);
                    }
                    else
                    {
                        var openingOperation = stack.FirstOrDefault();
                        var expectedToClose = OpenClosePairLookup[openingOperation];
                        if (expectedToClose == letter)
                        {
                            stack.Pop();
                        }
                        else
                        {
                            if (!ignoreLine)
                            {
                                errors.Add(letter);
                            }
                            ignoreLine = true;
                        }
                    }
                }
                if (stack.Count > 0 && !ignoreLine) //need to autocomplete
                {
                    long autoCompleteScore = 0;
                    while (stack.Count > 0)
                    {
                        var operation = stack.Pop();
                        autoCompleteScore = autoCompleteScore * 5 + AutoCompleteScoresLookup[operation];
                    }
                    autoCompletes.Add(autoCompleteScore);
                }
            }
            foreach (var error in errors)
            {
                retValue += CharScoresLookup[error];
            }
            AutoCompletes = autoCompletes;
            return retValue.ToString();
        }
        string IAocTask.Solve2()
        {
            var retValue = AutoCompletes.OrderBy(s => s).ElementAt(AutoCompletes.Count / 2);
            return retValue.ToString();
        }
    }
}





