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

        private string curFileName;

        private const float BUTTON_HEIGHT = 50f;
        private const float BUTTON_SPACE = 10f;

        public MainTabWindow_PrioritizedHauling()
        {
            //base.forcePause = true;
            this.curFileName = "Testing";
        }

        public override Vector2 InitialSize
        {
            get
            {
                //return base.InitialSize;
                return new Vector2(250f, 600f);
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

            List<FloatMenuOption> menuImportablePriorities = new List<FloatMenuOption>();
            foreach (string name in SaveLoad_PrioritizedHauling.getAllFiles())
            {
                menuImportablePriorities.Add(new FloatMenuOption(name, delegate { component.setPriorities(SaveLoad_PrioritizedHauling.importPriorities(name)); } ));
            }

            Text.Font = GameFont.Small;
            for (int i = 0; i <= 8; i++)
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
                        buttonLabel = "Clear Priorities";
                        if (Widgets.ButtonText(nextButton, buttonLabel))
                        {
                            component.clearPrioritizedThingDefs();
                        }
                        break;
                    case 6:
                        buttonLabel = "Import Priorities";
                        if (Widgets.ButtonText(nextButton, buttonLabel))
                        {
                            if (menuImportablePriorities.Count > 0)
                            {
                                Find.WindowStack.Add(new FloatMenu(menuImportablePriorities));
                            }
                        }
                        break;
                    case 7:
                        buttonLabel = "Export FileName";

                        Rect topHalf = new Rect(nextButton);
                        Rect bottomHalf = new Rect(nextButton);

                        topHalf.height = topHalf.height / 2;
                        bottomHalf.height = bottomHalf.height / 2;
                        bottomHalf.y += topHalf.height;

                        Widgets.Label(topHalf, buttonLabel);
                        curFileName = Widgets.TextField(bottomHalf, curFileName);
                        break;
                    case 8:
                        buttonLabel = "Export Priorities";
                        if (Widgets.ButtonText(nextButton, buttonLabel))
                        {
                            if (curFileName.Length > 0 && curFileName.Length < 30 && !curFileName.Contains('.'))
                            {
                                SaveLoad_PrioritizedHauling.exportPriorities(curFileName, component.getPriorities());
                            }
                        }
                        break;
                }
            }
        }

    }

}
