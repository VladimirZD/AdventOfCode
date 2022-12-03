using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;

namespace AdventOfCode.AocTasks2022
{
    [AocTask(2022, 2)]
    public class Aoc2022_Day02 : IAocTask
    {
        private string[] Rounds { get; set; }
        public Dictionary<char, char> LooseMoves { get; set; }
        public Dictionary<char, char> WinningMoves { get; set; }
        public Dictionary<char, char> SameMoves { get; set; }
        public Dictionary<char, int> MoveScores { get; set; }

        public Aoc2022_Day02(string filePath)
        {

            WinningMoves = new Dictionary<char, char>()
            {
                { 'A','Y' }, //Rock ,Paper
                { 'B','Z' }, //Paper ,Scissors
                { 'C','X' } //Scissors, Rock
            };
            LooseMoves = new Dictionary<char, char>()
            {
                { 'A','Z' }, //Rock ,Scissors
                { 'B','X' }, //Paper ,Rock
                { 'C','Y' } //Scissors, Paper
            };
            SameMoves = new Dictionary<char, char>()
            {
                { 'A','X' }, //Rock
                { 'B','Y' }, //Paper
                { 'C','Z' } //Scissors
            };

            MoveScores = new Dictionary<char, int>()
            {
                {'X',1}, //Rock
                {'Y',2 }, //Paper
                {'Z',3 } //Scissors
            };
            Rounds = LoadTaskinput(filePath);
        }
        static string[] LoadTaskinput(string filePath)
        {
            var numbers = new List<int>();
            var data = System.IO.File.ReadAllLines(filePath).Where(l => !string.IsNullOrEmpty(l)).ToArray();
            return data;
        }
        string IAocTask.Solve1()
        {
            var roundsSpan = Rounds.AsSpan();
            var totalScore = 0;
            foreach (var round in roundsSpan)
            {
                var moves = round.Split(' ');
                var myMove = char.Parse(moves[1]);
                var roundScore = WinningMoves[char.Parse(moves[0])] == myMove ? 6 : 0;
                roundScore += SameMoves[char.Parse(moves[0])] == myMove ? 3 : 0;
                totalScore += (roundScore + MoveScores[myMove]);
            }
            return totalScore.ToString();
        }
        string IAocTask.Solve2()
        {
            var roundsSpan = Rounds.AsSpan();
            var totalScore = 0;
            foreach (var round in roundsSpan)
            {
                var roundData = round.Split(' ');
                var myMove = WinningMoves[char.Parse(roundData[0])];
                var roundScore = 6;
                var roundStatus = roundData[1];
                switch (roundStatus)
                {
                    case "X": //Loose
                        myMove = LooseMoves[char.Parse(roundData[0])];
                        roundScore = 0;
                        break;
                    case "Y": //Draw
                        myMove = SameMoves[char.Parse(roundData[0])];
                        roundScore = 3;
                        break;
                    default: //WIN
                        break;
                }
                totalScore += (roundScore + MoveScores[myMove]);
            }
            return totalScore.ToString();

        }
    }
}





