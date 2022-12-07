using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;

namespace AdventOfCode.AocTasks2022
{
    [AocTask(2016, 6)]
    public class Signals_Noise : IAocTask
    {

        public string FilePath { get; set; }
        public string[] Input { get; set; }
        public string Message1 { get; set; }
        public string Message2 { get; set; }
        
        public Signals_Noise(string filePath)
        {
            FilePath = filePath;
            //Input = "eedadn\ndrvtee\r\neandsr\nraavrd\natevrs\ntsrnev\nsdttsa\nrasrtv\nnssdts\nntnada\nsvetve\ntesnvt\nvntsnd\nvrdear\ndvrsen\nenarar".Split("\n").ToArray();
            Input= System.IO.File.ReadAllLines(FilePath);
            
        }
        
        public void PrepareData()
        {
            var span = Input.AsSpan();
            var maxCnt = 0;
            string msgChar1;
            for (var col= 0; col < span[0].Length; col++)
            {
                var chars = new Dictionary<char, int>();
                msgChar1 = "";
                maxCnt = 0;
                for (var row = 0; row < span.Length; row++)
                {
                    var chr = span[row][col];
                    if (!chars.ContainsKey(chr))
                    {
                        chars.Add(chr, 0);
                    }
                    chars[chr]++;
                    if (chars[chr] > maxCnt)
                    {
                        maxCnt = chars[chr];
                        msgChar1 = chr.ToString();
                    }
                }
                Message1 += msgChar1;
                Message2 += chars.OrderBy(c => c.Value).First().Key.ToString();
            }
            
        }
        string IAocTask.Solve1()
        {
            return Message1;
        }
        string IAocTask.Solve2()
        {
            return Message2;
        }
    }
}