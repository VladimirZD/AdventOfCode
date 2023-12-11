using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Numerics;
using TraceReloggerLib;

namespace AdventOfCode.AocTasks2023
{
    [AocTask(2023, 9)]
    public class Mirage_Maintenance (string filePath) : IAocTask
    {
        public string FilePath { get; set; } = filePath;
        public string Sol1 { get; set; }
        public string Sol2 { get; set; }
        private struct ReportItem { public List<int> Readings; public List<List<int>>  generatedValues;}
        private List<ReportItem> ReportData;
       
        public void PrepareData()
        {
            var textData = File.ReadAllLines(FilePath);
            ReportData=new List<ReportItem>();
            //textData = "0 3 6 9 12 15\n1 3 6 10 15 21\n10 13 16 21 30 45".Split("\n",StringSplitOptions.RemoveEmptyEntries);
            foreach (var row in textData)
            {
                var parts = row.Split(" ",StringSplitOptions.RemoveEmptyEntries);
                var reportItem=new ReportItem();
                reportItem.Readings=new List<int>();
                foreach (var part in parts)
                {
                    reportItem.Readings.Add(int.Parse(part));
                }
                reportItem.generatedValues=new List<List<int>>();
                reportItem.generatedValues.Add(reportItem.Readings);
                ReportData.Add(reportItem);
            }

            foreach(var report in ReportData)
            {
                var generate=true;
                var i=0;
                var values = report.generatedValues[report.generatedValues.Count-1];
                while (generate)
                {
                     values =GenerateNewValues(values);
                     report.generatedValues.Add(values);
                     generate=values.Any(v=>v!=0);
                }
            }
        }
        string IAocTask.Solve1()
        {
            foreach(var report in ReportData)
            {
                ExtrapolateLast(report);
            }
            var sum=ReportData
                    .Where(item => item.generatedValues != null && item.generatedValues.Any())
                    .Sum(item => item.generatedValues.First().Last());
            Sol1=sum.ToString();
            Debug.Assert((Sol1 == "114") || (Sol1 == "1762065988"));
            return Sol1;
        }
        private void ExtrapolateLast(ReportItem report)
        {
            for (var i=report.generatedValues.Count-1;i>=0;i--)
            {
                var values=report.generatedValues[i];
                //Value to left
                var newValue =values[^1];
                //value bellow;
                newValue=newValue+ (i<report.generatedValues.Count-1 ? report.generatedValues[i+1][^1]:0);
                values.Add(newValue);

            }
        }
        private void ExtrapolateFirst(ReportItem report)
        {
            for (var i=report.generatedValues.Count-1;i>=0;i--)
            {
                var values=report.generatedValues[i];
                //First value
                var newValue =values[0];
                //value bellow;
                newValue=newValue-(i<report.generatedValues.Count-1 ? report.generatedValues[i+1][0]:0);
                values.Insert(0,newValue);

            }
        }
        private List<int> GenerateNewValues(List<int> values)
        {
            var retValue = new List<int>();
            for (var i=0;i<values.Count-1;i++)
            {
                retValue.Add(values[i+1]-values[i]);
            }
            return retValue;
        }
        string IAocTask.Solve2()
        {
            foreach(var report in ReportData)
            {
                ExtrapolateFirst(report);
            }
            var sum=ReportData
                    .Where(item => item.generatedValues != null && item.generatedValues.Any())
                    .Sum(item => item.generatedValues.First().First());
            Sol2=sum.ToString();
            Debug.Assert((Sol2 == "2") || (Sol2 == "1066"));
            return Sol2;
        }
     }
}

