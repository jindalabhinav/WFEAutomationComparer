using System;
using System.IO;
using System.Text.Json;

namespace WFEAutomationComparer // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var sideFilesPath = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, "Selenium Automations");
            var sideFiles = Directory.GetFiles(sideFilesPath, "DenverTest*");

            var sideFilesTargets = GetSidesTargets(sideFiles);
            var commonTargets = GetCommonTargets(@"D:\POCs\WFEAutomationComparer\WFEAutomationComparer\Selenium Automations\DenverTest.side", sideFilesTargets);
            PrintCommonTargets(commonTargets);

            UpdateOriginalSidesWithCommonalityInfo(commonTargets, sideFiles);
        }

        private static void UpdateOriginalSidesWithCommonalityInfo(HashSet<string> commonTargets, string[] sideFiles)
        {
            foreach (var sideFile in sideFiles)
            {

            }
            //var data = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Selenium Automations", sideFile));
        }

        private static void PrintCommonTargets(HashSet<string> commonTargets)
        {
            foreach (var target in commonTargets)
            {
                Console.WriteLine(target);
            }
        }

        private static HashSet<string> GetCommonTargets(string masterFile, Dictionary<string, HashSet<string>> sideFilesTargets)
        {
            var intersectionResult = sideFilesTargets[masterFile];
            foreach (var sideFiletargets in sideFilesTargets)
            {
                intersectionResult.Intersect(sideFiletargets.Value);
            }

            return intersectionResult;
        }

        private static Dictionary<string, HashSet<string>> GetSidesTargets(string[] sideFiles)
        {

            var sideFilesTargets = new Dictionary<string, HashSet<string>>();
            foreach (var sideFile in sideFiles)
            {

                var data = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Selenium Automations", sideFile));
                var sideFileData = JsonSerializer.Deserialize<SideFileOriginalData>(data);
                sideFilesTargets.Add(sideFile, sideFileData?.tests[0]?.commands?.Select(x => x.target)?.ToHashSet());
            }

            return sideFilesTargets;
        }
    }
}