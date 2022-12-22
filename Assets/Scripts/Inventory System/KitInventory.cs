using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Aquapunk
{
    public class KitInventory : MonoBehaviour
    {
        #region Fields
        [Header("Main settings")]
        public Player player;
        [Header("Inventory main kit and player model")]
        public GameObject playerModel;
        public Image weaponIcon;
        public Image toolIcon;
        public Image armorIcon;
        public Image artefactIcon;
        #endregion

        #region Methods
        #region Class Methods
        public void UpdateInventoryKit()
        {
            foreach(Item item in player.KitItems)
            {
                switch (item.typeItem)
                {
                    case Item.TypeItem.Weapon:
                        weaponIcon.sprite = item.iconItem;
                        break;
                    case Item.TypeItem.Tool:
                        toolIcon.sprite = item.iconItem;
                        break;
                    case Item.TypeItem.Armor:
                        armorIcon.sprite = item.iconItem;
                        break;
                    case Item.TypeItem.Artefact:
                        artefactIcon.sprite = item.iconItem;
                        break;
                }
            }
        }
        #endregion
        #region Unity Methods

        private void Awake()
        {
            player.setNewItem.AddListener(() =>UpdateInventoryKit());
        }
        #endregion
        #endregion
    }
}