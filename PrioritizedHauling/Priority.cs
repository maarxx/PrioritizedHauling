using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace PrioritizedHauling
{
    public struct Priority
    {
        public ThingDef thingDef;
        public ThingCategoryDef thingCategoryDef;

        public bool equals(Priority p2)
        {
            if (this.thingDef != null)
            {
                return this.thingDef == p2.thingDef;
            }
            else if (this.thingCategoryDef != null)
            {
                return this.thingCategoryDef == p2.thingCategoryDef;
            }
            else
            {
                return false;
            }
        }

        public string label
        {
            get
            {
                if (this.thingDef != null)
                {
                    return this.thingDef.label;
                }
                else if (this.thingCategoryDef != null)
                {
                    return this.thingCategoryDef.label;
                }
                else
                {
                    return "";
                }
            }
        }
    }
}
