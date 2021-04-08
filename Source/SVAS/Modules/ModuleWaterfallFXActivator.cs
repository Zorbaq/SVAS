using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Waterfall;

namespace SVAS.Modules
{
    public class ModuleWaterfallFXActivator : ModuleWaterfallFXToggler
    {
        /// <summary>
        /// In theory, the name of the module that, when active, sets the defined Waterfall controller to 1.0
        /// </summary>
        [KSPField]
        public string activatorModule = string.Empty;

        //private bool controllerIsEnabled = false;

        public override void OnAwake()
        {
            base.OnAwake();
            Events["ToggleWaterfallController"].active = false;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            var module = part.Modules.GetModule(activatorModule);

            // Apparently, each module type needs its own handling

            if (module is ModuleResourceHarvester moduleResourceHarvester)
            {
                // The problem with ResourceStatus is that it becomes "n/a" on scene load even on active converters
                //if (moduleResourceHarvester.ResourceStatus != "n/a")

                if (moduleResourceHarvester.status != null) // Status becomes null when ore is full and PAW is closed :D
                {
                    if (moduleResourceHarvester.status == "Operational" || moduleResourceHarvester.status.Contains("% load"))
                    {
                        EnableWaterfallController();
                    }
                    else
                    {
                        DisableWaterfallController();
                    }
                }
            }
            else if (module is ModuleAsteroidDrill || module is ModuleCometDrill)
            {
                BaseDrill potatoDrill = module as BaseDrill;

                if (potatoDrill.status != null)
                {
                    if (potatoDrill.status == "Connected")
                    {
                        EnableWaterfallController();
                    }
                    else
                    {
                        DisableWaterfallController();
                    }
                }
            }
        }
    }
}
