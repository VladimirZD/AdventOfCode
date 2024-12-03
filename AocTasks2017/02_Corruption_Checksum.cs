using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;
using BenchmarkDotNet.Attributes;
using System.Diagnostics;
using System.Linq.Expressions;

namespace AdventOfCode.AocTasks2022
{
    [AocTask(2017, 2)]
    public class Corruption_Checksum: IAocTask
    {
        private int Sol1;
        private int Sol2;
        private int[][] CheckSumData;
        public string FilePath { get; set; }
        public string Input { get; set; }
        public Corruption_Checksum(string filePath)
        {
            FilePath = filePath;
            Input = File.ReadAllText(filePath).Replace("\r", "");
            CheckSumData = Input.Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(l => l.Split(new[] {' ','\t' }).Select(int.Parse).ToArray()).ToArray();
        }
        public void PrepareData()
        {
            
        }
        string IAocTask.Solve1()
        {
            Sol1 = CalculateChecksum1(CheckSumData);
            Debug.Assert(Sol1 == 46402);
            return Sol1.ToString();
        }
        private int CalculateChecksum1(int[][] data)
        {
            int checkSum = 0;
            for (int i = 0; i < data.Length; i++)
            {
                int[] row = data[i];
                var min = int.MaxValue; 
                var max = int.MinValue;
                for (int j = 0;j<row.Length;j++)
                {
                   var item =row[j];
                    if (item>max)
                    {
                        max = item;
                    }
                    if (item<min)
                    {
                        min = item;
                    }
                }    
                checkSum +=  (max - min);
            }
            return checkSum;
        }
        string IAocTask.Solve2()
        {
            Sol2 = CalculateChecksum2(CheckSumData);
            Debug.Assert(Sol2 == 265);
            return Sol2.ToString();
        }
        private int CalculateChecksum2(int[][] data)
        {
            int checkSum = 0;
            var rowCheckSumFound = false;
            foreach (var row in data)
            {
                for (var i = 0; i < row.Length - 1; i++)
                {
                    var item1 = row[i];
                    for (var j = i+1; j < row.Length; j++)
                    {
                        var item2 = row[j];
                        var result = (Math.Max(item1, item2)) % (Math.Min(item1, item2));
                        if (result == 0)
                        {
                            checkSum += (Math.Max(item1, item2)) / (Math.Min(item1, item2));
                            break;
                        }
                    }
                }
            }
            return checkSum;
        }
    }
}
