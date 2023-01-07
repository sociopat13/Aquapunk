using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        [Header("Level system")]
        [SyncVar(hook = nameof(SyncLevel))]
        public float level;
        public float experienceLevel;
        public float maxExpLevel = 100;
        public float experienceDeath;
        public float procentExp = 10;
        public float expRange;
        public LayerMask layerXP;
        #endregion
        #region Methods
        #region Class Methods
        public void SyncLevel(float oldValue, float newValue)
        {
            level = newValue;
        }
        public void SyncHP(float oldValue, float newValue)
        {
            healthCurrent = newValue;
        }

        [Command]
        public void CmdSetExp(float exp, Player entity)
        {
            entity.setExp(exp);
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
        [Command]
        public void CmdAttackFromClient(Entity enemy, float damage, Entity entity)
        {
            enemy.Attacked(damage, entity);
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
                    print(collider.name);
                    if (collider.gameObject != gameObject && !collider.isTrigger)
                    {
                        if (isServer)
                        {
                            collider.GetComponent<Entity>().Attacked(attackDamage, this);
                        }
                        else
                        {
                            CmdAttackFromClient(collider.GetComponent<Entity>(), attackDamage, this);
                        }
                    }
                }
                timeAttackCoolDown = attackCollDown;
            }
        }

        protected virtual void DeathObject()
        {
            List<Collider> expColliders = Physics.OverlapSphere(transform.position, expRange, layerXP).ToList();
            for (int c = 0; c != expColliders.Count; c++)
            {
                if(expColliders[c].gameObject != gameObject && !expColliders[c].isTrigger)
                {
                    if (isServer)
                    {
                        expColliders[c].GetComponent<Player>().setExp(experienceDeath / expColliders.Count);
                    }
                    else
                    {
                        CmdSetExp(experienceDeath / expColliders.Count, expColliders[c].GetComponent<Player>());
                    }
                }
                print(expColliders[c].name);
            }
            Destroy(gameObject);
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