using BenchmarkDotNet.Attributes;

namespace AdventOfCode.Interfaces
{
    
    internal interface IAocTask
    {
        
        public string Solve1();
        public string Solve2();
        public void PrepareData();
    }
}
