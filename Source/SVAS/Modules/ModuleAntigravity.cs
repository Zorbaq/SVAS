using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SVAS.Modules
{
    public class ModuleAntigravity : PartModule
    {
        [KSPField(guiActive = true, guiActiveEditor = false, guiName = "Vessel Gravity Vector")]
        public Vector3d gravityVector = Vector3d.zero;
        [KSPField(guiActive = true, guiActiveEditor = false, guiName = "Antigravity Generator Active")]
        public bool isActive = false;
        [KSPField(guiActive = true, guiActiveEditor = false, guiName = "Gravity Multiplier")]
        public float gravityMultiplier = 1.0f;

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
        
        public override void OnUpdate()
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                gravityVector = vessel.gravityForPos;
            }
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            if (HighLogic.LoadedSceneIsFlight)
            {
                if (isActive)
                {
                    double resourceConsumptionPerTick = (baseResourceConsumption
                        * vessel.GetTotalMass()
                        * Math.Abs(gravityMultiplier - 1) + baseResourceConsumption) * TimeWarp.fixedDeltaTime;

                    double requestedResource = part.RequestResource(PartResourceLibrary.Instance.GetDefinition("Graviolium").id
                        , resourceConsumptionPerTick
                        , ResourceFlowMode.ALL_VESSEL);

                    if (requestedResource / resourceConsumptionPerTick < 0.999)
                    {
                        isActive = false;
                        vessel.gravityMultiplier = 1.0d;
                    }
                    else
                    {
                        vessel.gravityMultiplier = gravityMultiplier;
                    }
                    
                }
                else vessel.gravityMultiplier = 1.0d;
            }
        }
    }
}
