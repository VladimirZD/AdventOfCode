using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;

namespace AdventOfCode.AocTasks2022
{
    [AocTask(2016, 4)]
    public class Security_Through_Obscurity : IAocTask
    {
        private int Sol1;
        private int Sol2;
        public string FilePath { get; set; }
        public string[] Input { get; set; }
        public List<string> ValidRooms { get; set; }

        public Security_Through_Obscurity(string filePath)
        {
            FilePath = filePath;
            Input = File.ReadAllLines(filePath);
        }
        public void PrepareData()
        {

        }
        string IAocTask.Solve1()
        {
            Solve();
            return Sol1.ToString();
        }
        string IAocTask.Solve2()
        {
            Solve2();
            return Sol2.ToString();
        }
        private void Solve()
        {
            var sectorSum = 0;
            ValidRooms = new List<string>();
            foreach (var line in Input)
            {
                var letters = new Dictionary<char, int>();
                var hashStartIndex = line.IndexOf('[');
                var hash = line[(hashStartIndex + 1)..^1];
                var sectorID = int.Parse(line[(hashStartIndex - 3)..hashStartIndex]);
                for (var i = 0; i < line.Length - hash.Length - 5; i++)
                {
                    var item = line[i];
                    if (item != '-')
                    {
                        if (letters.ContainsKey(item))
                        {
                            letters[item]++;
                        }
                        else
                        {
                            letters.Add(item, 1);
                        }
                    }
                }
                var orderedLetters = letters.OrderByDescending(l => l.Value).ThenBy(l => l.Key).Select(i => i.Key).ToArray();
                string expectedHash = new string(orderedLetters)[0..5];
                if (expectedHash == hash)
                {
                    sectorSum += sectorID;
                    ValidRooms.Add(line);
                }
            }
            Sol1 = sectorSum;
        }
        private void Solve2()
        {
            var decryptedRooms = new List<string>();
            Sol2 = 0;

            foreach (var room in ValidRooms)
            {
                var hashStartIndex = room.IndexOf('[');
                var sectorID = int.Parse(room[(hashStartIndex - 3)..hashStartIndex]);
                var roomData = room[0..(hashStartIndex - 4)];
                var decrypted = DecryptRoomName(roomData, sectorID);
                decryptedRooms.Add(decrypted);
                if (decrypted.IndexOf("northpole object storage", 0) != -1)
                {
                    Sol2 = sectorID;
                    break;
                }
            }
        }
        public static string DecryptRoomName(string roomName, int sectorID)
        {
            string decryptedName = "";
            foreach (char c in roomName)
            {
                if (c == '-')
                {
                    decryptedName += " ";
                }
                else if (char.IsLetter(c))
                {
                    char newChar = (char)((c - 'a' + sectorID) % 26 + 'a');
                    decryptedName += newChar.ToString();
                }
                else
                {
                    decryptedName += c.ToString();
                }
            }
            return decryptedName;
        }
    }
}

