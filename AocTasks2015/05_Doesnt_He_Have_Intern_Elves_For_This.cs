using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading;


namespace AdventOfCode.AocTasks2015
{
    [AocTask(2015, 5)]
    public class Doesnt_He_Have_Intern_Elves_For_This : IAocTask
    {
        public string FilePath { get; set; }
        private List<string> NiceStringCandidates;
        private string Sol1 = "";
        private string Sol2 = "";
        public Doesnt_He_Have_Intern_Elves_For_This(string filePath)
        {
            FilePath = filePath;
        }
        static List<string> LoadTaskinput(string filePath)
        {
            return System.IO.File.ReadAllText(filePath).Split("\n").ToList();
        }
        string IAocTask.Solve1()
        {
            var niceCount = 0;
            //NiceStringCandidates = new List<string> { "jchzalrnumimnmhp", "haegwjzuvuyypxyu", "dvszwmarrgswjxmb" };
            foreach (var item in NiceStringCandidates)
            {
                if (!IsInvalidString(item) && HasLettersInRow(item) && ContainsVowels(item))
                {
                    niceCount++;
                }
            }
            Sol1 = niceCount.ToString();
            return Sol1;
        }
        private bool ContainsVowels(string candidate)
        {
            var regexPattern = @"[aeiou]";
            MatchCollection vowelMatches = Regex.Matches(candidate, regexPattern);
            return vowelMatches.Count >= 3;
        }
        private bool HasLettersInRow ( string candidate)
        {
            string regexPattern = @"(.)\1+";
            var retValue = Regex.IsMatch(candidate, regexPattern);
            return retValue;
        }
        private bool IsInvalidString(string candidate)
        {
            string regexPattern = @"ab|cd|pq|xy";
            var retValue = Regex.IsMatch(candidate, regexPattern);
            return retValue;
        }
        string IAocTask.Solve2()
        {
            var niceCount = 0;
            foreach (var item in NiceStringCandidates)
            {
                if (IsNiceWord(item))
                {
                    niceCount++;
                }
            }
            Sol2 = niceCount.ToString();
            return Sol2;
        }
        static bool IsNiceWord(string candidate)
        {
            string pairPattern = @"(\w{2}).*?\1";
            string repeatPattern = @"(\w).\1";

            return Regex.IsMatch(candidate, pairPattern) && Regex.IsMatch(candidate, repeatPattern);
        }
        public void PrepareData()
        {
            NiceStringCandidates = LoadTaskinput(FilePath);
        }
    }
}





