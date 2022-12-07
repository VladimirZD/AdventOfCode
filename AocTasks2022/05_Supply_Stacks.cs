using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;

namespace AdventOfCode.AocTasks2022
{
    [AocTask(2022, 5)]
    public class Supply_Stacks : IAocTask
    {

        public string FilePath { get; set; }
        private List<Stack<char>> Boxes1 { get; set; }
        private List<Stack<char>> Boxes2 { get; set; }
        public Supply_Stacks(string filePath)
        {
            FilePath = filePath;
        }
        public void PrepareData()
        {
            const int colWidth = 4;
            var input = System.IO.File.ReadAllText(FilePath);

            var index = input.IndexOf("\n\n");
            var boxData = input[..index].Split("\n").ToArray();
            //var boxData2 = input[..index];

            var instructions = GetInstructions(input[(index + 2)..].AsSpan());
            //CreateBoxes(colWidth, boxData,boxData2);
            CreateBoxes(colWidth, boxData);
            MoveBoxes(Boxes1, Boxes2, instructions);

            //void CreateBoxes(int colWidth, string[] boxData,string boxData2)
            void CreateBoxes(int colWidth, string[] boxData)
            {

                var colCount = boxData[boxData.Length - 1][boxData[boxData.Length - 1].Length - 2] - 48; // boxData2[boxData2.Length - 2]-48; 

                Boxes1 = new List<Stack<char>>();
                Boxes2 = new List<Stack<char>>();

                for (var i = 0; i < colCount; i++)
                {
                    Boxes1.Add(new Stack<char>());
                    Boxes2.Add(new Stack<char>());
                }
                var boxSpan = boxData.AsSpan();
                for (var row = boxSpan.Length - 2; row >= 0; row--)
                {
                    for (var col = 0; col < colCount; col++)
                    {
                        var elementPos = col * colWidth + 1;
                        var element = (char)boxSpan[row][elementPos];
                        if (element != 32)
                        {
                            Boxes1[col].Push(element);
                            Boxes2[col].Push(element);
                        }
                    }
                }
            }
            static List<int> GetInstructions(ReadOnlySpan<char> input)
            {
                List<int> instructions = new();
                var isNumericMode = false;
                string numVal = "";
                foreach (var item in input)
                {
                    if (char.IsNumber(item))
                    {
                        isNumericMode = true;
                        numVal += item.ToString();
                    }
                    else if (isNumericMode)
                    {
                        isNumericMode = false;
                        int element = int.Parse(numVal);
                        instructions.Add(element);
                        numVal = "";
                    }
                }
                return instructions;
            }
            static void MoveBoxes(List<Stack<char>> boxes1, List<Stack<char>> boxes2, List<int> instructions)
            {
                for (var z = 0; z < instructions.Count; z += 3)
                {
                    var moveAmount = instructions[z];
                    var moveFromIndex = instructions[z + 1] - 1;
                    var moveToIndex = instructions[z + 2] - 1;
                    var removedBoxes = new char[moveAmount];
                    for (var i = 0; i < moveAmount; i++)
                    {
                        var item = boxes1[(moveFromIndex)].Pop();
                        boxes1[moveToIndex].Push(item);
                        removedBoxes[i] = boxes2[(moveFromIndex)].Pop();
                    }
                    for (var i = moveAmount - 1; i >= 0; i--)
                    {
                        boxes2[(moveToIndex)].Push(removedBoxes[i]);
                    }
                }
            }
        }
        string IAocTask.Solve1()
        {
            var retValue = "";
            foreach (var box in Boxes1)
            {
                retValue += box.Pop().ToString();
            }
            return retValue;
        }
        string IAocTask.Solve2()
        {
            var retValue = "";
            foreach (var box in Boxes2)
            {
                retValue += box.Pop().ToString();
            }
            return retValue;
        }
    }
}