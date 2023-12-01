using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;
using System.Diagnostics;
using System.IO;

namespace AdventOfCode.AocTasks2023
{
    [AocTask(2023, 1)]
    public class Trebuchet(string filePath) : IAocTask
    {
        public string FilePath { get; set; } = filePath;
        public string Sol1 { get; set; }
        public string Sol2 { get; set; }
        public List<string> Lines { get; set; }

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
                var firstDigit= GetDigit(line, false,0,1);
                var secondDigit = GetDigit(line, false, line.Length-1, -1);
                totalCalibration+= firstDigit * 10 + secondDigit;
            }
            Sol1 = totalCalibration.ToString();
            //Debug.Assert(totalCalibration == 54597);
            return Sol1;
        }
        string IAocTask.Solve2()
        {
            var totalCalibration = 0;
            foreach (var line in Lines)
            {
                var firstDigit = GetDigit(line, true, 0, 1);
                var secondDigit = GetDigit(line, true, line.Length - 1, -1);
                totalCalibration += firstDigit * 10 + secondDigit;
            }
            Sol2=totalCalibration.ToString();
            //Debug.Assert(totalCalibration == 54504);
            return Sol2;
        }
        private static int GetDigit(string line, bool searchText,int startIndex, int step)
        {
            var digits = new string[] { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
            var foundDigits = new List<int>();
            var retValue = 0;
            for (var i = startIndex; i < line.Length; i+=step)
            {
                if (char.IsDigit(line[i]))
                {
                    foundDigits.Add(line[i] - '0');
                    retValue = line[i] - '0';
                }
                else if (searchText)
                {
                    for (var j = 0; j < digits.Length; j++)
                    {
                        var pattern = line[i..];
                        if (pattern.Length>=digits[j].Length && pattern.StartsWith(digits[j], StringComparison.InvariantCultureIgnoreCase))
                        {
                            retValue = j + 1;
                            break;
                        }
                    }
                }
                if (retValue != 0)
                {
                    break;
                }
            }
            return retValue;
        }
    }
}
