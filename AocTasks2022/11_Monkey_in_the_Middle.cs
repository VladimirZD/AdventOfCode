using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;
using System.Diagnostics;

namespace AdventOfCode.AocTasks2022
{
    [AocTask(2022, 11)]
    public class Monkey_in_the_Middle: IAocTask
    {
        public class Monkey
        {
            public int ID { get; set; }
            public string Operation { get; set; }
            public string OperationArgument { get; set; }
            public int TestDivisibleBy { get; set; }
            public int OnTrueThrowTo{ get; set; }
            public int OnFalseThrowTo { get; set; }
            public Queue<long> Items { get; set; }
            public long InspectedItems { get; set; }

            public Monkey(int id, string operation, string operationArgument, int testDivisibleBy, int onTrueThrowTo, int onFalseThrowTo, Queue<long> items)
            {
                ID = id;
                Operation = operation;
                OperationArgument = operationArgument;
                TestDivisibleBy = testDivisibleBy;
                OnTrueThrowTo = onTrueThrowTo;
                OnFalseThrowTo = onFalseThrowTo;
                Items = items;
                InspectedItems = 0L;
            }
        }
        private List<Monkey> Monkeys;
        private string[] Input { get; set; }
        private long Result1 { get; set; }
        private long Result2 { get; set; }
        public Monkey_in_the_Middle(string filePath)
        {
            Input = File.ReadAllLines(filePath); 
            //Input = "Monkey 0:\r\n  Starting items: 79, 98\r\n  Operation: new = old * 19\r\n  Test: divisible by 23\r\n    If true: throw to monkey 2\r\n    If false: throw to monkey 3\r\n\r\nMonkey 1:\r\n  Starting items: 54, 65, 75, 74\r\n  Operation: new = old + 6\r\n  Test: divisible by 19\r\n    If true: throw to monkey 2\r\n    If false: throw to monkey 0\r\n\r\nMonkey 2:\r\n  Starting items: 79, 60, 97\r\n  Operation: new = old * old\r\n  Test: divisible by 13\r\n    If true: throw to monkey 1\r\n    If false: throw to monkey 3\r\n\r\nMonkey 3:\r\n  Starting items: 74\r\n  Operation: new = old + 3\r\n  Test: divisible by 17\r\n    If true: throw to monkey 0\r\n    If false: throw to monkey 1".Split("\r\n").ToArray();
        }
        public void PrepareData()
        {
            Monkeys = new List<Monkey>();
            for (var i=0;i<Input.Length;i+=7)
            {
                var id = int.Parse(Input[i][7..(Input[i].Length - 1)]);
                var items = new Queue<long>(Input[i + 1][18..].Split(",").Select(i => long.Parse(i)));
                var operation = Input[i + 2][23..24];
                var operationArgument = Input[i + 2][25..];
                var testDivisibleBy = int.Parse(Input[i + 3][21..]);
                var onTrueThrowTo = int.Parse(Input[i + 4][29..]);
                var onFalseThrowTo= int.Parse(Input[i + 5][29..]);
                var monkey = new Monkey(id,operation,operationArgument,testDivisibleBy,onTrueThrowTo,onFalseThrowTo,items);
                Monkeys.Add(monkey);
            }
        }
        private void PlayGame(int rounds,Func<long,long> reduceWorryLevel)
        {
            for (var r = 0; r < rounds; r++)
            {
                for (var i = 0; i < Monkeys.Count; i++)
                {
                    var monkey = Monkeys[i];
                    while (monkey.Items.Count > 0)
                    {
                        Monkeys[i].InspectedItems++;
                        var startLevel = monkey.Items.Dequeue();
                        long argument = monkey.OperationArgument == "old" ? startLevel : long.Parse(monkey.OperationArgument);
                        long newLevel = (monkey.Operation == "+" ? startLevel + argument : startLevel * argument);
                        newLevel=reduceWorryLevel(newLevel);
                        var isDivisibleBy = (newLevel % monkey.TestDivisibleBy) == 0;
                        var throwTo = isDivisibleBy ? monkey.OnTrueThrowTo : monkey.OnFalseThrowTo;
                        Monkeys[throwTo].Items.Enqueue(newLevel);
                    }
                }
            }
        }
        string IAocTask.Solve1()
        {
            PrepareData();
            PlayGame(20,level=>level/3);
            Result1 = Monkeys.OrderByDescending(i => i.InspectedItems).Take(2).Aggregate(1L, (Result1, monkey) => Result1*monkey.InspectedItems); 
            //Debug.Assert(Result1 == 99852);
            return Result1.ToString();
        }
        string IAocTask.Solve2()
        {
            PrepareData();
            long reduceLevel = Monkeys.Aggregate(1L, (reduceLevel, monkey) => reduceLevel* monkey.TestDivisibleBy);
            PlayGame(10000, level=>level % reduceLevel);
            Result2 = Monkeys.OrderByDescending(i => i.InspectedItems).Take(2).Aggregate(1L, (Result2, monkey) => Result2 * monkey.InspectedItems);
            //Debug.Assert(Result1 == 25935263541);
            return Result2.ToString();
        }
    }
}


