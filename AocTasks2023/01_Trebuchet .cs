using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;
using System.IO;

namespace AdventOfCode.AocTasks2022
{
    [AocTask(2023, 1)]
    public class Trebuchet : IAocTask
    {
        public string FilePath { get; set; }
        public string Sol1 { get; set; }
        public string Sol2 { get; set; }
        public List<string> Lines { get; set; }
        public Trebuchet(string filePath)
        {
            FilePath = filePath;
        }
        public void PrepareData()
        {
            var textData = File.ReadAllText(FilePath);
            Lines = textData.Split("\n", StringSplitOptions.None).Where(line => !string.IsNullOrEmpty(line)).Select(line => new string(line.ToArray())).ToList();
        }
        string IAocTask.Solve1()
        {
            var totalCalibration = 0;
            foreach (var line in Lines)
            {
                List<int> foundDigits = GetDigits(line, false);
                totalCalibration += foundDigits[0]*10 + foundDigits[foundDigits.Count - 1];
            }
            Sol1 = totalCalibration.ToString();
            return Sol1;
        }
        string IAocTask.Solve2()
        {
            var totalCalibration = 0;
            foreach (var line in Lines)
            {
                List<int> foundDigits = GetDigits(line,true);
                totalCalibration += foundDigits[0] * 10 + foundDigits[foundDigits.Count - 1];
            }
            Sol2=totalCalibration.ToString();
            return Sol2;
        }
        private static List<int> GetDigits(string line,bool searchText)
        {
            var digits = new string[] { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
            var foundDigits = new List<int>();
            for (var i = 0; i < line.Length; i++)
            {
                if (char.IsDigit(line[i]))
                {
                    foundDigits.Add(line[i] - '0');
                }
                else if (searchText)
                {
                    for (var j = 0; j < digits.Length; j++)
                    {
                        
                        if (line[i..].StartsWith(digits[j], StringComparison.InvariantCultureIgnoreCase))
                        {
                            foundDigits.Add(j + 1);
                        }
                    }
                }
            }
            return foundDigits;
        }
    }
}
