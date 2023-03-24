using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

namespace Aquapunk
{
    public class StructureInfoCard : ItemInfoCard
    {
        #region Fields
        public float infoTimeBuid;
        #endregion

        #region Methods
        #region Class methods
        public void GetStructureInfo(StructureManager structure)
        {
            Apply(structure.structure);
            if(structure != null)
            {
                applyItemButton.gameObject.SetActive(true);
                applyItemButton.onClick.AddListener(() => playerInfo.player.SetStructure(structure));
            }
            else
            {
                applyItemButton.gameObject.SetActive(false);
            }
        }
        #endregion
        #endregion
    }

}
