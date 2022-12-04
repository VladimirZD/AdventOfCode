using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using static AdventOfCode.SessionExtractor;

namespace AdventOfCode
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //TODO run all run one run year
            //provide session cookie 
            var tasks = GetAocTasks();
            Console.WriteLine($"Found {tasks.Count} Aoc Tasks");
            CookieData cookieData = new();
            try
            {
                Console.WriteLine("Trying to find AOC session cookie in Chrome");
                cookieData = SessionExtractor.GetAocSessinCookie();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to find session cookie. Can't download task imput. Will skip tasks with no input\nError{ex}");
            }
            CreateTaskDataFolder();
            DownloadTaskData(tasks, cookieData);
            RunTasks(tasks);
        }
        private static void RunTasks(List<Type> tasks)
        {
            Stopwatch stopWatch = new();
            foreach (var task in tasks)
            {
                var aocTaskAttribute = task.GetCustomAttribute<AocTask>();
                if (aocTaskAttribute != null)
                {
                    Console.WriteLine($"Executing {task.Name}({aocTaskAttribute.Year}/{aocTaskAttribute.Day})");
                    string filePath = $@"{AppContext.BaseDirectory}TaskData\{aocTaskAttribute.Year}_{aocTaskAttribute.Day}.txt";
                    stopWatch.Start();
                    var instance = (Activator.CreateInstance(task, filePath) as IAocTask);
                    stopWatch.Stop();
                    Console.WriteLine($"\tInstance creation and data preparation done in {stopWatch.Elapsed}ms");
                    if (instance != null)
                    {
                        stopWatch.Restart();
                        var result1 = instance.Solve1();
                        stopWatch.Stop();
                        Console.WriteLine($"\tSolve1 result is {result1} done in {stopWatch.Elapsed}ms");
                        stopWatch.Restart();
                        var result2 = instance.Solve2();
                        stopWatch.Stop();
                        Console.WriteLine($"\tSolve2 result is {result2} done in {stopWatch.Elapsed}ms");
                    }
                    stopWatch.Stop();
                }
            }
        }
        private static void CreateTaskDataFolder()
        {
            var taskDataPath = Path.Combine(AppContext.BaseDirectory, "TaskData");
            if (!Directory.Exists(taskDataPath))
            {
                Directory.CreateDirectory(taskDataPath);
            }
        }
        static List<System.Type> GetAocTasks()
        {
            var types = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsDefined(typeof(AocTask)));
            return types.OrderBy(t => t.GetCustomAttribute<AocTask>().Year).ThenBy(t => t.GetCustomAttribute<AocTask>().Day).ToList();
        }
        public static async Task<string> GenerateTaskDataFile(string url, CookieData cookieData)
        {

            string result = "";
            var cookieContainer = new CookieContainer();
            cookieContainer.Add(new Cookie(cookieData.CookieName, cookieData.Value, "", cookieData.Domain));
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            using (HttpClient client = new(handler))
            {
                using HttpResponseMessage response = client.GetAsync(url).Result;
                using HttpContent content = response.Content;
                result = content.ReadAsStringAsync().Result;
            }
            return result;
        }
        private static void DownloadTaskData(List<System.Type> tasks, CookieData cookieData)
        {
            Console.WriteLine($@"Downloading task data to folder {AppContext.BaseDirectory}TaskData");
            foreach (var task in tasks)
            {
                var aocTaskAttribute = task.GetCustomAttribute<AocTask>();
                Console.Write($"\tDownloading task data :{aocTaskAttribute.Description}...");
                if (aocTaskAttribute != null)
                {
                    var fileName = $"{aocTaskAttribute.Year}_{aocTaskAttribute.Day}.txt";
                    string filePath = $@"{AppContext.BaseDirectory}TaskData\{fileName}";
                    string msg = $"File {fileName} exist, skipping download";
                    if (!File.Exists(filePath))
                    {
                        var url = $"https://adventofcode.com/{aocTaskAttribute.Year}/day/{aocTaskAttribute.Day}/input";
                        var downloadTask = GenerateTaskDataFile(url, cookieData);
                        downloadTask.Wait();
                        var textData = downloadTask.Result;
                        File.WriteAllText(filePath, textData);
                        msg = $"File {fileName} created";
                    }
                    Console.WriteLine(msg);
                }
            }
        }
    }
}