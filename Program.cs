using AdventOfCode.AocTasks2022;
using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using static AdventOfCode.SessionExtractor;

namespace AdventOfCode
{
    internal class Program
    {
        private const string AOC_WEB_BASE_URL = "https://adventofcode.com/";
        static void Main(string[] args)
        {

            var tasks = GetAocTasks();
            Console.WriteLine($"Found {tasks.Count} Aoc Tasks");
            CookieData  cookieData = GetCookieFromFile();
            if (ValidateCookie(cookieData))
            {
                CreateTaskDataFolder();
                DownloadTaskData(tasks, cookieData);
            }
            else
            {
                Console.WriteLine($"Session cookie is invalid. Can't download task imput. Will skip tasks with no input.");
            }
            RunTasks(tasks);
        }
        private static CookieData GetCookieFromFile()
        {
            var cookieData = new CookieData();
            var cookieFilePath = Path.Combine(AppContext.BaseDirectory, "CookieData.txt");
            //cookieData.Value = File.ReadAllText($"{AppContext.BaseDirectory}\\CookieData.txt");
            cookieData.Value = File.ReadAllText(cookieFilePath);
            return cookieData;
        }
        private static void RunTasks(List<Type> tasks)
        {

            foreach (var task in tasks)
            {
                var aocTaskAttribute = task.GetCustomAttribute<AocTask>();
                if (aocTaskAttribute != null)
                {
                    string filePath = $@"{AppContext.BaseDirectory}TaskData\{aocTaskAttribute.Year}_{aocTaskAttribute.Day}.txt";
                    //DoWarmUp(task, aocTaskAttribute, filePath);
                    //var runTimes = new Dictionary<string, List<TimeSpan>>
                    //{
                    //    ["DataPreparation"] = new List<TimeSpan>(),
                    //    ["Result1"] = new List<TimeSpan>(),
                    //    ["Result2"] = new List<TimeSpan>(),
                    //    ["Total"] = new List<TimeSpan>()
                    //};
                    if (File.Exists(filePath))
                    {
                        DoFinalRun(task, aocTaskAttribute, filePath);
                    }
                    else
                    {
                        Console.WriteLine($"Task input file for task {aocTaskAttribute.Year}/{aocTaskAttribute.Day} not found, skipping task");
                    }
                    //Console.WriteLine($"\tData preparation done in {GetFormatedElapsed(dataPreparationTime)} ");
                    //Console.WriteLine($"\tSolve1 result is {result1} done in {GetFormatedElapsed(result1Time)}");
                    //Console.WriteLine($"\tSolve2 result is {result2} done in {GetFormatedElapsed(stopWatch.Elapsed - result1Time - dataPreparationTime)}");
                    //Console.WriteLine($"\tTotal time {stopWatch.Elapsed} ({GetFormatedElapsed(stopWatch.Elapsed)})");
                    //var r = runTimes["DataPreparation"].Max();
                    //var r2 = runTimes["DataPreparation"].Average<TimeSpan>(i => i.);
                }

            }
        }
        private static void DoFinalRun(Type task, AocTask? aocTaskAttribute, string filePath)
        {
            Console.WriteLine($"Executing {task.Name}({aocTaskAttribute?.Year}/{aocTaskAttribute?.Day})");
            for (var i = 1; i < 2; i++)
            {
                var instance = (Activator.CreateInstance(task, filePath) as IAocTask);
                if (instance != null)
                {

                    Stopwatch stopWatch = Stopwatch.StartNew();
                    instance.PrepareData();
                    var dataPreparationTime = stopWatch.Elapsed;

                    var result1 = instance.Solve1();
                    var result1Time = stopWatch.Elapsed - dataPreparationTime;
                    var result2 = instance.Solve2();
                    stopWatch.Stop();

                    //runTimes["DataPreparation"].Add(dataPreparationTime);
                    //runTimes["Result1"].Add(result1Time);
                    //runTimes["Result2"].Add(stopWatch.Elapsed - result1Time - dataPreparationTime);
                    //runTimes["Total"].Add(stopWatch.Elapsed);

                    Console.WriteLine($"\tData preparation done in {GetFormatedElapsed(dataPreparationTime)} ");
                    Console.WriteLine($"\tSolve1 result is {result1} done in {GetFormatedElapsed(result1Time)}");
                    Console.WriteLine($"\tSolve2 result is {result2} done in {GetFormatedElapsed(stopWatch.Elapsed - result1Time - dataPreparationTime)}");
                    Console.WriteLine($"\tTotal time {stopWatch.Elapsed} ({GetFormatedElapsed(stopWatch.Elapsed)})");
                }
            }
        }
        private static void DoWarmUp(Type task, AocTask? aocTaskAttribute, string filePath)
        {
            var warmUprounds = 20;
            for (var i = 1; i <= warmUprounds; i++)
            {
                Console.WriteLine($"Warming up for {task.Name}({aocTaskAttribute?.Year}/{aocTaskAttribute?.Day}) round {i}/{warmUprounds}");
                Console.CursorTop--;
                var instance = (Activator.CreateInstance(task, filePath) as IAocTask);
                if (instance != null)
                {
                    instance.PrepareData();
                    _ = instance.Solve1();
                    _ = instance.Solve2();

                }
            }
            Console.CursorTop++;
        }
        private static string GetFormatedElapsed(TimeSpan time)
        {
            var formatedValue = $"{time.TotalMicroseconds} µs";
            if (time.TotalMicroseconds>=1000)
            {
                formatedValue = $"{time.TotalMilliseconds} ms";
            }
            return $"{formatedValue}";
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
            return types.OrderBy(t => t.GetCustomAttribute<AocTask>()?.Year).ThenBy(t => t.GetCustomAttribute<AocTask>()?.Day).ToList();
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
        private static bool ValidateCookie(CookieData cookieData)
        {
            var url = $"{AOC_WEB_BASE_URL}";
            var downloadTask = GenerateTaskDataFile(url, cookieData);
            downloadTask.Wait();
            var textData = downloadTask.Result;
            var isCookieValid = !textData.Contains("[Log In]");
            return isCookieValid;
        }
        private static void DownloadTaskData(List<System.Type> tasks, CookieData cookieData)
        {
            Console.WriteLine($@"Downloading task data to folder {AppContext.BaseDirectory}TaskData");
            foreach (var task in tasks)
            {
                var aocTaskAttribute = task.GetCustomAttribute<AocTask>();
                Console.Write($"\tDownloading task data: {task.Name}...");
                if (aocTaskAttribute != null)
                {
                    var fileName = $"{aocTaskAttribute.Year}_{aocTaskAttribute.Day}.txt";
                    string filePath = $@"{AppContext.BaseDirectory}TaskData\{fileName}";
                    string msg = $"File {fileName} exist, skipping download";
                    if (!File.Exists(filePath))
                    {
                        var url = $"{AOC_WEB_BASE_URL}{aocTaskAttribute.Year}/day/{aocTaskAttribute.Day}/input";
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