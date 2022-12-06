﻿using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;
using Org.BouncyCastle.Asn1.Cms;
using Org.BouncyCastle.Utilities;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Xml.Linq;

namespace AdventOfCode.AocTasks2022
{
    [AocTask(2022, 6)]
    public class Daay_6_Tuning_Trouble : IAocTask
    {

        public string FilePath { get; set; }
        public string Message { get; set; }
        public int SearchStart { get; set; }
        public Daay_6_Tuning_Trouble(string filePath)
        {
            FilePath = filePath;
            SearchStart = 0;
        }
        public void PrepareData()
        {
            Message = System.IO.File.ReadAllText(FilePath);
        }
        string IAocTask.Solve1()
        {
            var retValue= FindUniquePatternStart(0, 4);
            SearchStart= retValue;
            return retValue.ToString();
        }
        string IAocTask.Solve2()
        {
            return FindUniquePatternStart(SearchStart, 14).ToString();
        }
        private int  FindUniquePatternStart(int searchStart, int patLen)
        {
            var start = 0;
            var msgAsSpan = Message.AsSpan();
            for (var i = searchStart; i < msgAsSpan.Length - patLen; i++)
            {
                var part = Message[i..(i + patLen)];
                start = i + patLen;
                int cnt = 0;
                var usedChars = new HashSet<char>();
                foreach (var letter in part)
                {
                    int n = 0; ;
                    cnt = 0;
                    while ((n = part.IndexOf(letter, n)) != -1 && cnt < 2)
                    {
                        n += 1;
                        cnt++;
                    }
                    if (cnt > 1)
                    {
                        break;
                    }
                }
                if (cnt == 1)
                {
                    break;
                }
            }
            return start;
        }
    }
}