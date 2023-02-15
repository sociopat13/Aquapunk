using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aquapunk
{
    public class MobsArea : MonoBehaviour
    {
        #region Fields
        [SerializeField] private List<Mob> mobs;
        #endregion
        #region Methods
        #region Unity Methods
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Mob>() && !mobs.Contains(other.GetComponent<Mob>()))
            {
                mobs.Add(other.GetComponent<Mob>());

                other.GetComponent<Mob>().startPos = transform.position;

                StartCoroutine(other.GetComponent<Mob>().TerritoryPatrol());
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (mobs.Contains(other.GetComponent<Mob>()))
            {
                StopCoroutine(other.GetComponent<Mob>().TerritoryPatrol());
                other.GetComponent<Mob>().ReturnToTheArea();
            }
        }
        #endregion
        #endregion
    }
}