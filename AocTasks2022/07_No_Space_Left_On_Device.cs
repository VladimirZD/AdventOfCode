using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;
using System.Diagnostics;
using System.Runtime.ConstrainedExecution;

namespace AdventOfCode.AocTasks2022
{
    [AocTask(2022, 7)]
    public class No_Space_Left_On_Device : IAocTask
    {

        public string[] Input { get; set; }
        public Dictionary<string, int> DirData { get; set; }
        public int DelFolderSize { get; set; }
        public No_Space_Left_On_Device(string filePath)
        {
            Input = File.ReadAllLines(filePath);
            //Input = "$ cd /\n$ ls\ndir a\n14848514 b.txt\n8504156 c.dat\ndir d\r\n$ cd a\n$ ls\ndir e\n29116 f\n2557 g\n62596 h.lst\n$ cd e\n$ ls\n584 i\n$ cd ..\n$ cd ..\n$ cd d\n$ ls\n4060174 j\n8033020 d.log\n5626152 d.ext\n7214296 k".Split("\n").ToArray();
            DirData = new Dictionary<string, int>();
        }
        public void PrepareData()
        {
            var currentPath = "";
            var span = Input.AsSpan();
            foreach (var line in span)
            {
                if (line != "$ ls")
                {
                    if (line[0..4] == "$ cd")
                    {
                        var path = line.Split(' ')[2];
                        if (path == "..")
                        {
                            currentPath = currentPath[0..(currentPath[0..(currentPath.Length - 1)].LastIndexOf("/"))];
                            currentPath = currentPath == "" ? "/" : currentPath;
                        }
                        else if (path == "/")
                        {
                            currentPath = "/";
                        }
                        else
                        {
                            currentPath += (currentPath.EndsWith("/") ? $"{path}" : $"/{path}");
                            
                        }
                        if (!DirData.ContainsKey(currentPath))
                        {
                            DirData.Add(currentPath, 0);
                        }
                    }
                    else if (line[0..3] != "dir")
                    {
                        var fileSize = int.Parse(line.Split(' ')[0]);
                        var pathToUpdate = currentPath;
                        while (!string.IsNullOrEmpty(pathToUpdate))
                        {
                            DirData[pathToUpdate] = DirData[pathToUpdate] += fileSize;
                            pathToUpdate = (pathToUpdate == "/") ? "" : pathToUpdate[0..(pathToUpdate[0..(pathToUpdate.Length - 1)].LastIndexOf("/"))];
                            if (pathToUpdate == "" && currentPath != "/")
                            {
                                DirData["/"] = DirData["/"] += fileSize;
                            }
                        }
                    }
                }

            }
        }
        string IAocTask.Solve1()
        {
            const int SIZE_LIMIT = 100000;
            const int DISK_SIZE = 70000000;
            const int TARGET_FREE = 30000000;

            var totalSpaceUsed = DirData["/"];
            var needToDelete = TARGET_FREE - DISK_SIZE + totalSpaceUsed;

            var retValue = 0;
            DelFolderSize = int.MaxValue;
            foreach (var item in DirData)
            {
                retValue += item.Value <= SIZE_LIMIT ? item.Value : 0;
                if (item.Value >= needToDelete && item.Value < DelFolderSize)
                {
                    DelFolderSize = item.Value;
                }
            }
            Debug.Assert(retValue == 1644735);
            return retValue.ToString();
        }
        string IAocTask.Solve2()
        {
            Debug.Assert(DelFolderSize == 1300850);
            return DelFolderSize.ToString();
        }
    }
}

