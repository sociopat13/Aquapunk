using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aquapunk
{
    [CreateAssetMenu(fileName = "New Structure", menuName = "Structures/Create New Structures")]
    public class Structure : ScriptableObject
    {
        #region Fields
        public int id;
        public float timeBuild;

        public string itemName;
        public string itemInfo;

        public Sprite iconItem;
        #endregion

        #region Methods
        #region Class Methods
        #endregion
        #endregion
    }
}