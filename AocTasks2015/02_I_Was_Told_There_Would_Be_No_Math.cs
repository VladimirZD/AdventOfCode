using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;

namespace AdventOfCode.AocTasks2015
{
    [AocTask(2015, 2)]
    public class I_Was_Told_There_Would_Be_No_Math : IAocTask
    {
        public string FilePath { get; set; }
        public List<List<int>> PackageData { get; set; }
        public I_Was_Told_There_Would_Be_No_Math(string filePath)
        {
            FilePath = filePath;
        }
        public void PrepareData()
        {
            PackageData = System.IO.File.ReadAllLines(FilePath).Select(l => l.Split('x').Select(int.Parse).ToList()).ToList();
        }
        public static int GetArea(int l, int w, int h)
        {
            /*2*l*w + 2*w*h + 2*h*l*/
            return 2 * (l * w + w * h + h * l);
        }
        string IAocTask.Solve1()
        {
            var totalArea = 0L;
            foreach (var package in PackageData)
            {
                var area = GetArea(package[0], package[1], package[2]);
                var sides = package.OrderBy(p => p).Take(2).ToList();
                var extra = sides[0] * sides[1];
                //var ribbon =
                totalArea += (area + extra);
            }
            return totalArea.ToString();
        }
        string IAocTask.Solve2()
        {
            var totalRibon = 0L;
            foreach (var package in PackageData)
            {
                var sides = package.OrderBy(p => p).Take(2).ToList();
                var ribbon = 2 * (sides[0] + sides[1]);
                var bow = package[0] * package[1] * package[2];
                totalRibon += (ribbon + bow);
            }
            return totalRibon.ToString();
        }
    }
}





