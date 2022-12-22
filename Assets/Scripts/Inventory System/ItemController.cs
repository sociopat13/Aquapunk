
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aquapunk
{
    public class ItemController : MonoBehaviour
    {
        #region Fields
        public Item item;
        #endregion
        #region Methods
        #region ClassMethods
        private void PickUpItem(Player player)
        {
            if (player != null)
            {
                item.PickUp(player);
                Destroy(gameObject);
            }
        }
        #endregion
        #region UnityMethods
        private void OnTriggerEnter(Collider other)
        {
            PickUpItem(other.GetComponent<Player>());
        }
        #endregion
        #endregion

    }
}

