using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;

namespace AdventOfCode.AocTasks2022
{
    [AocTask(2022, 3)]
    public class Aoc2022_Day03 : IAocTask
    {
        private string[] Gifts { get; set; }
        public Aoc2022_Day03(string filePath)
        {

            Gifts = LoadTaskinput(filePath);
        }
        static string[] LoadTaskinput(string filePath)
        {

            var input = System.IO.File.ReadAllText(filePath);
            var data = input.Split("\n").Where(l => !string.IsNullOrEmpty(l)).ToArray();
            return data;
        }
        string IAocTask.Solve1()
        {

            var totalScore = 0;
            var gifts = Gifts.AsSpan();
            foreach (var giftData in gifts)
            {
                var halfLen = giftData.Length / 2;
                var usedLetters = new HashSet<char>();
                for (var i = 0; i < halfLen; i++)
                {
                    var gift = giftData[i];
                    if (giftData.IndexOf(giftData[i], halfLen) != -1)
                    {
                        if (!usedLetters.Contains(gift))
                        {
                            int score = GetCharScore(gift);
                            totalScore += score;
                            usedLetters.Add(gift);
                        }
                    }
                }
            }
            return totalScore.ToString();
        }

        private static int GetCharScore(char gift)
        {
            return (int)gift - (char.IsLower(gift) ? 96 : 38);
        }

        string IAocTask.Solve2()
        {
            var totalScore = 0;
            var gifts = Gifts.AsSpan();
            for (var i = 0; i < gifts.Length; i = i + 3)
            {
                var gift1 = gifts[i];
                var gift2 = gifts[i + 1];
                var gift3 = gifts[i + 2];
                for (var j = 0; j < gift1.Length; j++)
                {
                    var gift = gift1[j];
                    if (gift2.IndexOf(gift) != -1 && gift3.IndexOf(gift) != -1)
                    {
                        var score = GetCharScore(gift);
                        totalScore += score;
                        break;
                    }
                }
            }
            return totalScore.ToString();

        }
    }
}





