using AdventOfCode.AocTasks2023;
using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using static AdventOfCode.SessionExtractor;
using BenchmarkDotNet.Running;
using CommandLine;


namespace AdventOfCode
{
    [SimpleJob(RuntimeMoniker.Net80)]
    public class Program
    {
        private const string AOC_WEB_BASE_URL = "https://adventofcode.com/";
        public int Year { get; set; }
        public int Day { get; set; }
        static void Main(string[] args)
        {

            //var summary = BenchmarkRunner.Run<Program>();
            //return;
            var cmdLineOptions = ParseCmdLine(args);
            List<Type> tasks = GetAocTasks(cmdLineOptions);
            Console.WriteLine($"Found {tasks.Count} Aoc Tasks");
            CookieData cookieData = GetCookieFromFile();
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

        private static List<Type> GetAocTasks(CmdLineOptions cmdLineOptions)
        {
            return GetAocTasks()
                            .Where(t =>
                            (cmdLineOptions.Year == 0 || t.GetCustomAttribute<AocTask>()?.Year == cmdLineOptions.Year) &&
                            (cmdLineOptions.Day == 0 || t.GetCustomAttribute<AocTask>()?.Day == cmdLineOptions.Day))
                            .ToList();
        }

        private static CmdLineOptions ParseCmdLine(string[] args)
        {
            var result = Parser.Default.ParseArguments<CmdLineOptions>(args);
            result.WithNotParsed(errs =>
            {
                Console.WriteLine($"Failed with errors:\n\t{String.Join("\n\t", errs)}");
                Environment.Exit(1);
            });
            return result.Value;
        }

        private static CookieData GetCookieFromFile()
        {
            var cookieData = new CookieData();
            var cookieFilePath = Path.Combine(AppContext.BaseDirectory, "CookieData.txt");
            cookieData.Value = File.ReadAllText(cookieFilePath);
            return cookieData;
        }

        [Benchmark()]
        public void DoTheBenchmark()
        {
            string filePath = Path.Combine(Program.GetTasksFolder(), "2023_7.txt");
            var solver = (IAocTask)new Camel_Cards(filePath);
            solver.PrepareData();
            _ = solver.Solve1();
            _ = solver.Solve2();
        }
        private static void RunTasks(List<Type> tasks)
        {
            foreach (var task in tasks)
            {
                var aocTaskAttribute = task.GetCustomAttribute<AocTask>();
                if (aocTaskAttribute != null)
                {
                    string filePath = Path.Combine(GetTasksFolder(), $"{aocTaskAttribute.Year}_{aocTaskAttribute.Day}.txt");
                    if (File.Exists(filePath))
                    {
                        DoFinalRun(task, aocTaskAttribute, filePath);
                    }
                    else
                    {
                        Console.WriteLine($"Task input file for task {aocTaskAttribute.Year}/{aocTaskAttribute.Day} not found, skipping task");
                    }
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

                    Console.WriteLine($"\tData preparation done in {GetFormatedElapsed(dataPreparationTime)} ");
                    Console.WriteLine($"\tSolve1 result is {result1} done in {GetFormatedElapsed(result1Time)}");
                    Console.WriteLine($"\tSolve2 result is {result2} done in {GetFormatedElapsed(stopWatch.Elapsed - result1Time - dataPreparationTime)}");
                    Console.WriteLine($"\tTotal time {stopWatch.Elapsed} ({GetFormatedElapsed(stopWatch.Elapsed)})");
                }
            }
        }
        private static string GetFormatedElapsed(TimeSpan time)
        {
            var formatedValue = $"{time.TotalMicroseconds} µs";
            if (time.TotalMicroseconds >= 1000)
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
            return [.. types.OrderBy(t => t.GetCustomAttribute<AocTask>()?.Year).ThenBy(t => t.GetCustomAttribute<AocTask>()?.Day)];
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
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception($"Error running task. HTTP Status: {response.StatusCode},error: {response.ReasonPhrase}");
                }
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
            Console.WriteLine($@"Downloading task data to folder {GetTasksFolder()}");
            Directory.CreateDirectory(GetTasksFolder());
            foreach (var task in tasks)
            {
                var aocTaskAttribute = task.GetCustomAttribute<AocTask>();
                Console.Write($"\tDownloading task data: {task.Name}...");
                if (aocTaskAttribute != null)
                {

                    var fileName = $"{aocTaskAttribute.Year}_{aocTaskAttribute.Day}.txt";
                    string filePath = Path.Combine(GetTasksFolder(), fileName);
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
        private static string GetTasksFolder()
        {
            return $@"{Path.GetTempPath()}AocComplete\TaskData\";
        }
    }
}
