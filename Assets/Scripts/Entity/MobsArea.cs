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
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (mobs.Contains(other.GetComponent<Mob>()))
            {
                other.GetComponent<Mob>().ReturnToTheArea();
            }
        }
        #endregion
        #endregion
    }
}