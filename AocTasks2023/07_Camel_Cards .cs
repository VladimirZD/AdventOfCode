using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;
using Microsoft.CodeAnalysis;
using System.Diagnostics;

namespace AdventOfCode.AocTasks2023
{
    [AocTask(2023, 7)]
    public class Camel_Cards(string filePath) : IAocTask
    {
        public string FilePath { get; set; } = filePath;
        public string Sol1 { get; set; }
        public string Sol2 { get; set; }
        private Dictionary<char, int> CardValues1 = new Dictionary<char, int> { { 'A', 14 }, { 'K', 13 }, { 'Q', 12 }, { 'J', 11 }, { 'T', 10 }, { '9', 9 }, { '8', 8 }, { '7', 7 }, { '6', 6 }, { '5', 5 }, { '4', 4 }, { '3', 3 }, { '2', 2 } };
        private Dictionary<char, int> CardValues2 = new Dictionary<char, int> { { 'A', 14 }, { 'K', 13 }, { 'Q', 12 }, { 'T', 10 }, { '9', 9 }, { '8', 8 }, { '7', 7 }, { '6', 6 }, { '5', 5 }, { '4', 4 }, { '3', 3 }, { '2', 2 }, { 'J', 1 } };

        private struct Hand { public string Cards; public string Cards2; public int Bid; public int Score; public int Score2; }
        private List<Hand> Hands;

        public void PrepareData()
        {
            Hands = new List<Hand>();
            var textData = File.ReadAllLines(FilePath);
            //textData = "32T3K 765\nT55J5 684\nKK677 28\nKTJJT 220\nQQQJA 483".Split("\n", StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in textData)
            {
                var parts = item.Split(" ");
                var hand = new Hand();
                hand.Bid = int.Parse(parts[1]);
                
                hand.Cards = parts[0];
                hand.Score = GetHandStrength(hand.Cards);
                hand.Cards2= GetJokerHand(hand);
                hand.Score2 = GetHandStrength(hand.Cards2);
                Hands.Add(hand);
            }
        }
        string IAocTask.Solve1()
        {
            Hands.Sort((hand1, hand2) =>
            {
                int strengthComparison = hand1.Score.CompareTo(hand2.Score);
                if (strengthComparison == 0)
                {
                    return CompareHands(hand1.Cards, hand2.Cards, CardValues1);
                }
                return strengthComparison;
            });

            var winings = Hands.Select((hand, index) => hand.Bid * (index + 1)).Sum();
            Sol1 = winings.ToString();
            Debug.Assert((Sol1 == "6440") || (Sol1 == "249726565") );
            return Sol1;
        }
        public int GetHandStrength(string hand)
        {
            var groupedValues = hand.GroupBy(v => v).OrderByDescending(grp => grp.Count()).ThenByDescending(grp => grp.Key).ToList();
            var score = 0;
            if (groupedValues.Any(grp => grp.Count() == 5))
            {
                score = 100_000;
            }
            else if (groupedValues.Any(grp => grp.Count() == 4))
            {
                score = 80_000;
            }
            else if (groupedValues.Any(grp => grp.Count() == 3) && groupedValues.Any(grp => grp.Count() == 2))
            {
                score = 60_000;
            }
            else if (groupedValues.Any(grp => grp.Count() == 3))
            {
                score = 40_000;
            }
            else if (groupedValues.Count(grp => grp.Count() == 2) == 2)
            {
                score = 20_000;
            }
            else if (groupedValues.Count(grp => grp.Count() == 2) == 1)
            {
                score = 10_000;
            }
            else if (groupedValues.Count == 5)
            {
                return 1_000;
            }
            return score;
        }
        private int CompareHands(string hand1, string hand2, Dictionary<char, int> cardValues)
        {
            {
                List<int> values1 = hand1.Select(c => cardValues[c]).ToList();
                List<int> values2 = hand2.Select(c => cardValues[c]).ToList();
                for (int i = 0; i < values1.Count; i++)
                {
                    if (values1[i] != values2[i])
                    {
                        return values1[i].CompareTo(values2[i]);
                    }
                }
            }
            return 0;
        }
        string IAocTask.Solve2()
        {
            Hands.Sort((hand1, hand2) =>
            {
                int strengthComparison = hand1.Score2.CompareTo(hand2.Score2);
                if (strengthComparison == 0)
                {
                    return CompareHands(hand1.Cards, hand2.Cards, CardValues2);
                }
                return strengthComparison;
            });
            var winings = Hands.Select((hand, index) => hand.Bid * (index + 1)).Sum();
            Sol2 = winings.ToString();
            Debug.Assert((Sol2 == "5905") || (Sol2 == "251135960"));
            return Sol2;
        }
        private string GetJokerHand(Hand hand)
        {
            var retValue = hand.Cards;
            if (hand.Cards.Contains('J'))
            {
                var cardToAdd = hand.Cards.Where(c => c != 'J').GroupBy(c => c).OrderByDescending(g => g.Count()).Select(g => g.Key).FirstOrDefault('J');
                retValue = hand.Cards.Replace('J', cardToAdd);
            }
            return retValue;
        }
    }
}

