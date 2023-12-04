using AdventOfCode.AocTasks2021;
using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;
using BenchmarkDotNet.Disassemblers;
using System.Diagnostics;

namespace AdventOfCode.AocTasks2023
{
    [AocTask(2023, 3)]
    public class Gear_Ratios(string filePath) : IAocTask
    {
        public string FilePath { get; set; } = filePath;
        public string Sol1 { get; set; }
        public string Sol2 { get; set; }
        public Dictionary<string, List<int>> Gears { get; set; }
        public char[,] Schematic { get; set; }
        private record SymbolTouched(int row, int col,bool isGear,bool IsTouching);

        public void PrepareData()
        {
            var textData= File.ReadAllLines(FilePath);
            //textData = "...661*466\n...*......\n..35..633.\n......#...\n617*......\n.....+.58.\n..592.....\n......755.\n...$.*....\n.664.598..".Split("\n",StringSplitOptions.RemoveEmptyEntries);
            Schematic=new char[textData[0].Length,textData.Length]; 
            Gears=new Dictionary<string, List<int>>();
            for (var i=0; i < textData.Length; i++)
            {
                var item = textData[i];
                for (var j =0;j< item.Length; j++)
                {
                    Schematic[i, j] = item[j];
                    if (Schematic[i, j] == '*')
                    {
                        Gears.Add($"{i}x{j}", []);
                    }
                }
            }
        }
        string IAocTask.Solve1()
        {
            var validNumbers = new List<int>();
            var maxRows = Schematic.GetUpperBound(0);
            var maxCols= Schematic.GetUpperBound(1);
            for (var i = 0; i <= maxRows; i++)
            {
                string numValue="";
                bool isNumber;
                bool isValidNumber = false;
                bool isGear=false;
                var currentGearKey = "";
                for (var j = 0; j <= maxCols; j++)
                {
                    var item = Schematic[i, j];
                    if (char.IsDigit(item))
                    {
                        isNumber = true;
                        numValue += item.ToString();
                        isValidNumber = isValidNumber || IsNumberTouchingSymbol(i, j, maxRows, maxCols, false).IsTouching;
                        var check = IsNumberTouchingSymbol(i, j, maxRows, maxCols, true);
                        isGear = isGear || check.isGear;
                        if (check.isGear)
                        {
                            currentGearKey = $"{check.row}x{check.col}";
                        }
                    }
                    else
                    {
                        if (isValidNumber)
                        {
                            validNumbers.Add(int.Parse(numValue));
                            if (currentGearKey!="")
                            {
                                Gears[currentGearKey].Add(int.Parse(numValue));
                            }
                        }
                        isNumber = false;
                        isGear= false;
                        isValidNumber = false;
                        currentGearKey = "";
                        numValue = "";
                    }
                }
                if (isValidNumber)
                {
                    validNumbers.Add(int.Parse(numValue));
                    if(currentGearKey != "")
                            {
                        Gears[currentGearKey].Add(int.Parse(numValue));
                    }
                }
            }
            Sol1 = validNumbers.Sum().ToString();
            //Debug.Assert(Sol1 == "531561");
            var result = Gears.Where(i => i.Value.Count == 2).ToList().Select(i => i.Value[0] * i.Value[1]).Sum();
            Sol2 = result.ToString();
            //Debug.Assert(Sol2 == "83279367");
            return Sol1;
        }
        private SymbolTouched IsNumberTouchingSymbol(int row, int col, int maxRows, int maxColumns, bool justGearSymbol)
        {
            int[] dx = { 0, 0, -1, 1, -1, -1, 1, 1 };
            int[] dy = { 1, -1, 0, 0, 1, -1, 1, -1 };

            for (var i = 0; i < dx.Length; i++)
            {
                var newRow = row + dx[i];
                var newCol = col + dy[i];
                if (IsValidPos(newRow, newCol, maxRows, maxColumns))
                {
                    var item = Schematic[newRow, newCol];
                    if (justGearSymbol)
                    {
                        if (item == '*')
                        {
                            return new SymbolTouched(newRow, newCol, true, true);
                        }
                    }
                    else
                    {
                        if (!(char.IsDigit(item) || item == '.'))
                        {
                            return new SymbolTouched(newRow, newCol, false, true);
                        }
                    }
                }
            }
            return new SymbolTouched(0, 0, false, false);
        }
        static bool IsValidPos(int x, int y, int width, int height)
        {
            var retValue = x >= 0 && x <= width && y >= 0 && y <= height;
            return retValue;
        }
        string IAocTask.Solve2()
        {
            return Sol2;
        }
    }
}
