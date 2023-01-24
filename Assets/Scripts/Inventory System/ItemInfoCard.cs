using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Aquapunk
{
    public class ItemInfoCard : MonoBehaviour
    {
        #region Fields
        public Image ItemIcon;
        public TextMeshProUGUI ItemName;
        public TextMeshProUGUI ItemInfo;
        public Button applyItemButton;
        public PlayerInfo playerInfo;
        #endregion
        #region Methods
        #region ClassMethods
        public void GetItemInfo(Item item)
        {
            applyItemButton.onClick.RemoveAllListeners();
            ItemIcon.sprite = item.iconItem;
            ItemName.text = item.itemName;
            ItemInfo.text = item.itemInfo;
            if(item.typeItem != Item.TypeItem.Default)
            {
                applyItemButton.gameObject.SetActive(true);
                applyItemButton.onClick.AddListener(() => playerInfo.player.SetItem(item));
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