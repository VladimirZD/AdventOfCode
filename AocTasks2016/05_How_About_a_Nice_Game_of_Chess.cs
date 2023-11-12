using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;
using Microsoft.VisualBasic;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode.AocTasks2022
{
    [AocTask(2016, 5)]
    public class How_About_a_Nice_Game_of_Chess : IAocTask
    {
        private string Sol1;
        private string Sol2;
        public string FilePath { get; set; }
        public string Input { get; set; }
        public List<string> ValidRooms { get; set; }

        public How_About_a_Nice_Game_of_Chess(string filePath)
        {
            FilePath = filePath;
            Input = File.ReadAllLines(filePath).First();
        }
        public void PrepareData()
        {
            Sol1 = "";
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
            MD5 md5 = MD5.Create();
            var i = 0;
            var startString = Input;
            var sol2 = Enumerable.Repeat(' ', 8).ToArray();
            var sol2charsFound = 0;

            while (Sol1.Length < 8 || sol2charsFound < 8)
            {
                var stringToHash = startString + i.ToString();
                byte[] inputBytes = Encoding.ASCII.GetBytes(stringToHash);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                string hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                if (hash[0..5] == "00000")
                {
                    if (Sol1.Length < 8)
                    {
                        Sol1 += hash[5];
                    }
                    if (sol2charsFound < 8)
                    {
                        if (int.TryParse(hash[5].ToString(), out int charPosition))
                        {
                            if (charPosition < 8 && sol2[charPosition] == ' ')
                            {
                                sol2[charPosition] = hash[6];
                                sol2charsFound++;
                            }
                        }
                    }
                }
                i++;
            }
            Sol2 = string.Join("", sol2);
        }
    }
}
