using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Verse;

namespace PrioritizedHauling
{
    class SaveLoad_PrioritizedHauling
    {

        // Took some notes from JTExport on how to do basic file manipulation in C# / in a RimWorld mod. Thanks JamesTec!

        public readonly static DirectoryInfo saveBase = new DirectoryInfo(Path.Combine(GenFilePaths.SaveDataFolderPath, "PrioritizedHauling"));
        public readonly static DirectoryInfo savePriorities = new DirectoryInfo(Path.Combine(saveBase.FullName, "PrioritySets"));

        public static void ensureDirectoriesExist()
        {
            if (!saveBase.Exists)
            {
                saveBase.Create();
            }
            if (!savePriorities.Exists)
            {
                savePriorities.Create();
            }
        }

        public static List<string> getAllFiles()
        {
            List<string> names = new List<string>();
            FileInfo[] files = savePriorities.GetFiles("*.txt", SearchOption.TopDirectoryOnly);
            foreach (FileInfo file in files)
            {
                string fileName = file.Name;
                if (fileName.EndsWith(".txt"))
                {
                    string name = fileName.Substring(0, fileName.Length - 4);
                    names.Add(name);
                }
            }
            return names;
        }

        public static bool exportPriorities(string name, List<Priority> priorities)
        {

            ensureDirectoriesExist();

            List<string> data = new List<string>();

            //data.Add("Name=" + name);

            foreach (Priority priority in priorities)
            {
                if (priority.thingDef != null)
                {
                    data.Add("ThingDef=" + priority.thingDef.defName);
                }
                else
                {
                    if (priority.thingCategoryDef != null)
                    {
                        data.Add("ThingCategoryDef=" + priority.thingCategoryDef.defName);
                    }
                }
            }

            string finalLocation = Path.Combine(savePriorities.FullName, name + ".txt");
            try
            {
                File.WriteAllLines(finalLocation, data.ToArray());
                return true;
            }
            catch (Exception e)
            {
                Log.Error("PrioritizedHauling: ERROR: " + e.GetType().ToString() + " while trying to save to " + finalLocation);
                return false;
            }

        }

        public static List<Priority> importPriorities(string name)
        {

            ensureDirectoriesExist();

            string finalLocation = Path.Combine(savePriorities.FullName, name + ".txt");

            List<string> data;
            try
            {
                data = File.ReadAllLines(finalLocation).ToList();
            }
            catch (Exception e)
            {
                Log.Error("PrioritizedHauling: ERROR: " + e.GetType().ToString() + " while trying to read from " + finalLocation);
                return null;
            }

            List<Priority> priorities = new List<Priority>();
            foreach (string s in data)
            {
                Priority p = new Priority();
                string[] ele = s.Split('=');
                if (ele[0] == "ThingDef")
                {
                    p.thingDef = ThingDef.Named(ele[1]);
                }
                else
                {
                    if (ele[0] == "ThingCategoryDef")
                    {
                        p.thingCategoryDef = ThingCategoryDef.Named(ele[1]);
                    }
                }
                priorities.Add(p);
            }

            return priorities;

        }

    }
}
