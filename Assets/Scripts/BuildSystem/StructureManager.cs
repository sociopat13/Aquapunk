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
        public Transform SpawnStructureHologram(PlaneLandscape plane)
        {
            Transform obj = BuildObject(plane, structureHologram);
            return obj;
        }

        public Transform BuildStructure(PlaneLandscape plane)
        {
            Transform obj = BuildObject(plane, structureObject);

            return obj;
        }

        [Server]
        private Transform BuildObject(PlaneLandscape plane, GameObject obj)
        {
            Transform structureTransform = Instantiate(obj).transform;

            structureTransform.position = plane.transform.position;
            structureTransform.SetParent(plane.transform);
            structureTransform.GetComponent<StructureObject>().structure = structure;
            structureTransform.GetComponent<StructureObject>().Build();

            return structureTransform;
        }
        #endregion
        #endregion
    }
}