using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;
using System.Diagnostics;

namespace AdventOfCode.AocTasks2022
{
    [AocTask(2022, 8)]
    public class Treetop_Tree_House : IAocTask
    {

        public string[] Input { get; set; }
        public char[,] Trees { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Result1 { get; set; }
        public int Result2 { get; set; }
        public Treetop_Tree_House(string filePath)
        {
            Input = File.ReadAllLines(filePath);
            //Input = "30373\n25512\n65332\n33549\n35390".Split('\n');
        }

        public void PrepareData()
        {
            Width = Input.Length;
            Height = Input[0].Length;
            Trees = new char[Height, Width];
            for (int y = 0; y <= Height - 1; y++)
            {
                for (int x = 0; x <= Width - 1; x++)
                {
                    Trees[x, y] = Input[y][x];
                }
            }
        }
        string IAocTask.Solve1()
        {
            var maxX = Width - 1;
            var maxY = Height - 1;
            var visibleCnt = (maxX + maxY) * 2;

            Result2 = 0;
            for (var y = 1; y < maxY; y++)
            {
                for (var x = 1; x < maxX; x++)
                {
                    var currentTree = Trees[x, y];
                    var visible = CheckTreeStatus(x, y, currentTree, out int distanceScore);
                    Result2 = distanceScore > Result2 ? distanceScore : Result2;
                    visibleCnt += visible ? 1 : 0;
                }
            }
            return visibleCnt.ToString();
        }

        private bool CheckTreeStatus(int treeX, int treeY, int currentTree, out int totalDistanceScore)
        {
            var visible = false;
            var treeHasView = false;
            var distanceScore = 0;

            for (var x = treeX - 1; x >= 0; x--)
            {
                visible = Trees[x, treeY] < currentTree;
                distanceScore++;
                if (!visible) break;
            }
            treeHasView = treeHasView || visible;
            totalDistanceScore = distanceScore;

            distanceScore = 0;
            for (var x = treeX + 1; x < Width; x++)
            {
                visible = Trees[x, treeY] < currentTree;
                distanceScore++;
                if (!visible) break;
            }
            treeHasView = treeHasView || visible;
            totalDistanceScore *= distanceScore;

            distanceScore = 0;
            for (var y = treeY - 1; y >= 0; y--)
            {
                visible = Trees[treeX, y] < currentTree;
                distanceScore++;
                if (!visible) break;
            }
            treeHasView = treeHasView || visible;
            totalDistanceScore *= distanceScore;

            distanceScore = 0;
            for (var y = treeY + 1; y < Height; y++)
            {
                visible = Trees[treeX, y] < currentTree;
                distanceScore++;
                if (!visible) break;
            }
            treeHasView = treeHasView || visible;
            totalDistanceScore *= distanceScore;
            return treeHasView;
        }
        string IAocTask.Solve2()
        {
            return Result2.ToString();
        }
        

    }
}

