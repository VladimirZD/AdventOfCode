using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;
namespace AdventOfCode.AocTasks2021
{
    [AocTask(2021, 3)]
    public class Aoc2021_Day03 : IAocTask
    {
        public string FilePath { get; set; }
        public List<string> Data { get; set; }
        public Aoc2021_Day03(string filePath)
        {
            FilePath = filePath;
        }
        public void PrepareData()
        {
            Data = System.IO.File.ReadAllLines(FilePath).ToList<string>();
        }
        string IAocTask.Solve1()
        {
            string gammaRate = "";
            string epsilonRate = "";
            var cnt = Data.Count;
            List<int> sums = new() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            foreach (var item in Data)
            {
                for (var i = 0; i < item.Length; i++)
                {
                    sums[i] = sums[i] + int.Parse(item[i].ToString());
                }
            }

            foreach (var sum in sums)
            {
                var element = ((sum / (cnt / 2) >= 1) ? "1" : "0");
                gammaRate += element;
                epsilonRate += ((sum / (cnt / 2) >= 1) ? "0" : "1");
            }
            var retVale = Convert.ToInt32(gammaRate, 2) * Convert.ToInt32(epsilonRate, 2);

            return retVale.ToString();
        }
        string IAocTask.Solve2()
        {
            var oRating = 0;
            var co2Rating = 0;
            var index = 0;
            var startData = Data;

            while (Data.Count > 1)
            {
                var sum = 0;
                var cnt = (float)Data.Count;
                foreach (var item in Data)
                {
                    sum += int.Parse(item[index].ToString());
                }
                var msb = ((sum / (cnt / 2) >= 1) ? "1" : "0");
                Data = Data.Where(i => i[index].ToString() == msb).ToList();
                index++;

            }
            oRating = Convert.ToInt32(Data[0], 2);
            Data = startData;
            index = 0;
            while (Data.Count > 1)
            {
                var sum = 0;
                var cnt = (float)Data.Count;
                foreach (var item in Data)
                {
                    sum += int.Parse(item[index].ToString());
                }
                var msb = ((sum / (cnt / 2) >= 1) ? "1" : "0");
                Data = Data.Where(i => i[index].ToString() != msb).ToList();
                index++;

            }
            co2Rating = Convert.ToInt32(Data[0], 2);
            var retValue = oRating * co2Rating;
            return retValue.ToString();
        }
    }
}