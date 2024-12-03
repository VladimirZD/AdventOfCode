using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;
using Microsoft.Diagnostics.Runtime.Utilities;
using System.Diagnostics;
using System.IO;

namespace AdventOfCode.AocTasks2024
{
    [AocTask(2024, 2)]
    public class Red_Nosed_Reports(string filePath) : IAocTask
    {
        public string FilePath { get; set; } = filePath;
        public long Sol1 { get; set; }
        public long Sol2 { get; set; }
        public int[][] TaskData { get; set; }

        public void PrepareData()
        {
            var lines = File.ReadAllLines(filePath);
            //lines = "7 6 4 2 1\r\n1 2 7 8 9\r\n9 7 6 2 1\r\n1 3 2 4 5\r\n8 6 4 4 1\r\n1 3 6 7 9\r\n36 36 38 36 36".Replace("\r\n", "\n").Split('\n');
            TaskData = new int[lines.Length][];
            for (int i = 0; i < lines.Length; i++)
            {
                var numbers = lines[i].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                TaskData[i] = Array.ConvertAll(numbers, int.Parse);
            }
        }
        string IAocTask.Solve1()
        {
            Sol1 = 0;
            Sol2 = 0;
            for (var row = 0; row < TaskData.Length; row++)
            {
                bool rowSafe = IsRowSafe(TaskData[row]);
                if (rowSafe)
                {
                    Sol1++;
                }
                for (var col = 0; col < TaskData[row].Length; col++)
                {
                    var subset = RemoveElementAt(TaskData[row], col);
                    if (IsRowSafe(subset))
                    {
                        Sol2++;
                        break;
                    }
                }
            }
            Debug.Assert(Sol1 == 606 || Sol1==2);
            return Sol1.ToString();
        }
        private bool IsRowSafe(int[] row)
        {
            var rowSafe = true;
            var change = Math.Sign(row[0] - row[1]);
            if (change==0)
            {
                rowSafe = false;
                return rowSafe;
            }
            
            for (var col = 0; col < row.Length - 1; col++)
            {
                var diff = row[col] - row[col + 1];
                if (Math.Sign(diff) != change || Math.Abs(diff) > 3)
                {
                    rowSafe = false;
                    return rowSafe;
                }
            }
            return rowSafe;
        }
        string IAocTask.Solve2()
        {
            Debug.Assert(Sol2 == 644 || Sol2 == 4);
            return Sol2.ToString();
        }
        private int[] RemoveElementAt(int[] original, int indexToRemove)
        {
            var result = new int[original.Length - 1];
            original.AsSpan(0, indexToRemove).CopyTo(result.AsSpan(0, indexToRemove)); 
            original.AsSpan(indexToRemove + 1).CopyTo(result.AsSpan(indexToRemove));  
            return result;
        }
    }
}
