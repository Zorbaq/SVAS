using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SVAS.Modules
{
    [KSPModule("Antigravity Generator")]
    public class ModuleAntigravity : PartModule, IModuleInfo
    {
        [KSPField(guiActive = true, guiActiveEditor = false, guiName = "Vessel Gravity Vector")]
        public Vector3d gravityVector = Vector3d.zero;
        [KSPField(guiActive = true, guiActiveEditor = true, guiName = "Antigravity Generator Active")]
        public bool isActive = false;
        [KSPField(guiActive = true, guiActiveEditor = false, guiName = "Device Gravity Multiplier")]
        public float gravityMultiplier = 1.0f;
        [KSPField(guiActive = true, guiActiveEditor = false, guiName = "Vessel Gravity Multiplier")]
        public float vesselGravityMultiplier = 1.0f;

        private float baseResourceConsumption = 0.001f;
        
        [KSPEvent(guiActive = true, guiActiveEditor = false, guiName = "Toggle Antigravity Generator")]
        public void ToggleAGModule()
        {
            isActive = !isActive;            
        }

        [KSPEvent(guiActive = true, guiActiveEditor = false, guiName = "Gravity Multiplier +0.1")]
        public void IncreaseAGMultiplier()
        {
            gravityMultiplier += 0.1f;
        }

        [KSPEvent(guiActive = true, guiActiveEditor = false, guiName = "Gravity Multiplier 0.0")]
        public void ZeroAGMultiplier()
        {
            gravityMultiplier = 0f;
        }

        [KSPEvent(guiActive = true, guiActiveEditor = false, guiName = "Gravity Multiplier -0.1")]
        public void DecreaseAGMultiplier()
        {
            gravityMultiplier -= 0.1f;
        }


        public override void OnStart(StartState state)
        {
            base.OnStart(state);
            part.force_activate();
        }

        public override void OnUpdate()
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                //Debug.Log("Is flight! (non-fixed)");
                gravityVector = part.vessel.gravityForPos;
            }
        }

        /// <summary>
        /// Doesn't get called if the PartModule is not active. Staging activates the part but so does manual engine activation
        /// if the part is connected to an engine.
        /// </summary>
        public override void OnFixedUpdate() 
        {
            //Debug.Log("On fixed!");
            vesselGravityMultiplier = (float)part.vessel.gravityMultiplier;

            if (HighLogic.LoadedSceneIsFlight)
            {
                //Debug.Log("Is flight! (fixed)");
                if (isActive)
                {
                    //Debug.Log("isActive!");
                    double resourceConsumptionPerTick = (baseResourceConsumption
                        * part.vessel.GetTotalMass()
                        * Math.Abs(part.vessel.gravityMultiplier - 1) + baseResourceConsumption) * TimeWarp.fixedDeltaTime;

                    double requestedResource = part.RequestResource(PartResourceLibrary.Instance.GetDefinition("Graviolium").id
                        , resourceConsumptionPerTick
                        , ResourceFlowMode.ALL_VESSEL);

                    if (requestedResource / resourceConsumptionPerTick < 0.999)
                    {
                        //Debug.Log("Out of resource!");
                        isActive = false;
                        part.vessel.gravityMultiplier = 1.0d;
                    }
                    else
                    {
                        //Debug.Log("Applying multiplier!");
                        part.vessel.gravityMultiplier = gravityMultiplier;
                    }

                }
                else
                {
                    //Debug.Log("not Active!");
                    part.vessel.gravityMultiplier = 1.0d;
                }
            }
        }



        public string GetModuleTitle()
        {
            return "Antigravity Generator";
        }

        public Callback<Rect> GetDrawModulePanelCallback()
        {
            return null;
        }

        public string GetPrimaryField()
        {
            return string.Empty;
        }
    }
}
