using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Aquapunk
{
    public class Flamethrower : Mob
    {
        public GameObject flameEffect; // fire effect prefab

        public float rearDamageMultiplier;

        private GameObject flame;

        public override void setDamage(float damage, Entity entity)
        {
            
            Vector3 directionToTarget = entity.transform.position - transform.position;
            Vector3 forward = transform.forward;
            float dotProduct = Vector3.Dot(directionToTarget, forward);
            float multiplier = 1;
            if (dotProduct < 0)
            {
                multiplier = rearDamageMultiplier;
                Debug.Log("DD");
            }
            base.setDamage(damage * multiplier, entity);
        }

        public void Shoot(Vector3 direction)
        {
            flame = Instantiate(flameEffect, transform.position, transform.rotation);
            flame.GetComponent<FlameScript>().rider = transform;
        }

        public override void Attack()
        {
            if (_timeStanCoolDown <= 0 && _timeAttackCoolDown <= 0)
            {
                //anim
                //atack
                Shoot((trigger.transform.position - transform.position).normalized);
                //state_swich
                _timeAttackCoolDown = _attackCollDown;
            }
        }

        protected override void BehaveAtTrigger()
        {
            if (trigger != null && _timeStanCoolDown <= 0 && _timeAttackCoolDown <= 0)
            {
                _mobMovement.RotateTo((trigger.transform.position - transform.position).normalized);
            }
            base.BehaveAtTrigger();
        }

        protected override void DeathObject()
        {
            base.DeathObject();
            if (flame)
            {
                Destroy(flame);
            }
        }
    }
}

