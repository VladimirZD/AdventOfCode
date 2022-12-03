//using AdventOfCode.Attributes;
//using AdventOfCode.Interfaces;
//using System.Runtime.InteropServices;
//using static AdventOfCode.AocTasks2021.Aoc2021_Day09;
//using static System.Runtime.InteropServices.JavaScript.JSType;

//namespace AdventOfCode.AocTasks2015
//{
//    [AocTask(2015, 3)]
//    public class Aoc2015_Day03 : IAocTask
//    {
//        public List<List<int>> PackageData { get; set; }
//        public Aoc2015_Day03(string filePath)
//        {
//            PackageData = LoadTaskinput(filePath);
//        }
//        static List<List<int>> LoadTaskinput(string filePath)
//        {
//            var data = System.IO.File.ReadAllLines(filePath).Select(l => l.Split('x').Select(int.Parse).ToList()).ToList();
//            return data;
//        }

//        public static int GetArea(int l, int w, int h)
//        {
//            /*2*l*w + 2*w*h + 2*h*l*/
//            return 2 * (l * w + w * h + h * l);
//        }
//        string IAocTask.Solve1()
//        {
//            var totalArea = 0L;
//            foreach (var package in PackageData)
//            {
//                var area = GetArea(package[0], package[1], package[2]);
//                var sides = package.OrderBy(p => p).Take(2).ToList();
//                var extra = sides[0] * sides[1];
//                //var ribbon =
//                totalArea += (area + extra);
//            }
//            return totalArea.ToString();
//        }
//        string IAocTask.Solve2()
//        {

//            /*
//             *The elves are also running low on ribbon. Ribbon is all the same width, so they only have to worry about the length they need to order, which they would again like to be exact.
//                The ribbon required to wrap a present is the shortest distance around its sides, or the smallest perimeter of any one face. Each present also requires a bow made out of ribbon as well; 
//                the feet of ribbon required for the perfect bow is equal to the cubic feet of volume of the present. Don't ask how they tie the bow, though; they'll never tell.
//                For example:
//                A present with dimensions 2x3x4 requires 2+2+3+3 = 10 feet of ribbon to wrap the present plus 2*3*4 = 24 feet of ribbon for the bow, for a total of 34 feet.
//                A present with dimensions 1x1x10 requires 1+1+1+1 = 4 feet of ribbon to wrap the present plus 1*1*10 = 10 feet of ribbon for the bow, for a total of 14 feet.
//                How many total feet of ribbon should they order?
//             * */
//            var totalRibon = 0L;
//            foreach (var package in PackageData)
//            {
//                var sides = package.OrderBy(p => p).Take(2).ToList();
//                var ribbon = 2 * (sides[0] + sides[1]);
//                var bow = package[0] * package[1] * package[2];
//                totalRibon += (ribbon + bow);
//            }
//            return totalRibon.ToString();
//        }
//    }
//}





