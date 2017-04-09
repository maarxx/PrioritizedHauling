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

            List<ThingDef> prioritizedThingDefs = component.getPrioritizedThingDefs();
            List<ThingDef> knownThingDefs = component.getKnownThingDefs();

            List<FloatMenuOption> menuRemovePrioritizedThingDefs = new List<FloatMenuOption>();
            List<FloatMenuOption> menuIncrementPrioritizedThingDefs = new List<FloatMenuOption>();
            List<FloatMenuOption> menuDecrementPrioritizedThingDefs = new List<FloatMenuOption>();
            foreach (ThingDef td in prioritizedThingDefs)
            {
                menuRemovePrioritizedThingDefs.Add(new FloatMenuOption(td.label, delegate { component.removePriorityThingDef(td); }));
                menuIncrementPrioritizedThingDefs.Add(new FloatMenuOption(td.label, delegate { component.incrementPriorityThingDef(td); }));
                menuDecrementPrioritizedThingDefs.Add(new FloatMenuOption(td.label, delegate { component.decrementPriorityThingDef(td); }));
            }

            List<FloatMenuOption> menuAddPrioritizableThingDefs = new List<FloatMenuOption>();
            foreach (ThingDef td in knownThingDefs)
            {
                if (!prioritizedThingDefs.Contains(td))
                {
                    menuAddPrioritizableThingDefs.Add(new FloatMenuOption(td.label, delegate { component.addPriorityThingDef(td); }));
                }
            }

            Text.Font = GameFont.Small;
            for (int i = 0; i <= 4; i++)
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
                        buttonLabel = "Increment Priority ThingDef";
                        if (Widgets.ButtonText(nextButton, buttonLabel))
                        {
                            if (menuIncrementPrioritizedThingDefs.Count > 0)
                            {
                                Find.WindowStack.Add(new FloatMenu(menuIncrementPrioritizedThingDefs));
                            }
                        }
                        break;
                    case 2:
                        buttonLabel = "Decrement Priority ThingDef";
                        if (Widgets.ButtonText(nextButton, buttonLabel))
                        {
                            if (menuDecrementPrioritizedThingDefs.Count > 0)
                            {
                                Find.WindowStack.Add(new FloatMenu(menuDecrementPrioritizedThingDefs));
                            }
                        }
                        break;
                    case 3:
                        buttonLabel = "Remove Priority ThingDef";
                        if (Widgets.ButtonText(nextButton, buttonLabel))
                        {
                            if (menuRemovePrioritizedThingDefs.Count > 0)
                            {
                                Find.WindowStack.Add(new FloatMenu(menuRemovePrioritizedThingDefs));
                            }
                        }
                        break;
                    case 4:
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
