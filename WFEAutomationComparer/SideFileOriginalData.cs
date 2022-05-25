namespace WFEAutomationComparer
{
    public class SideFileOriginalData
    {
        public string id { get; set; }
        public string version { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public Test[] tests { get; set; }
        public Suite[] suites { get; set; }
        public string[] urls { get; set; }
        public string[] plugins { get; set; }
    }

    public class Test
    {
        public string id { get; set; }
        public string name { get; set; }
        public CommandArray[] commands { get; set; }
    }

    public class CommandArray
    {
        public string id { get; set; }
        public string comment { get; set; }
        public string command { get; set; }
        public string target { get; set; }
        public string[][] targets { get; set; }
        public string value { get; set; }
    }

    public class Suite
    {
        public string id { get; set; }
        public string name { get; set; }
        public bool persistSession { get; set; }
        public bool parallel { get; set; }
        public int timeout { get; set; }
        public string[] tests { get; set; }
    }

}
