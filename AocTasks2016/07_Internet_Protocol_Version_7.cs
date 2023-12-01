using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;
using Microsoft.VisualBasic;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode.AocTasks2022
{
    [AocTask(2016, 7)]
    public class Internet_Protocol_Version_7 : IAocTask
    {
        private string Sol1;
        private string Sol2;
        public string FilePath { get; set; }
        public string[] Input { get; set; }
        public List<string> ValidRooms { get; set; }

        public Internet_Protocol_Version_7(string filePath)
        {
            FilePath = filePath;
            Input=File.ReadAllLines(filePath);
            Input = ["abba[mnop]qrst", "abcd[bddb]xyyx", "aaaa[qwer]tyui", "ioxxoj[asdfgh]zxcvbn"];
            //Input = new string[] { "vjqhodfzrrqjshbhx[lezezbbswydnjnz]ejcflwytgzvyigz[hjdilpgdyzfkloa]mxtkrysovvotkuyekba" };
            

        }
        public void PrepareData()
        {
            Sol1 = "";
            Sol2 = "";
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
            return;
            /*part 1 works, part 2 never finished*/
            //TODO: Finish this!
            var validIps=new List<string>();
            var invalidIPs =new List<string>();
            int supportsTls = 0;
            foreach(var line in Input)
            {
                {
                    string abbaPattern = @"(\w)(\w)\2\1";
                    string hyperNetPattern = @"\[([^\]]+)\]";
                    var hyperNetMatches = Regex.Matches(line, hyperNetPattern).Cast<Match>().Select(m => m.Groups[1].Value).ToList();
                    string pattern = @"\[.*?\]"; 
                    string noHyperNetLine = Regex.Replace(line, pattern, "");
                    var abbaMatches = Regex.Matches(noHyperNetLine, abbaPattern).Cast<Match>().Select(m => m.Value).ToList(); 
                    if (abbaMatches.Count > 0)
                    {
                        bool supportsTLS = true;
                        foreach (var abba in abbaMatches)
                        {
                            if (abba[0] != abba[1])
                            {
                                if (hyperNetMatches.IndexOf(abba)!=-1)
                                {
                                    supportsTLS = false;
                                }
                            }
                            else
                            {
                                supportsTLS = false;
                            }
                        }
                        if (supportsTLS)
                        {
                            supportsTls++;
                            validIps.Add(line);
                        }
                        else
                        {
                            invalidIPs.Add(line);
                        }
                    }
                    
                }
            }
            Sol1 = supportsTls.ToString();
        }

    }
}
