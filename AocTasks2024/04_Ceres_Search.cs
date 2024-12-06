using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;
using System.Diagnostics;

namespace AdventOfCode.AocTasks2024
{
    [AocTask(2024, 4)]
    public class Ceres_Search(string filePath) : IAocTask
    {
        public string FilePath { get; set; } = filePath;
        public long Sol1 { get; set; }
        public long Sol2 { get; set; }
        public char[,] TaskData { get; set; }
        public List<(int, int)> StartingXCells = new List<(int, int)>();
        public List<(int, int)> StartingACells = new List<(int, int)>();

        private int[] dx = { -1, -1, -1, 0, 1, 1, 1, 0 }; 
        private int[] dy = { -1, 0, 1, 1, 1, 0, -1, -1 }; 

        public void PrepareData()
        {

            var inputLines = File.ReadAllText(filePath);
            ///inputLines = "MMMSXXMASM\r\nMSAMXMSMSA\r\nAMXSXMAAMM\r\nMSAMASMSMX\r\nXMASAMXAMM\r\nXXAMMXXAMA\r\nSMSMSASXSS\r\nSAXAMASAAA\r\nMAMMMXMMMM\r\nMXMXAXMASX".Replace("\r\n", "\n");

            var rows = inputLines.Split('\n');
            TaskData = new char[rows.Length, rows[0].Length];
            for (int i = 0; i < rows.Length; i++)
            {
                for (int j = 0; j < rows[i].Length; j++)
                {
                    TaskData[i, j] = rows[i][j];
                    if (rows[i][j] == 'X')
                    {
                        StartingXCells.Add((i, j));
                    }
                }
            }
            Sol1 = 0;
            Sol2 = 0;
        }
        string IAocTask.Solve1()
        {
            string pattern= "XMAS";
            foreach (var cell in StartingXCells)
            {
                Sol1+=WalkTheMap(cell.Item1, cell.Item2, pattern);
            }
            Debug.Assert(Sol1 == 18 || Sol1 == 2493);
            return Sol1.ToString();
        }
        string IAocTask.Solve2()
        {
            for (int row = 1; row < TaskData.GetLength(0) - 1; row++)
            {
                for (int col = 1; col < TaskData.GetLength(1) - 1; col++)
                {
                    if (IsXShape(row,col))
                    {
                            Sol2++;
                    }
                }
            }
            Debug.Assert(Sol2 == 1890 || Sol2 == 48);
            return Sol2.ToString();
        }

        private bool IsXShape(int row, int col)
        {

            if (row - 1 < 0 || col - 1 < 0 || row + 1 >= TaskData.GetLength(0) || col + 1 >= TaskData.GetLength(1))
                return false;
            if (TaskData[row, col] != 'A') return false;

            bool isTopLeftBottomRightValid =
                (TaskData[row - 1, col - 1] == 'M' && TaskData[row + 1, col + 1] == 'S') ||
                (TaskData[row - 1, col - 1] == 'S' && TaskData[row + 1, col + 1] == 'M');

            bool isTopRightBottomLeftValid =
                (TaskData[row - 1, col + 1] == 'S' && TaskData[row + 1, col - 1] == 'M') ||
                (TaskData[row - 1, col + 1] == 'M' && TaskData[row + 1, col - 1] == 'S');

            return isTopLeftBottomRightValid && isTopRightBottomLeftValid;
        }

        bool IsPatternFound(int row, int col, int index, int direction,string pattern)
        {
            if (index == pattern.Length) return true;
            if (row < 0 || row >= TaskData.GetLength(0) || col < 0 || col >= TaskData.GetLength(1)) return false;
            if (TaskData[row, col] != pattern[index]) return false;

            return IsPatternFound(row + dx[direction], col + dy[direction], index + 1, direction, pattern);
        }
        private int WalkTheMap(int x, int y,string patern)
        {
            var retValue = 0;
            for (var i = 0; i < 8; i++)
            {
                if (IsPatternFound(x, y, 0, i,patern))
                {
                    retValue++;
                }
            }
            return retValue;
        } 
    }
}
