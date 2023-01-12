using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aquapunk
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Item/Create New Item")]
    public class Item : ScriptableObject
    {
        #region Fields
        public int id;
        public int value;
        public string itemName;
        public string itemInfo;
        public Sprite iconItem;
        public TypeItem typeItem = default;
        public Player owner;
        [Header("boos parameters")]
        public float MaxHealth;
        #endregion
        #region Methods
        #region Class Methods
        public virtual void PickUp(Player player)
        {
            player.items.Add(this);
            owner = player;
        }

        public void SetParameters()
        {
            owner.healthMax += MaxHealth;
        }

        public void ResetParameters()
        {
            owner.healthMax -= MaxHealth;
            
        }
        #endregion
        #endregion
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