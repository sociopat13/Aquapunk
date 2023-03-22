using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aquapunk
{
    public class StructureManager : MonoBehaviour
    {
        #region Fields
        public Player owner;

        public GameObject structureHologram;
        public GameObject structureProcessBuild;
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
            Transform obj = BuildObject(plane, structureProcessBuild);
            return obj;
        }

        private Transform BuildObject(PlaneLandscape plane, GameObject obj)
        {
            Transform structureTransform = Instantiate(obj).transform;

            structureTransform.position = plane.transform.position;
            structureTransform.SetParent(plane.transform);

            return structureTransform;
        }
        #endregion
        #endregion
    }
}