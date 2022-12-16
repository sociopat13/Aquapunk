using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Purchasing;
using UnityEngine;
using UnityEngine.UI;

namespace Aquapunk
{
    public class InventoryPanel : MonoBehaviour
    {
        public Player player;
        public GameObject ItemCell;
        public GameObject context;

        public void LoadItem()
        {
            foreach(Transform child in context.transform)
            {
                Destroy(child.gameObject);
            }
            foreach(Item item in player.items)
            {
                GameObject cell = Instantiate(ItemCell, context.transform);

                cell.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = item.itemName;
                cell.transform.Find("Icon").GetComponent<Image>().sprite = item.iconItem;
            }
        }
    }
}