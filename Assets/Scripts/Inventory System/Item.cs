using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aquapunk
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Item/Create New Item")]
    public class Item : ScriptableObject
    {
        public int id;
        public int value;
        public string itemName;
        public Sprite iconItem;
        public TypeItem typeItem = default;

        public virtual void PickUp(Player player)
        {
            player.items.Add(this);
        }

        public enum TypeItem
        {
            Default,
            Armor,
            Tool,
            Weapon,
            Artefact
        }
    }
}