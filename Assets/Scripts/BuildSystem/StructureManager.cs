using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aquapunk
{
    public class StructureManager : NetworkBehaviour
    {
        #region Fields
        public Structure structure;
        public Player owner;

        public GameObject structureHologram;
        public GameObject structureObject;
        #endregion

        #region Methods
        #region Class Methods
        public void SpawnStructureHologram(GameObject plane)
        {
            BuildObject(plane, structureHologram);
        }

        public void BuildStructure(GameObject plane)
        {
            BuildObject(plane, structureObject);
        }

        
        private void BuildObject(GameObject plane, GameObject obj)
        {
            Transform structureTransform = Instantiate(obj).transform;

            structureTransform.position = plane.transform.position;
            structureTransform.SetParent(plane.transform);
            structureTransform.GetComponent<StructureObject>().structure = structure;
            structureTransform.GetComponent<StructureObject>().Build();
            plane.GetComponent<PlaneLandscape>().structureTransform = structureTransform;
        }
        #endregion
        #endregion
    }
}