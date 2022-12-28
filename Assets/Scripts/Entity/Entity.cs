using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Aquapunk
{
    public class Entity : NetworkBehaviour
    {
        #region Fields
        public Canvas canvas;
        public Rigidbody _rigidbody;

        protected StateEntity state = StateEntity.Idle;

        [Header("Attack")]
        [SerializeField] protected float attackRange = 1.5f;
        [SerializeField] protected float attackDamage = 20f;
        [SerializeField] protected float timeAttackCoolDown, attackCollDown = 0.5f, timeStanCoolDown, stanCollDown = 0.5f;
        [SerializeField] protected Vector3 attackOffset;
        [SerializeField] protected List<GameObject> enemys;
        [SerializeField] protected LayerMask layer;
        [Header("HP bar")]
        public float healthMax = 100f;
        [SyncVar(hook = nameof(SyncHP))]
        [SerializeField] protected float healthCurrent;
        [SerializeField] protected Vector3 HPBarOffset;
        #endregion
        #region Methods
        #region Class Methods
        public void SyncHP(float oldValue, float newValue)
        {
            healthCurrent = newValue;
        }
        [Server]
        public virtual void Attacked(float damage, Entity entity)
        {
            if (damage >= healthCurrent)
            {
                DeathObject();
                return;
            }
            healthCurrent -= damage;
            Stan();
        }
        
        public virtual void Attack()
        {
            if(timeAttackCoolDown <= 0 && state != StateEntity.Stan)
            {
                // animate
                //detected hit enemys in range of attack
                Collider[] colliders = Physics.OverlapSphere(transform.position + attackOffset, attackRange, layer);
                // damage
                foreach (Collider collider in colliders)
                {
                    if (collider.gameObject != gameObject && !collider.isTrigger)
                    {
                        if (isServer)
                        {
                            collider.GetComponent<Entity>().Attacked(attackDamage, this);
                        }
                        else
                        {
                            AttackFromClient(collider.GetComponent<Entity>(), attackDamage, this);
                        }
                        timeAttackCoolDown = attackCollDown;
                    }
                }
            }
        }
        [Command]
        public void AttackFromClient(Entity enemy, float damage, Entity entity)
        {
            enemy.Attacked(damage, entity);
        }

        protected virtual void GoToDir(MoveFunk moveFunk,Vector3 dir)
        {
            
            if(state != StateEntity.Stan)
            {
                state = StateEntity.Move;
                //anim movement

                moveFunk(dir);
            }
        }

        protected virtual void Stan()
        {
            timeStanCoolDown = stanCollDown;
            state = StateEntity.Stan;
            //animation stan
        }

        protected virtual void Idle()
        {
            state = StateEntity.Idle;
            //anim state
        }

        protected virtual void DeathObject()
        {
            Destroy(gameObject);
        }
        #endregion
        #region Unity Methods

        private void Awake()
        {
            healthCurrent = healthMax;
        }
        #endregion
        #endregion

        public enum StateEntity
        {
            Stan,
            Idle,
            Move,
            Sprint
        }

        public delegate void MoveFunk(Vector3 dir);
    }
}