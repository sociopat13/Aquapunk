using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Aquapunk
{
    public class RPGInteresManager : InterestManagement
    {
        #region Fields
        public Player LocalPlayer;
        public LocalFunc hpView;
        [Tooltip("The maximum range that objects will be visible at.")]
        public int visRange = 10;

        [Tooltip("Rebuild all every 'rebuildInterval' seconds.")]
        public float rebuildInterval = 1;
        private double lastRebuildTime;


        #endregion
        #region Methods
        #region Class Methods
        public override bool OnCheckObserver(NetworkIdentity identity, NetworkConnectionToClient newObserver)
        {
            return Vector3.Distance(identity.transform.position, newObserver.identity.transform.position) <= visRange;
        }

        public override void OnRebuildObservers(NetworkIdentity identity, HashSet<NetworkConnectionToClient> newObservers)
        {
            Vector3 position = identity.transform.position;

            foreach (NetworkConnectionToClient conn in NetworkServer.connections.Values)
            {
                if (conn != null && conn.isAuthenticated && conn.identity != null)
                {
                    if (Vector3.Distance(conn.identity.transform.position, position) < visRange)
                    {
                        newObservers.Add(conn);
                    }
                }
            }
        }

        public override void SetHostVisibility(NetworkIdentity identity, bool visible)
        {
            base.SetHostVisibility(identity, visible);
            if(LocalPlayer != null && identity.GetComponent<Entity>())
            {
                if (visible)
                {
                    hpView?.Invoke(identity.GetComponent<Entity>());
                    //LocalPlayer.SetHPBar(identity.GetComponent<Entity>());
                }
                else
                {
                    if(identity.GetComponent<Entity>())
                    {
                        identity.GetComponent<Entity>().DeleteHPBar();
                    }
                }
            }
            
        }
        #endregion
        #region Unity Methods
        [ServerCallback]
        private void Update()
        {
            if (NetworkTime.time >= lastRebuildTime + rebuildInterval)
            {
                RebuildAll();
                lastRebuildTime = NetworkTime.time;
            }
        }
        #endregion
        #endregion

        public delegate void LocalFunc(Entity entity);
    }
}

