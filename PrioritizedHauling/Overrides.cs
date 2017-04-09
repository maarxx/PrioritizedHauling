using HugsLib.Source.Detour;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Verse;
using Verse.AI;

namespace PrioritizedHauling
{
    static class Overrides
    {
        static MethodInfo _gtgjp = typeof(JobGiver_Work).GetMethod("GiverTryGiveJobPrioritized", BindingFlags.NonPublic | BindingFlags.Instance);
        static MethodInfo _pcuwg = typeof(JobGiver_Work).GetMethod("PawnCanUseWorkGiver", BindingFlags.NonPublic | BindingFlags.Instance);

        [DetourMethod(typeof(JobGiver_Work), "TryGiveJob")]
        private static Job TryGiveJob(this JobGiver_Work self, Pawn pawn)
        {
            if (self.emergency && pawn.mindState.priorityWork.IsPrioritized)
            {
                List<WorkGiverDef> workGiversByPriority = pawn.mindState.priorityWork.WorkType.workGiversByPriority;
                for (int i = 0; i < workGiversByPriority.Count; i++)
                {
                    WorkGiver worker = workGiversByPriority[i].Worker;
                    //Job job = self.GiverTryGiveJobPrioritized(pawn, worker, pawn.mindState.priorityWork.Cell);
                    Job job = (Job) _gtgjp.Invoke(self, new object[] { pawn, worker, pawn.mindState.priorityWork.Cell });
                    if (job != null)
                    {
                        job.playerForced = true;
                        return job;
                    }
                }
                pawn.mindState.priorityWork.Clear();
            }
            List<WorkGiver> list = self.emergency ? pawn.workSettings.WorkGiversInOrderEmergency : pawn.workSettings.WorkGiversInOrderNormal;
            int num = -999;
            TargetInfo targetInfo = TargetInfo.Invalid;
            WorkGiver_Scanner workGiver_Scanner = null;
            for (int j = 0; j < list.Count; j++)
            {
                WorkGiver workGiver = list[j];
                if (workGiver.def.priorityInType != num && targetInfo.IsValid)
                {
                    break;
                }
                //if (self.PawnCanUseWorkGiver(pawn, workGiver))
                if ((bool) _pcuwg.Invoke(self, new object[] { pawn, workGiver }))
                {
                    try
                    {
                        Job job2 = workGiver.NonScanJob(pawn);
                        if (job2 != null)
                        {
                            return job2;
                        }
                        WorkGiver_Scanner scanner = workGiver as WorkGiver_Scanner;
                        if (scanner != null)
                        {
                            if (workGiver.def.scanThings)
                            {
                                Predicate<Thing> predicate = (Thing t) => !t.IsForbidden(pawn) && scanner.HasJobOnThing(pawn, t);
                                IEnumerable<Thing> enumerable = scanner.PotentialWorkThingsGlobal(pawn);
                                Thing thing;
                                if (scanner.Prioritized)
                                {
                                    IEnumerable<Thing> enumerable2 = enumerable;
                                    if (enumerable2 == null)
                                    {
                                        enumerable2 = pawn.Map.listerThings.ThingsMatching(scanner.PotentialWorkThingRequest);
                                    }
                                    Predicate<Thing> validator = predicate;
                                    if (workGiver is WorkGiver_PrioritizedHauling)
                                    {
                                        //Log.Message("Hello from JobGiver_Work.TryGiveJob for WorkGiver_PrioritizedHauling for scanner.Prioritized");
                                        WorkGiver_PrioritizedHauling wg_ph = (WorkGiver_PrioritizedHauling)workGiver;
                                        thing = null;
                                        foreach (List<Thing> prioritySet in wg_ph.getAllPrioritySets(pawn))
                                        {
                                            thing = GenClosest.ClosestThing_Global_Reachable(pawn.Position, pawn.Map, prioritySet, scanner.PathEndMode, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, validator, (Thing x) => scanner.GetPriority(pawn, x));
                                            if (thing != null)
                                            {
                                                break;
                                            }
                                        }
                                        
                                    }
                                    else
                                    {
                                        thing = GenClosest.ClosestThing_Global_Reachable(pawn.Position, pawn.Map, enumerable2, scanner.PathEndMode, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, validator, (Thing x) => scanner.GetPriority(pawn, x));
                                    }
                                }
                                else
                                {
                                    Predicate<Thing> validator = predicate;
                                    if (workGiver is WorkGiver_PrioritizedHauling)
                                    {
                                        //Log.Message("Hello from JobGiver_Work.TryGiveJob for WorkGiver_PrioritizedHauling for !scanner.Prioritized");
                                        WorkGiver_PrioritizedHauling wg_ph = (WorkGiver_PrioritizedHauling)workGiver;
                                        thing = null;
                                        foreach (List<Thing> prioritySet in wg_ph.getAllPrioritySets(pawn))
                                        {
                                            thing = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, scanner.PotentialWorkThingRequest, scanner.PathEndMode, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, validator, prioritySet, scanner.LocalRegionsToScanFirst, prioritySet != null);
                                            if (thing != null)
                                            {
                                                break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        thing = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, scanner.PotentialWorkThingRequest, scanner.PathEndMode, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, validator, enumerable, scanner.LocalRegionsToScanFirst, enumerable != null);
                                    }
                                }
                                if (thing != null)
                                {
                                    targetInfo = thing;
                                    workGiver_Scanner = scanner;
                                }
                            }
                            if (workGiver.def.scanCells)
                            {
                                IntVec3 position = pawn.Position;
                                float num2 = 99999f;
                                float num3 = -3.40282347E+38f;
                                bool prioritized = scanner.Prioritized;
                                foreach (IntVec3 current in scanner.PotentialWorkCellsGlobal(pawn))
                                {
                                    bool flag = false;
                                    float lengthHorizontalSquared = (current - position).LengthHorizontalSquared;
                                    if (prioritized)
                                    {
                                        if (!current.IsForbidden(pawn) && scanner.HasJobOnCell(pawn, current))
                                        {
                                            float priority = scanner.GetPriority(pawn, current);
                                            if (priority > num3 || (priority == num3 && lengthHorizontalSquared < num2))
                                            {
                                                flag = true;
                                                num3 = priority;
                                            }
                                        }
                                    }
                                    else if (lengthHorizontalSquared < num2 && !current.IsForbidden(pawn) && scanner.HasJobOnCell(pawn, current))
                                    {
                                        flag = true;
                                    }
                                    if (flag)
                                    {
                                        targetInfo = new TargetInfo(current, pawn.Map, false);
                                        workGiver_Scanner = scanner;
                                        num2 = lengthHorizontalSquared;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error(string.Concat(new object[]
                        {
                            pawn,
                            " threw exception in WorkGiver ",
                            workGiver.def.defName,
                            ": ",
                            ex.ToString()
                        }));
                    }
                    finally
                    {
                    }
                    if (targetInfo.IsValid)
                    {
                        pawn.mindState.lastGivenWorkType = workGiver.def.workType;
                        Job job3;
                        if (targetInfo.HasThing)
                        {
                            job3 = workGiver_Scanner.JobOnThing(pawn, targetInfo.Thing);
                        }
                        else
                        {
                            job3 = workGiver_Scanner.JobOnCell(pawn, targetInfo.Cell);
                        }
                        if (job3 != null)
                        {
                            return job3;
                        }
                        Log.ErrorOnce(string.Concat(new object[]
                        {
                            workGiver_Scanner,
                            " provided target ",
                            targetInfo,
                            " but yielded no actual job for pawn ",
                            pawn,
                            ". The CanGiveJob and JobOnX methods may not be synchronized."
                        }), 6112651);
                    }
                    num = workGiver.def.priorityInType;
                }
            }
            return null;
        }
    }
}
