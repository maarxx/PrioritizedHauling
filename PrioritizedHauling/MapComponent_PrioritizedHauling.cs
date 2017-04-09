using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace PrioritizedHauling
{
    class MapComponent_PrioritizedHauling : MapComponent
    {

        private HashSet<ThingDef> knownThingDefs;
        private HashSet<ThingCategoryDef> knownThingCategoryDefs;
        private List<Priority> priorities;

        public MapComponent_PrioritizedHauling(Map map) : base(map)
        {
            this.knownThingDefs = new HashSet<ThingDef>();
            this.knownThingCategoryDefs = new HashSet<ThingCategoryDef>();
            this.priorities = new List<Priority>();
            LongEventHandler.QueueLongEvent(ensureComponentExists, null, false, null);
        }

        public static void ensureComponentExists()
        {
            foreach (Map m in Find.Maps)
            {
                if (m.GetComponent<MapComponent_PrioritizedHauling>() == null)
                {
                    m.components.Add(new MapComponent_PrioritizedHauling(m));
                }
            }
        }

        private IEnumerable<Thing> everythingPotentiallyNeedingHauling()
        {
            return map.listerHaulables.ThingsPotentiallyNeedingHauling();
        }

        internal IEnumerable<Thing> ThingsPotentiallyNeedingHauling()
        {
            List<List<Thing>> llt = getAllPrioritySets();
            List<Thing> condensed = new List<Thing>();
            foreach (List<Thing> lt in llt)
            {
                foreach (Thing t in lt)
                {
                    condensed.Add(t);
                }
            }
            return condensed;
        }

        internal List<List<Thing>> getAllPrioritySets()
        {
            IEnumerable<Thing> everything = everythingPotentiallyNeedingHauling();
            List<List<Thing>> allPrioritySets = new List<List<Thing>>();
            foreach (Priority priority in priorities)
            {
                List<Thing> prioritySet = new List<Thing>();
                foreach (Thing thing in everything)
                {
                    if (priority.thingDef != null && priority.thingDef == thing.def)
                    {
                        prioritySet.Add(thing);
                    }
                    if (priority.thingCategoryDef != null && thing.def.thingCategories.Contains(priority.thingCategoryDef))
                    {
                        prioritySet.Add(thing);
                    }
                }
                if (prioritySet.Count > 0)
                {
                    allPrioritySets.Add(prioritySet);
                }
            }
            return allPrioritySets;
        }

        public List<ThingDef> getKnownThingDefs()
        {
            foreach (Thing t in this.everythingPotentiallyNeedingHauling())
            {
                this.knownThingDefs.Add(t.def);
            }
            return this.knownThingDefs.OrderBy(o => o.label).ToList();
        }

        public List<ThingCategoryDef> getKnownThingCategoryDefs()
        {
            foreach (ThingDef td in getKnownThingDefs())
            {
                foreach (ThingCategoryDef tcd in td.thingCategories)
                {
                    this.knownThingCategoryDefs.Add(tcd);
                }
            }
            return this.knownThingCategoryDefs.OrderBy(o => o.label).ToList();
        }

        public List<Priority> getPriorities()
        {
            return this.priorities;
        }

        /*
        public void printThingCategoryDefs()
        {
            Dictionary<ThingCategoryDef, int> tallies = new Dictionary<ThingCategoryDef, int>();
            foreach (ThingDef t in getKnownThingDefs())
            {
                foreach (ThingCategoryDef tcd in t.thingCategories)
                {
                    if (tallies.ContainsKey(tcd))
                    {
                        tallies[tcd]++;
                    }
                    else
                    {
                        tallies.Add(tcd, 1);
                    }
                }
            }
            foreach (KeyValuePair<ThingCategoryDef, int> kvp in tallies)
            {
                Log.Message("" + kvp.Key.label + ", " + kvp.Value);
            }
        }
        */

        /*
        public void setPriorityThingDef(ThingDef td, int priority)
        {
            //if (this.prioritizedThingDefs.Contains(td))
            //{
                this.prioritizedThingDefs.Remove(td);
            //}
            this.prioritizedThingDefs.Insert(priority, td);
        }
        */

        public void addPriorityThingDef(ThingDef td)
        {
            Priority temp = new Priority();
            temp.thingDef = td;
            this.priorities.Remove(temp);
            this.priorities.Add(temp);
        }

        public void addPriorityThingCategoryDef(ThingCategoryDef tcd)
        {
            Priority temp = new Priority();
            temp.thingCategoryDef = tcd;
            this.priorities.Remove(temp);
            this.priorities.Add(temp);
        }

        public void removePriority(Priority p)
        {
            this.priorities.Remove(p);
        }

        public void incrementPriority(Priority p)
        {
            int oldIndex = priorities.IndexOf(p);
            if (oldIndex > 0)
            {
                this.priorities.Remove(p);
                this.priorities.Insert(oldIndex - 1, p);
            }
        }

        public void decrementPriority(Priority p)
        {
            int oldIndex = priorities.IndexOf(p);
            if (oldIndex < priorities.Count - 1)
            {
                this.priorities.Remove(p);
                this.priorities.Insert(oldIndex + 1, p);
            }
        }

        public void clearPrioritizedThingDefs()
        {
            this.priorities = new List<Priority>();
        }

    }
}
