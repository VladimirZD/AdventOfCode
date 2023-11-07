using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;
using System.Data;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading;
namespace AdventOfCode.AocTasks2015
{
    [AocTask(2015, 6)]
    public class Probably_a_Fire_Hazard : IAocTask
    {
        public string FilePath { get; set; }
        private int[,] Lights { get;set; }
        private int[,] Lights2 { get; set; }
        private string[] Instructions { get; set; }
        private string Sol1 = "";
        private string Sol2 = "";
        public Probably_a_Fire_Hazard(string filePath)
        {
            FilePath = filePath;
        }
        string IAocTask.Solve1()
        {
            foreach (var instruction in Instructions)
            {
                var commandData = GetCurrentCommand(instruction).Split(',');
                string command = commandData[0]; 
                int startX = int.Parse(commandData[1]);
                int startY = int.Parse(commandData[2]);
                int endX = int.Parse(commandData[3]);
                int endY = int.Parse(commandData[4]);
                for (var x = startX; x <= endX;x++)
                {
                    for (var y = startY; y<= endY; y++)
                    {
                        Lights[x, y] = GetBlubState(command, Lights[x,y]);
                        Lights2[x, y] = GetBlubState2(command, Lights2[x, y]);
                    }
                }
                //Sol1 = Lights.Cast<int>().Count(b => b).ToString();
            }
            Sol1= Lights.Cast<int>().Count(b => b!=0).ToString();
            return Sol1;
        }
        private string GetCurrentCommand(string instruction)
        {
            string pattern = @"^(.*?) (\d+,\d+) through (\d+,\d+)$";
            Match match = Regex.Match(instruction, pattern);
            var command = match.Groups[1].Value;
            string startCoordinates = match.Groups[2].Value;
            string endCoordinates = match.Groups[3].Value;
            string[] startParts = startCoordinates.Split(',');
            string[] endParts = endCoordinates.Split(',');
            var startX = startParts[0];
            var startY = startParts[1];
            var endX = endParts[0];
            var endY = endParts[1];
            return $"{command},{startX},{startY},{endX},{endY}";
        }
        private int GetBlubState(string command, int currentState)
        {
            var newState = currentState;
            switch (command)
            {
                case "turn off":
                    newState = 0;    
                    break;
                case "turn on":
                    newState = 1;
                    break;
                case "toggle":
                    newState = currentState==1 ? 0:1;
                    break;
            }
            return newState;
        }
        private static int GetBlubState2(string command, int currentState)
        {
            var newState = currentState;
            switch (command)
            {
                case "turn off":
                    newState= currentState-1;
                    break;
                case "turn on":
                    newState= currentState+1;
                    break;
                case "toggle":
                    newState=currentState+2;
                    break;
            }
            if (newState<0)
            {
                newState= 0;
            }
            return newState;
        }
        string IAocTask.Solve2()
        {
            Sol2 = Lights2.Cast<int>().Sum(b => b).ToString();
            return Sol2;
        }
        private static int [,] CreateLights()
        {
            var lights = new int[1000, 1000];
            for (var x = 0; x < 1000; x++)
            {
                for (var y = 0; y < 1000; y++)
                {
                    lights[x,y] = 0;
                }
            }
            return lights;
        }
        public void PrepareData()
        {
            Lights = CreateLights();
            Lights2 = CreateLights();
            Instructions = File.ReadLines(FilePath).ToArray();
        }
    }
}

