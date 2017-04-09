using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace PrioritizedHauling
{
    public class WorkGiver_PrioritizedHauling : WorkGiver_HaulGeneral
    {
        public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
        {
            Log.Message("Hello from WorkGiver_PrioritizedHauling.PotentialWorkThingsGlobal");
            return pawn.Map.GetComponent<MapComponent_PrioritizedHauling>().ThingsPotentiallyNeedingHauling();
        }

        internal List<List<Thing>> getAllPrioritySets(Pawn pawn)
        {
            return pawn.Map.GetComponent<MapComponent_PrioritizedHauling>().getAllPrioritySets();
        }
    }
}
