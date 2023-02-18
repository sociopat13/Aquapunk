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
                Mob mob = other.GetComponent<Mob>();
                mobs.Add(mob);

                mob.startPos = transform.position;
                mob.radiusPatrol = GetComponent<SphereCollider>().radius;

                mob.startPos = transform.position;
                StartCoroutine(mob.TerritoryPatrol());

            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (mobs.Contains(other.GetComponent<Mob>()))
            {
                Mob mob = other.GetComponent<Mob>();
                mob.ReturnToTheArea();
            }
        }
        #endregion
        #endregion
    }
}