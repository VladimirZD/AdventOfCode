using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;
using System.Diagnostics;
using System.Linq.Expressions;

namespace AdventOfCode.AocTasks2022
{
    [AocTask(2017, 1)]
    public class Inverse_Captcha: IAocTask
    {
        private int Sol1;
        private int Sol2;
        public string FilePath { get; set; }
        public string Input { get; set; }
        public string Message1 { get; set; }
        public string Message2 { get; set; }
        public Inverse_Captcha(string filePath)
        {
            FilePath = filePath;
            Input = File.ReadAllText(filePath).Replace("\n","");
        }
        public void PrepareData()
        {
            
        }
        string IAocTask.Solve1()
        {
            Sol1= CalculateCaptcha(1);
            Debug.Assert(Sol1 == 1223);
            return Sol1.ToString();
        }
        private int CalculateCaptcha(int step)
        {
            var inputLen = Input.Length;
            var sum = 0;
            for (int i = 0; i < Input.Length; i++)
            {
                var firstDigit = Input[i];
                var secondDigit = Input[(i + step)%inputLen];
                sum = sum + (firstDigit == secondDigit ? firstDigit-'0' : 0);
            }
            return sum;
        }
        string IAocTask.Solve2()
        {
            Sol2= CalculateCaptcha(Input.Length/2);
            Debug.Assert(Sol2 == 1284);
            return Sol2.ToString();
        }
    }
}
