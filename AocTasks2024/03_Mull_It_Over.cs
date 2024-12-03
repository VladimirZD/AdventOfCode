using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;
using Microsoft.Diagnostics.Runtime.Utilities;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace AdventOfCode.AocTasks2024
{
    [AocTask(2024, 3)]
    public class Mull_It_Over(string filePath) : IAocTask
    {
        public string FilePath { get; set; } = filePath;
        public long Sol1 { get; set; }
        public long Sol2 { get; set; }
        public string TaskData { get; set; }

        public void PrepareData()
        {
            TaskData = File.ReadAllText(filePath);
            //TaskData = "xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))".ToLower();
            Sol1 = 0;
            Sol2 = 0;
        }
        string IAocTask.Solve1()
        {
            var mulEnabled = true;
            var startIndex = -1;
            var i = 0;
            while (i < TaskData.Length - 4)
            {
                var data = TaskData.Substring(i, 4);

                if (TaskData[i..(i + 4)] == "do()")
                {
                    mulEnabled = true;
                    i += 4;
                    continue;
                }

                if (i + 7 <= TaskData.Length && TaskData[i..(i + 7)] == "don't()")
                {
                    mulEnabled = false;
                    i += 7;
                    continue;
                }
                data = TaskData.Substring(i, 4);
                if (data == "mul(")
                    if (TaskData[i..(i + 4)] == "mul(")
                    {
                        startIndex = i;
                        var endIndex = TaskData.IndexOf(")", startIndex);
                        string[] parts = TaskData[(startIndex + 4)..endIndex].Split(',');
                        if (parts.Length == 2 && long.TryParse(parts[0], out long num1) && long.TryParse(parts[1], out long num2))
                        {
                            Sol1 += (num1 * num2);
                            i = endIndex;
                            if (mulEnabled)
                            {
                                Sol2 += (num1 * num2);
                            }
                        }
                    }
                i++;
            }
            Debug.Assert(Sol1 == 173517243 || Sol1==161);
            return Sol1.ToString();
        }
        
        string IAocTask.Solve2()
        {
            Debug.Assert(Sol2 == 100450138 || Sol2 == 48);
            return Sol2.ToString();
        }
    }
}
