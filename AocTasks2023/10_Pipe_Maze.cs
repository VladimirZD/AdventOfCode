using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Diagnostics.Tracing.Parsers.Kernel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Numerics;
using TraceReloggerLib;

namespace AdventOfCode.AocTasks2023
{
    [AocTask(2023, 10)]
    public class Pipe_Maze (string filePath) : IAocTask
    {
        public string FilePath { get; set; } = filePath;
        public string Sol1 { get; set; }
        public string Sol2 { get; set; }
        public struct Cell {public int X;public int Y;public char Value;public bool HasAnimal;}
        private  List<Cell> MapData {get;set;}
        private struct CellOffset {public int[] DX;public int[] DY;}
        private Dictionary<char,string> ConnectingPipes =new Dictionary<char, string> {{'|',"LJ"}};
          /*
        | is a vertical pipe connecting north and south.
- is a horizontal pipe connecting east and west.
L is a 90-degree bend connecting north and east.
J is a 90-degree bend connecting north and west.
7 is a 90-degree bend connecting south and west.
F is a 90-degree bend connecting south and east.
. is ground; there is no pipe in this tile.
S is the starting position of the animal; there is a pipe on this tile, but your sketch doesn't show what shape the pipe has.
        */
        
        public void PrepareData()
        {
            MapData=new List<Cell>();
            var textData = File.ReadAllLines(FilePath);
            textData = ".....\n.S-7.\n.|.|.\n.L-J.\n.....".Split("\n",StringSplitOptions.RemoveEmptyEntries);
            for (var y=0;y<textData.Length;y++)
            {
                for (var x=0;x<textData[y].Length;x++)
                {
                    var hasAnimal=textData[y][x]=='S'?true:false;
                    var cell =new Cell{X=x,Y=y,Value=textData[y][x],HasAnimal=hasAnimal};
                    MapData.Add(cell);
                }
            }
          
        }
      
        
        string IAocTask.Solve1()
        {
            Debug.Assert((Sol1 == "114") || (Sol1 == "1762065988"));
            return Sol1;
        }
        string IAocTask.Solve2()
        {
            Debug.Assert((Sol2 == "2") || (Sol2 == "1066"));
            return Sol2;
        }
     }
}

