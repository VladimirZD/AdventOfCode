using AdventOfCode.AocTasks2023;
using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using static AdventOfCode.SessionExtractor;

namespace AdventOfCode
{
    [SimpleJob(RuntimeMoniker.Net80)]
    public class Program
    {
        private const string AOC_WEB_BASE_URL = "https://adventofcode.com/";
        static void Main(string[] args)
        {

            //var summary = BenchmarkRunner.Run<Program>();
            //return;
            //TODO: ADD cmd line args
            var tasks = GetAocTasks().Where(t=>t.GetCustomAttribute<AocTask>()?.Year==2023).ToList();
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
            string filePath = $@"D:\Development\AdventOfCodeComplete\bin\Release\net8.0\TaskData\2023_2.txt";
            var solver = (IAocTask) new Cube_Conundrum(filePath) ;
            solver.PrepareData();
            _= solver.Solve1();
            _= solver.Solve2();
        }
        private static void RunTasks(List<Type> tasks)
        {
            foreach (var task in tasks)
            {
                var aocTaskAttribute = task.GetCustomAttribute<AocTask>();
                if (aocTaskAttribute != null)
                {
                    string filePath = $@"{AppContext.BaseDirectory}TaskData\{aocTaskAttribute.Year}_{aocTaskAttribute.Day}.txt";
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
