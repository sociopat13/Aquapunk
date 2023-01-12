
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aquapunk
{
    public class ItemController : NetworkBehaviour
    {
        #region Fields
        public Item item;
        #endregion
        #region Methods
        #region ClassMethods
        [Server]
        private void PickUpItem(Player player)
        {
            if (player != null)
            {
                item.PickUp(player);
                NetworkServer.Destroy(gameObject);
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

