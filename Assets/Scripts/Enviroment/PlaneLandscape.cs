using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aquapunk
{
    public class PlaneLandscape : NetworkBehaviour
    {
        public PlayerInfo playerInfo;
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
            structureTransform = playerInfo.player.structureBuilding.BuildStructure(gameObject);
        }
        private void OnMouseEnter()
        {
            if (playerInfo.player != null && playerInfo.player.structureBuilding != null)
            {
                structureTransform = playerInfo.player.structureBuilding.SpawnStructureHologram(gameObject);
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
            playerInfo = FindObjectOfType<PlayerInfo>();
        }
    }
}