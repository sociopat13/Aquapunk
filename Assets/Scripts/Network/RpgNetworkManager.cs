using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace Aquapunk
{
    public class RpgNetworkManager : NetworkManager
    {
        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            GameObject player = Instantiate(playerPrefab);
            //player.GetComponent<Player>().canvas = FindObjectOfType<Canvas>();
            NetworkServer.AddPlayerForConnection(conn, player);
        }
    }
}

