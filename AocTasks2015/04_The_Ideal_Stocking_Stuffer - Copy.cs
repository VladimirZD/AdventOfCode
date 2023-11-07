using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;
using System.Security.Cryptography;


namespace AdventOfCode.AocTasks2015
{
    [AocTask(2015, 4)]
    public class The_Ideal_Stocking_Stuffer : IAocTask
    {
        public string FilePath { get; set; }
        private string SecretKey;
        private string Sol1 = "";
        private string Sol2 = "";
        public The_Ideal_Stocking_Stuffer(string filePath)
        {
            FilePath = filePath;
        }
        static string LoadTaskinput(string filePath)
        {
            return System.IO.File.ReadAllText(filePath).Replace("\n","");
        }

        string IAocTask.Solve1()
        {
            MD5 md5Hash = MD5.Create();
            var i = -1;
            var hash = "111111";
            while (Sol1 == "" || Sol2=="")
            {
                if (hash.Substring(0, 6) == "000000")
                {
                    Sol2 = i.ToString();
                }
                else if (hash.Substring(0, 5) == "00000" && Sol1=="")
                {
                    Sol1=i.ToString();
                }
                i++;
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(SecretKey+i.ToString());
                byte[] hashBytes = md5Hash.ComputeHash(inputBytes);
                hash= Convert.ToHexString(hashBytes);
            }
            return Sol1;
        }
        string IAocTask.Solve2()
        {
            return Sol2;
        }

        public void PrepareData()
        {
            SecretKey = LoadTaskinput(FilePath);
        }
    }
}





