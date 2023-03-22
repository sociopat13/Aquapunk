using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aquapunk
{
    public class PlaneLandscape : NetworkBehaviour
    {
        public CameraModifier cameraMod;
        private Transform structureTransform;
        [SyncVar(hook = nameof(SyncStatus))]
        public bool freePlane = true;

        public void SyncStatus(bool oldValue, bool newValue)
        {
            freePlane = newValue;
        }

        private void ClearStructureTransform()
        {
            if (structureTransform != null)
            {
                Destroy(structureTransform.gameObject);
            }
        }

        [Command]
        private void CmdBuildStructure()
        {
            freePlane = true;
            ClearStructureTransform();
            structureTransform = cameraMod.player.structureBuilding.BuildStructure(this);
        }
        private void OnMouseEnter()
        {
            if (cameraMod.player != null && cameraMod.player.structureBuilding != null)
            {
                structureTransform = cameraMod.player.structureBuilding.SpawnStructureHologram(this);
            }
        }
        
        private void OnMouseExit()
        {
            if (freePlane)
            {
                ClearStructureTransform();
            }
        }

        private void OnMouseDown()
        {
            if (freePlane)
            {
                CmdBuildStructure();
            }
        }

        private void Start()
        {
            cameraMod = FindObjectOfType<CameraModifier>();
        }
    }
}