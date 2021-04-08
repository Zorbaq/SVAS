using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Waterfall;

namespace SVAS
{
    public class ModuleWaterfallFXToggler : PartModule
    {
        [KSPField]
        public string controllerName = string.Empty;

        [KSPEvent(guiActive = true, guiName = "Toggle Waterfall Controller")]
        public void ToggleWaterfallController()
        {
            ModuleWaterfallFX waterfallModule = GetWaterfallModule();

            if (waterfallModule != null)
            {                
                float controllerOverrideValue = waterfallModule.GetControllerValue(controllerName)[0]; // No more than 1 controller per part please!

                waterfallModule.SetControllerOverride(controllerName, true);
                waterfallModule.SetControllerOverrideValue(controllerName, Math.Abs(controllerOverrideValue -= 1f));

                //UnityEngine.Debug.Log("Toggling waterfall module!");
            }
        }

        public bool EnableWaterfallController()
        {
            ModuleWaterfallFX waterfallModule = GetWaterfallModule();

            waterfallModule.SetControllerOverride(controllerName, true);
            waterfallModule.SetControllerOverrideValue(controllerName, 1f);

            return true;
        }
        public bool DisableWaterfallController()
        {
            ModuleWaterfallFX waterfallModule = GetWaterfallModule();

            waterfallModule.SetControllerOverride(controllerName, true);
            waterfallModule.SetControllerOverrideValue(controllerName, 0f);

            return false;
        }
        private ModuleWaterfallFX GetWaterfallModule()
        {
            var partModuleList = this.part.Modules;

            //UnityEngine.Debug.Log("This part: " + part.name);
            //UnityEngine.Debug.Log("PartModuleList count: " + partModuleList.Count);

            foreach (PartModule module in this.part.Modules)
            {
                //UnityEngine.Debug.Log(module.moduleName);
                if (module.moduleName == "ModuleWaterfallFX")
                {
                    //UnityEngine.Debug.Log("Found " + module.moduleName + "!");
                    return module as ModuleWaterfallFX;
                }
            }
            //UnityEngine.Debug.Log("No waterfall module found!");
            return null;
        }
    }
}
