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
        private List<ThingDef> prioritizedThingDefs;

        public MapComponent_PrioritizedHauling(Map map) : base(map)
        {
            this.knownThingDefs = new HashSet<ThingDef>();
            this.prioritizedThingDefs = new List<ThingDef>();
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
            foreach (ThingDef def in prioritizedThingDefs)
            {
                List<Thing> prioritySet = new List<Thing>();
                foreach (Thing thing in everything)
                {
                    if (thing.def == def)
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

        public List<ThingDef> getPrioritizedThingDefs()
        {
            return this.prioritizedThingDefs;
        }

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
            this.prioritizedThingDefs.Remove(td);
            this.prioritizedThingDefs.Add(td);
        }

        public void removePriorityThingDef(ThingDef td)
        {
            this.prioritizedThingDefs.Remove(td);
        }

        public void incrementPriorityThingDef(ThingDef td)
        {
            int oldIndex = prioritizedThingDefs.IndexOf(td);
            if (oldIndex > 0)
            {
                this.prioritizedThingDefs.Remove(td);
                this.prioritizedThingDefs.Insert(oldIndex - 1, td);
            }
        }

        public void decrementPriorityThingDef(ThingDef td)
        {
            int oldIndex = prioritizedThingDefs.IndexOf(td);
            if (oldIndex < prioritizedThingDefs.Count - 1)
            {
                this.prioritizedThingDefs.Remove(td);
                this.prioritizedThingDefs.Insert(oldIndex + 1, td);
            }
        }

        public void clearPrioritizedThingDefs()
        {
            this.prioritizedThingDefs = new List<ThingDef>();
        }

    }
}
