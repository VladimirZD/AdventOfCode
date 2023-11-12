using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;
using System.Linq.Expressions;

namespace AdventOfCode.AocTasks2022
{
    [AocTask(2016, 2)]
    public class Bathroom_Security : IAocTask
    {
        private string Sol1;
        private string Sol2;
        public string FilePath { get; set; }
        public string[] Input { get; set; }

        public Bathroom_Security(string filePath)
        {
            FilePath = filePath;
            Input = File.ReadAllLines(filePath);
            //Input = "ULL\r\nRRDDD\r\nLURDL\r\nUUUUD".Split("\r\n");
        }
        public void PrepareData()
        {
                        
        }
        string IAocTask.Solve1()
        {
            string[] keyPad = new string[] { "123", "456", "789" };
            Sol1 = FindDigits(keyPad,1,1);
            return Sol1.ToString();
        }
        string IAocTask.Solve2()
        {
            string[] keyPad = new string[] { "xx1xx", "x234x", "56789","xABCx","xxDxx" };
            Sol2 = FindDigits(keyPad,0,2);
            return Sol2.ToString();
        }
        private string  FindDigits(string[] keyPad,int startX,int startY)
        {
            int currentX = startX;
            int currentY = startY;
            int dx = 0;
            int dy = 0;
            var digits = new List<char>();
            var keyPadLength = keyPad.Length-1;
            foreach (var line in Input) 
            {
                foreach (var move in line)
                {
                    dx = 0;
                    dy = 0;
                    switch (move)
                    {
                        case 'U':
                            dy = -1;
                            break;
                        case 'D':
                            dy = 1;
                            break;
                        case 'L':
                            dx = -1;
                            break;
                        case 'R':
                            dx = 1;
                            break;
                        default:
                            break;
                    }
                    if (currentX + dx>=0 && currentX + dx <= keyPadLength && currentY + dy >= 0 && currentY + dy <= keyPadLength && keyPad[currentX + dx][currentY+dy]!='x')
                    {
                        currentX += dx;
                        currentY += dy;
                    }
                }
                digits.Add(keyPad[currentY][currentX]);
            }
            return string.Join("",digits);
        }
    }
}