using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aquapunk
{
    public class PlaneLandscape : MonoBehaviour
    {
        public CameraModifier cameraMod;
        private Transform structureTransform;
        private bool freePlane = true;
        private void ClearStructureTransform()
        {
            if (structureTransform != null)
            {
                Destroy(structureTransform.gameObject);
            }
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
                freePlane = true;
                ClearStructureTransform();
                structureTransform = cameraMod.player.structureBuilding.BuildStructure(this);
            }
        }

        private void Start()
        {
            cameraMod = FindObjectOfType<CameraModifier>();
        }
    }
}