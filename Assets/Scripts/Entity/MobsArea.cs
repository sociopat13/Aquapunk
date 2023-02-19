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
        //private void OnTriggerEnter(Collider other)
        //{
        //    if (other.GetComponent<Mob>() && !other.isTrigger)
        //    {
        //        Mob mob = other.GetComponent<Mob>();
        //        mobs.Add(mob);
        //        mob.area = gameObject;
        //        mob.radiusPatrol = GetComponent<SphereCollider>().radius;
        //        mob.agreed = true;
        //        mob.StartPatrol();
        //
        //    }
        //}
        //
        //private void OnTriggerExit(Collider other)
        //{
        //    if (other.GetComponent<Mob>() && !other.isTrigger)
        //    {
        //        Mob mob = other.GetComponent<Mob>();
        //        mob.StopPatrol();
        //        mob.ReturnToTheArea(transform.position);
        //    }
        //}
        #endregion
        #endregion
    }
}