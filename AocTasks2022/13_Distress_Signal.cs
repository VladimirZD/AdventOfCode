using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;
using System.Diagnostics;
using System.Text;

namespace AdventOfCode.AocTasks2022
{
    [AocTask(2022, 13)]
    public class Distress_Signal : IAocTask
    {
        record struct ParseResult(string value, int position);
        private string[] Input { get; set; }
        private List<Packet> Packets { get; set; }
        private int Result1 { get; set; }
        private int Result2 { get; set; }

        private class Packet
        {
            public List<Packet> Elements { get; set; }
            public int? Value { get; set; }
            public Packet()
            {
                Elements = new();
            }
            //public override string ToString()
            //{
            //    StringBuilder sb = new("[");
            //    for (int i = 0; i < Elements.Count; i++)
            //    {
            //        if (Elements[i].Elements.Count == 0)
            //        {
            //            sb.Append(Elements[i].Value).Append(',');
            //        }
            //        else
            //        {
            //            sb.Append(Elements[i].ToString()).Append(',');
            //        }
            //    }
            //    if (sb.Length > 1) { sb.Length--; }
            //    return sb.Append(']').ToString();
            //}
        }
        public Distress_Signal(string filePath)
        {
            Input = File.ReadAllText(filePath).Split("\n\n");
            //Input = "[1,1,3,1,1]\r\n[1,1,5,1,1]\r\n\r\n[[1],[2,3,4]]\r\n[[1],4]\r\n\r\n[9]\r\n[[8,7,6]]\r\n\r\n[[4,4],4,4]\r\n[[4,4],4,4,4]\r\n\r\n[7,7,7,7]\r\n[7,7,7]\r\n\r\n[]\r\n[3]\r\n\r\n[[[]]]\r\n[[]]\r\n\r\n[1,[2,[3,[4,[5,6,7]]]],8,9]\r\n[1,[2,[3,[4,[5,6,0]]]],8,9]".Replace("\r\n", "\n").Split("\n\n").ToArray();
        }
        public void PrepareData()
        {
            Packets = new List<Packet>();
            foreach (var row in Input)
            {
                var rowData = row.Split("\n");
                var left = GetPacketFromString(rowData[0]);
                var right = GetPacketFromString(rowData[1]);
                Packets.Add(left);
                Packets.Add(right);
            }
        }
        private static Packet GetPacketFromString(string packetData)
        {
            int start = 1;
            int currentPos = 1;
            return CreatePacket(packetData, ref start, ref currentPos);
        }
        private static Packet CreatePacket(string packet, ref int start, ref int index)
        {
            var item = new Packet();
            while (index < packet.Length)
            {
                char current = packet[index];
                index++;
                switch (current)
                {
                    case '[':
                        start = index;
                        var newElement = CreatePacket(packet, ref start, ref index);
                        item.Elements.Add(newElement);
                        break;
                    case ']':
                    case ',':
                        if (start < index - 1)
                        {
                            newElement = new Packet() { Value = int.Parse(packet[start..(index - 1)]) };
                            item.Elements.Add(newElement);
                        }
                        start = index;
                        if (current == ']')
                        {
                            return item;
                        }
                        break;
                }
            }
            return item;
        }
        private int CompareElements(Packet left, Packet right)
        {
            for (int i = 0; i < left.Elements.Count; i++)
            {
                var rightCnt = right.Elements.Count;
                var leftElement = left.Elements[i];
                if (i >= rightCnt)
                {
                    return 1;
                }
                var rightElement = right.Elements[i];
                var leftValue = leftElement.Value;
                var rightValue = rightElement.Value;
                if (leftValue != null && rightValue != null)
                {
                    int valueComp = leftValue.Value.CompareTo(rightValue.Value);
                    if (valueComp != 0) { return valueComp; }
                    continue;
                }
                else if (leftValue != null)
                {
                    leftElement = new Packet() { Elements = new() };
                    leftElement.Elements.Add(new Packet() { Value = leftValue });
                }
                else if (rightValue != null)
                {
                    rightElement = new Packet() { Elements = new() };
                    rightElement.Elements.Add(new Packet() { Value = rightValue });
                }

                int result = CompareElements(leftElement, rightElement);
                if (result != 0)
                {
                    return result;
                }
            }
            return left.Elements.Count.CompareTo(right.Elements.Count);
        }
        string IAocTask.Solve1()
        {
            for (int i = 0; i < Packets.Count; i += 2)
            {
                Packet left = Packets[i];
                Packet right = Packets[i + 1];
                if (CompareElements(left, right) < 0)
                {
                    Result1 += (i / 2 + 1);
                }
            }
            Debug.Assert(Result1 == 5252);
            return Result1.ToString();
        }
        string IAocTask.Solve2()
        {
            var packet1 = GetPacketFromString("[[2]]");
            var packet2 = GetPacketFromString("[[6]]");
            Packets.Add(packet1);
            Packets.Add(packet2);
            Packets.Sort((left, right) => CompareElements(left, right));

            Debug.Assert(Result1 == 5252);
            Result2 = (Packets.IndexOf(packet1) + 1) * (Packets.IndexOf(packet2) + 1);
            return Result2.ToString();
        }
    }
}
