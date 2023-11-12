using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;
using System.Linq.Expressions;
using System.Net.Http.Headers;

namespace AdventOfCode.AocTasks2022
{
    [AocTask(2016, 3)]
    public class Squares_With_Three_Sides : IAocTask
    {
        private string Sol1;
        private string Sol2;
        public string FilePath { get; set; }
        public int[][] Input { get; set; }

        public Squares_With_Three_Sides(string filePath)
        {
            FilePath = filePath;
            Input = File.ReadAllText(filePath).Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                              .Select(line => line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                                                  .Select(int.Parse)
                                                  .ToArray())
                              .ToArray();
        }
        public void PrepareData()
        {
                        
        }
        string IAocTask.Solve1()
        {
            Solve1();
            return Sol1.ToString();
        }
        string IAocTask.Solve2()
        {

            Solve2();
            return Sol2.ToString();
        }
        private void Solve2()
        {
            int cnt = 0;
            for (int i = 0; i < Input.Length; i+=3)
            {
                for (int j = 0; j < 3; j++)
                {
                 
                    var s1 = Input[i][j];
                    var s2 = Input[i+1][j];
                    var s3 = Input[i+2][j];
                    if (IsTriangle(s1, s2, s3))
                    {
                        cnt++;
                    }
                }
            }
            Sol2 = cnt.ToString();
        }
        private void Solve1()
        {
            int cnt = 0;
            foreach (var item in Input)
            {
                var s1 = item[0];
                var s2 = item[1];
                var s3 = item[2];
                if (IsTriangle(s1, s2, s3))
                {
                    cnt++;
                }
            }
            Sol1 = cnt.ToString();
        }
        private static bool  IsTriangle(int s1, int s2, int s3)
        {
            var retValue = false;
            if (s1 + s2 > s3)
            {
                if (s1 + s3 > s2)
                {
                    if (s2 + s3 > s1)
                    {
                        retValue= true;
                    }
                }
            }
            return retValue;
        }
    }
}