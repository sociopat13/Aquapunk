using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Aquapunk
{
    public class Rider : Mob
    {
        public GameObject flameEffect; // fire effect prefab

        public void Shoot(Vector3 direction)
        {
            GameObject flame = Instantiate(flameEffect, transform.position, transform.rotation);
        }

        public override void Attack()
        {
            if (_timeAttackCoolDown <= 0 && _state != StateEntity.Stan)
            {
                //anim
                //atack
                Shoot((trigger.transform.position - transform.position).normalized);
                //state_swich
                AttackState();
            }
        }

        protected override void BehaveAtTrigger()
        {
            if (trigger != null && (_state != StateEntity.Stan || _state != StateEntity.Attack))
            {

                float distance = (trigger.transform.position - transform.position).magnitude;
                if (distance < _attackRange)
                {
                    Attack();
                }

                else
                {
                    _state = StateEntity.Move;
                    _mobMovement.Movement(trigger.transform.position - transform.position);
                }
            }
        }
    }
}

