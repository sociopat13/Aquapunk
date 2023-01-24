using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace Aquapunk
{
    public class RpgNetworkManager : NetworkManager
    {
        #region Methods
        #region ClassMethods
        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            GameObject player = Instantiate(playerPrefab);
            NetworkServer.AddPlayerForConnection(conn, player);
        }
        #endregion
        #endregion
    }
}

