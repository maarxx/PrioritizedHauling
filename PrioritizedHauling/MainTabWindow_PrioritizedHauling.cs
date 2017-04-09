using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace PrioritizedHauling
{

    class MainTabWindow_PrioritizedHauling : MainTabWindow
    {

        private const float BUTTON_HEIGHT = 50f;
        private const float BUTTON_SPACE = 10f;

        public MainTabWindow_PrioritizedHauling()
        {
            //base.forcePause = true;
        }

        public override Vector2 InitialSize
        {
            get
            {
                //return base.InitialSize;
                return new Vector2(250f, 400f);
            }
        }

        public override MainTabWindowAnchor Anchor =>
            MainTabWindowAnchor.Right;

        public override void DoWindowContents(Rect canvas)
        {
            base.DoWindowContents(canvas);

            MapComponent_PrioritizedHauling component = Find.VisibleMap.GetComponent<MapComponent_PrioritizedHauling>();

            List<Priority> priorities = component.getPriorities();

            List<ThingDef> knownThingDefs = component.getKnownThingDefs();
            List<ThingCategoryDef> knownThingCategoryDefs = component.getKnownThingCategoryDefs();

            List<FloatMenuOption> menuRemovePriorities = new List<FloatMenuOption>();
            List<FloatMenuOption> menuIncrementPriorities = new List<FloatMenuOption>();
            List<FloatMenuOption> menuDecrementPriorities = new List<FloatMenuOption>();
            foreach (Priority p in priorities)
            {
                menuRemovePriorities.Add(new FloatMenuOption(p.label, delegate { component.removePriority(p); }));
                menuIncrementPriorities.Add(new FloatMenuOption(p.label, delegate { component.incrementPriority(p); }));
                menuDecrementPriorities.Add(new FloatMenuOption(p.label, delegate { component.decrementPriority(p); }));
            }

            List<FloatMenuOption> menuAddPrioritizableThingDefs = new List<FloatMenuOption>();
            foreach (ThingDef td in knownThingDefs)
            {
                Priority temp = new Priority();
                temp.thingDef = td;
                if (!priorities.Contains(temp))
                {
                    menuAddPrioritizableThingDefs.Add(new FloatMenuOption(td.label, delegate { component.addPriorityThingDef(td); }));
                }
            }

            List<FloatMenuOption> menuAddPrioritizableThingCategoryDefs = new List<FloatMenuOption>();
            foreach (ThingCategoryDef tcd in knownThingCategoryDefs)
            {
                Priority temp = new Priority();
                temp.thingCategoryDef = tcd;
                if (!priorities.Contains(temp))
                {
                    menuAddPrioritizableThingCategoryDefs.Add(new FloatMenuOption(tcd.label, delegate { component.addPriorityThingCategoryDef(tcd); }));
                }
            }

            Text.Font = GameFont.Small;
            for (int i = 0; i <= 5; i++)
            {
                Rect nextButton = new Rect(canvas);
                nextButton.y = i * (BUTTON_HEIGHT + BUTTON_SPACE);
                nextButton.height = BUTTON_HEIGHT;

                string buttonLabel;
                switch (i)
                {
                    /*
                    case 0:
                        buttonLabel = "Entire Mod is Currently:" + Environment.NewLine;
                        if (curEnabled)
                        {
                            buttonLabel += "ENABLED";
                        }
                        else
                        {
                            buttonLabel += "DISABLED";
                        }
                        if (Widgets.ButtonText(nextButton, buttonLabel))
                        {
                            component.enabled = !curEnabled;
                        }
                        break;
                    */
                    case 0:
                        buttonLabel = "Add Priority ThingDef";
                        if (Widgets.ButtonText(nextButton, buttonLabel))
                        {
                            if (menuAddPrioritizableThingDefs.Count > 0)
                            {
                                Find.WindowStack.Add(new FloatMenu(menuAddPrioritizableThingDefs));
                            }
                        }
                        break;
                    case 1:
                        buttonLabel = "Add Priority ThingCategoryDef";
                        if (Widgets.ButtonText(nextButton, buttonLabel))
                        {
                            if (menuAddPrioritizableThingCategoryDefs.Count > 0)
                            {
                                Find.WindowStack.Add(new FloatMenu(menuAddPrioritizableThingCategoryDefs));
                            }
                        }
                        break;
                    case 2:
                        buttonLabel = "Increment Priority";
                        if (Widgets.ButtonText(nextButton, buttonLabel))
                        {
                            if (menuIncrementPriorities.Count > 0)
                            {
                                Find.WindowStack.Add(new FloatMenu(menuIncrementPriorities));
                            }
                        }
                        break;
                    case 3:
                        buttonLabel = "Decrement Priority";
                        if (Widgets.ButtonText(nextButton, buttonLabel))
                        {
                            if (menuDecrementPriorities.Count > 0)
                            {
                                Find.WindowStack.Add(new FloatMenu(menuDecrementPriorities));
                            }
                        }
                        break;
                    case 4:
                        buttonLabel = "Remove Priority";
                        if (Widgets.ButtonText(nextButton, buttonLabel))
                        {
                            if (menuRemovePriorities.Count > 0)
                            {
                                Find.WindowStack.Add(new FloatMenu(menuRemovePriorities));
                            }
                        }
                        break;
                    case 5:
                        buttonLabel = "Clear Priority ThingDef";
                        if (Widgets.ButtonText(nextButton, buttonLabel))
                        {
                            component.clearPrioritizedThingDefs();
                        }
                        break;
                }
            }
        }

    }

}
