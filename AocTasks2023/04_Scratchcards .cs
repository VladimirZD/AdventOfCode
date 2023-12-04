using AdventOfCode.AocTasks2021;
using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;
using BenchmarkDotNet.Disassemblers;
using Org.BouncyCastle.Utilities;
using System;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Text.RegularExpressions;
using System.Threading;

namespace AdventOfCode.AocTasks2023
{
    [AocTask(2023, 4)]
    public class Scratchcards(string filePath) : IAocTask
    {
        public string FilePath { get; set; } = filePath;
        public string Sol1 { get; set; }
        public string Sol2 { get; set; }
        public char[,] Schematic { get; set; }
        private record Card (int ID,List<int> WinningNumbers,List<int>MyNumbers, int TotalPoints);
        private List<Card> Cards;

        public void PrepareData()
        {
            var textData= File.ReadAllLines(FilePath);
            //textData = "Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53\nCard 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19\nCard 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1\nCard 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83\nCard 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36\nCard 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11".Split("\n",StringSplitOptions.RemoveEmptyEntries);
            Cards = new List<Card>();
            for(var i=0;i<textData.Length;i++)
            {
                var item = textData[i];
                var items = item[(item.IndexOf(":") + 1)..].Split("|");
                List<int> winingNumbers = items[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
                List<int> myNumbers = items[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
                var card = new Card(i+1, winingNumbers, myNumbers,0);
                Cards.Add(card);
            }
        }
        string IAocTask.Solve1()
        {
            var totalScore = 0;
            var i = 0;
            while (i<Cards.Count)
            {
                var card = Cards[i];
                var correctNumbers=card.WinningNumbers.Intersect(card.MyNumbers).ToList();
                if (correctNumbers.Count > 0)
                {
                    var winingSets = correctNumbers.Count - 1;
                    totalScore += (int)Math.Pow(2, winingSets);
                }
                i++;
            }
            Sol1 = totalScore.ToString();
            //Debug.Assert(Sol1 == "23673");
            return Sol1;
        }
        string IAocTask.Solve2()
        {
            var totalCards = 0;
            var originalCards = new List<Card>(Cards);

            while (Cards.Count>0)
            {
                var card = Cards[0];
                var numOfCurrentCard = Cards.Where(c=>c.ID==card.ID).Count();
                Cards.RemoveAll(c => c.ID == card.ID);
                var newList = new List<Card>();
                var correctNumbers = card.WinningNumbers.Intersect(card.MyNumbers).ToList();
                totalCards += numOfCurrentCard;
                if (correctNumbers.Count > 0)
                {
                    var newCards = originalCards.Skip(card.ID).Take(correctNumbers.Count).ToList();
                    for (var i = 0; i < numOfCurrentCard; i++) 
                    {
                        Cards.AddRange(newCards);
                    }
                    Cards = Cards.OrderBy(i=>i.ID).ToList();
                }
            }
            Sol2 = totalCards.ToString();
            //Debug.Assert(Sol2 == "12263631");
            return Sol2;
        }
    }
}
