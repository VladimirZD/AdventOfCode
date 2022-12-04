namespace AdventOfCode.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class AocTask : Attribute
    {
        public int Year { get; set; }
        public int Day { get; set; }
        public string Description { get; set; }
        public AocTask(int year, int day)
        {
            Year = year;
            Day = day;
        }
        public AocTask(int year, int day, string descirption)
        {
            Year = year;
            Day = day;
            Description = descirption;
        }
    }
}
