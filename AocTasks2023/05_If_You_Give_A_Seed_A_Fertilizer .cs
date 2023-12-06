using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.Diagnostics.Tracing.Parsers.Kernel;
using System.Diagnostics;

namespace AdventOfCode.AocTasks2023
{
    [AocTask(2023, 5)]
    public class If_You_Give_A_Seed_A_Fertilizer(string filePath) : IAocTask
    {
        public string FilePath { get; set; } = filePath;
        public string Sol1 { get; set; }
        public string Sol2 { get; set; }
        //private long[] Seeds1 { get; set; }
        private List<SeedRange> Seeds1 { get; set; }
        private List<SeedRange> Seeds2 { get; set; }
        private Dictionary<string, List<Mapping>> MappingData { get; set; }
        //private Dictionary<long, long> SeedLocations { get; set; }
        private struct Mapping { public long Dest, Source, Range; }
        private struct SeedRange { public long SeedStart, Range; }


        public void PrepareData()
        {
            MappingData = new Dictionary<string, List<Mapping>>();
            Seeds1 = new List<SeedRange>();
            Seeds2 =new List<SeedRange>();

            var textData = File.ReadAllLines(FilePath).Where(l => !string.IsNullOrEmpty(l)).ToArray();
            //textData = "seeds: 79 14 55 13\n\nseed-to-soil map:\n50 98 2\n52 50 48\n\nsoil-to-fertilizer map:\n0 15 37\n37 52 2\n39 0 15\n\nfertilizer-to-water map:\n49 53 8\n0 11 42\n42 0 7\n57 7 4\n\nwater-to-light map:\n88 18 7\n18 25 70\n\nlight-to-temperature map:\n45 77 23\n81 45 19\n68 64 13\n\ntemperature-to-humidity map:\n0 69 1\n1 0 69\n\nhumidity-to-location map:\n60 56 37\n56 93 4".Split("\n", StringSplitOptions.RemoveEmptyEntries);
            var seeds = textData[0].Replace("seeds:", "").Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(i => long.Parse(i)).ToArray();

            var j = 0;
            while (j < seeds.Length)
            {
                var seedRange = new SeedRange { SeedStart = seeds[j], Range = 1 };
                Seeds1.Add(seedRange);
                seedRange = new SeedRange { SeedStart = seeds[j+1], Range = 1 };
                Seeds1.Add(seedRange);
                seedRange = new SeedRange { SeedStart = seeds[j], Range = seeds[j + 1] };
                Seeds2.Add(seedRange);    
                j += 2;
            }
            
            var i = 1;
            var groupKey = "";
            var mappingItems = new List<Mapping>();
            while (i < textData.Length)
            {
                var item = textData[i];
                if (char.IsLetter(item[0]))
                {
                    if (groupKey != "")
                    {
                        MappingData.Add(groupKey, mappingItems);
                        mappingItems = new List<Mapping>();
                    }
                    groupKey = item;
                }
                else
                {
                    var data = item.Split(" ").Select(i => long.Parse(i)).ToArray();
                    var mapping = new Mapping { Dest = data[0], Source = data[1], Range = data[2] };
                    mappingItems.Add(mapping);
                }
                i++;
            }
            MappingData.Add(groupKey, mappingItems);
        }
        string IAocTask.Solve1()
        {
            var minLocation = GetMinLocation(Seeds1);
            //var seedLocations = new Dictionary<long, long>();
            //foreach (var seed in Seeds1)
            //{
            //    var source = seed;
            //    foreach (var map in MappingData)
            //    {
            //        var mapRow = map.Value.Where(d => d.Source <= source && d.Source + d.Range >= source).FirstOrDefault();
            //        source = source + mapRow.Dest - mapRow.Source;
            //    }
            //    seedLocations.Add(seed, source);
            //}
            //var Sol1 = seedLocations.Min(i => i.Value).ToString();
            Sol1 = minLocation.ToString();
            Debug.Assert((Sol1 == "35") || (Sol1 == "322500873"));
            return Sol1;
        }
        string IAocTask.Solve2()
        {
            long minLocation = GetMinLocation(Seeds2);
            Debug.Assert(minLocation == 108956227);
            Sol2 = minLocation.ToString();
            return Sol2;
        }

        private long GetMinLocation(List<SeedRange> seeds)
        {
            var minLocation = long.MaxValue;
            for (int i = 0; i < seeds.Count; i++)
            {
                var seed = seeds[i];
                var j = 0;
                while (j < seed.Range)
                {
                    var source = seed.SeedStart + j;
                    foreach (var map in MappingData)
                    {
                        var mapRow = map.Value.Where(d => d.Source <= source && d.Source + d.Range >= source).FirstOrDefault();
                        source = source + mapRow.Dest - mapRow.Source;
                    }
                    minLocation = Math.Min(minLocation, source);
                    j++;
                }
            }

            return minLocation;
        }
    }
}
