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
            var masterFile = Path.Combine(sideFilesPath, "DenverTest.side"); // @"D:\POCs\WFEAutomationComparer\WFEAutomationComparer\Selenium Automations\DenverTest.side";

            var sideFilesTargets = GetSidesTargets(sideFiles);
            var commonTargets = GetCommonTargets(sideFilesTargets);
            PrintCommonTargets(commonTargets);

            UpdateOriginalSidesWithCommonalityInfo(commonTargets, sideFiles, masterFile);
        }

        private static void UpdateOriginalSidesWithCommonalityInfo(HashSet<string> commonTargets, string[] sideFiles, string masterFile)
        {
            foreach (var sideFile in sideFiles)
            {
                // skipped updating the master file to retain all the duplicate functions in that
                if (sideFile.Equals(masterFile))
                    continue;
                UpdateSide(commonTargets, sideFile);
            }
        }

        private static void UpdateSide(HashSet<string> commonTargets, string sideFile)
        {
            bool changed = false;
            var sideFileData = GetSideFileData(sideFile);
            foreach (var command in sideFileData?.tests[0]?.commands)
            {
                if (!string.IsNullOrEmpty(command.target) && commonTargets.Contains(command.target) && !command.comment.EndsWith("--Duplicate"))
                {
                    command.comment += "--Duplicate";
                    changed = true;
                }
            }
            if (changed)
            {
                var changedData = JsonSerializer.Serialize(sideFileData);
                File.WriteAllText(sideFile, changedData);
            }
        }

        private static void PrintCommonTargets(HashSet<string> commonTargets)
        {
            foreach (var target in commonTargets)
            {
                Console.WriteLine(target);
            }
        }

        private static HashSet<string> GetCommonTargets(HashSet<HashSet<string>> sideFilesTargets)
        {
            var intersectionResult = sideFilesTargets.First();
            foreach (var sideFiletargets in sideFilesTargets)
            {
                intersectionResult.Intersect(sideFiletargets);
            }

            return intersectionResult;
        }

        private static HashSet<HashSet<string>> GetSidesTargets(string[] sideFiles)
        {
            var sideFilesTargets = new HashSet<HashSet<string>>();
            foreach (var sideFile in sideFiles)
            {
                SideFileOriginalData? sideFileData = GetSideFileData(sideFile);
                sideFilesTargets.Add(sideFileData?.tests[0]?.commands?.Select(x => x.target)?.ToHashSet());
            }

            return sideFilesTargets;
        }

        private static SideFileOriginalData? GetSideFileData(string sideFile)
        {
            var data = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Selenium Automations", sideFile));
            var sideFileData = JsonSerializer.Deserialize<SideFileOriginalData>(data);
            return sideFileData;
        }
    }
}