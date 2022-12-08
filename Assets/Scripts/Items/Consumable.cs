using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static Aquapunk.Consumable;

namespace Aquapunk
{
    public class Consumable : Item
    {
        #region Fields
        public ConsumableType consumableType;
        #endregion
        #region Methods
        protected override void Obtaining(GameObject game)
        {
            switch (consumableType)
            {
                case ConsumableType.water:
                    game.GetComponent<Player>().WaterCounter = _count;
                    break;
            }
            Destroy(gameObject);
        }
        #endregion
        #region Enums
        public enum ConsumableType
        {
            water,
            gold
        }
        #endregion
    }
}