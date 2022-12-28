using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace Aquapunk
{
    public class RpgNetworkManager : NetworkManager
    {
        public Transform startPos;
        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            GameObject player = Instantiate(playerPrefab, startPos.position, startPos.rotation);
            //player.GetComponent<Player>().canvas = FindObjectOfType<Canvas>();
            NetworkServer.AddPlayerForConnection(conn, player);
        }
    }
}

