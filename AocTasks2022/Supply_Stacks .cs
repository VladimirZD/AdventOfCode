using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;
using Org.BouncyCastle.Asn1.Cms;
using Org.BouncyCastle.Utilities;
using System.Collections.Generic;

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
            //var input = "    [D]    \n[N] [C]    \n[Z] [M] [P]\n 1   2   3 \n\nmove 1 from 2 to 1\nmove 3 from 1 to 3\nmove 2 from 2 to 1\nmove 1 from 1 to 2";
            var input = System.IO.File.ReadAllText(FilePath);
            var stateData = input.Split("\n\n");
            var boxData = stateData[0].Split("\n").ToArray();
            var instructionData = stateData[1].Replace("move ", "").Replace("from ", "").Replace("to ", "").Split("\n").Where(i=>!string.IsNullOrEmpty(i)).Select(i => i.Split(" ")).ToArray();

            void CreateBoxes(int colWidth, string[] boxData)
            {
                Boxes1= new List<Stack<char>>();
                Boxes2 = new List<Stack<char>>();
                var colCount = int.Parse(boxData[boxData.Length - 1].Max().ToString());
                for (var i = 0; i < colCount; i++)
                {
                    Boxes1.Add(new Stack<char>());
                    Boxes2.Add(new Stack<char>());
                }
                for (var row = boxData.Length - 2; row >= 0; row--)
                {
                    for (var col = 0; col < colCount; col++)
                    {
                        var elementPos = col * colWidth + 1;
                        var element = (char)boxData[row][elementPos];
                        if (element != 32)
                        {
                            Boxes1[col].Push(element);
                            Boxes2[col].Push(element);
                        }
                    }
                }
            }
            static void MoveBoxes(string[][] instructionData, List<Stack<char>> boxes1, List<Stack<char>> boxes2)
            {
                foreach (var move in instructionData)
                {
                    var moveAmount = int.Parse(move[0]);
                    var moveFromIndex = int.Parse(move[1])-1;
                    var moveToIndex = int.Parse(move[2])-1;

                    var source1 = boxes1[moveFromIndex];
                    var source2 = boxes2[moveFromIndex];
                    var destination1 = boxes1[moveToIndex];
                    var destination2 = boxes2[moveToIndex];

                    var removedBoxes = new char[moveAmount];
                    for (var i = 0; i < moveAmount; i++)
                    {
                        var item = source1.Pop();
                        destination1.Push(item);
                        removedBoxes[i] = source2.Pop();
                    }
                    for (var i = moveAmount-1; i >=0; i--)
                    {
                        destination2.Push(removedBoxes[i]);
                    }

                    
                }
            }
            CreateBoxes(colWidth, boxData);
            MoveBoxes(instructionData, Boxes1,Boxes2);
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





