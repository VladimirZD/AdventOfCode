using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;
using Microsoft.VisualBasic;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode.AocTasks2022
{
    [AocTask(2016, 7)]
    public class Internet_Protocol_Version_7 : IAocTask
    {
        private string Sol1;
        private string Sol2;
        public string FilePath { get; set; }
        public string Input { get; set; }
        public List<string> ValidRooms { get; set; }

        public Internet_Protocol_Version_7(string filePath)
        {
            FilePath = filePath;
            Input = "abbhdwsy";
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
        }
    }
}
