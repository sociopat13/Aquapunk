using Mirror;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Aquapunk
{
    public class Entity : NetworkBehaviour
    {
        #region Fields
        public Canvas canvasWorld;
        public Vector3 offsetHPBar;
        protected Rigidbody _rigidbody;
        [SerializeField] protected StateEntity _state = StateEntity.Idle;

        [Header("Attack")]

        public LayerMask layer;
        public List<GameObject> enemys;

        [SerializeField] protected float _attackRange = 1.5f, _attackDamage = 20f;
        [SerializeField] protected float _timeAttackCoolDown, _attackCollDown = 0.5f, _timeStanCoolDown, _stanCollDown = 0.5f;
        [SerializeField] protected Vector3 _attackOffset;

        [Header("Level system")]

        [SyncVar(hook = nameof(SyncLevel))]
        public float level;
        public float experienceLevel;

        protected float maxExpLevel = 100;
        protected float experienceDeath;
        protected float procentExp = 10;
        protected float expRange;
        protected LayerMask layerXP;

        [Header("HP system")]

        public float healthMax = 100f;
        [SyncVar(hook = nameof(SyncHP))]
        public float healthCurrent;

        public HPBar hpBar;
        #endregion
        #region Methods
        #region Class Methods
        /// <summary>
        /// The hook attribute can be used to specify a function to be called when the SyncVar changes value on the client.
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        public void SyncLevel(float oldValue, float newValue)
        {
            level = newValue;
        }

        /// <summary>
        /// The hook attribute can be used to specify a function to be called when the SyncVar changes value on the client.
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        public virtual void SyncHP(float oldValue, float newValue)
        {
            healthCurrent = newValue;
            if(hpBar != null)
            {
                hpBar.SetHP(healthCurrent / healthMax);
            }
        }

        /// <summary>
        /// used to call a method on the client for execution on the server 
        /// passes the experience gained to the player object
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="entity"></param>
        [Command]
        public void CmdSetExp(float exp, Player player)
        {
            player.SetExp(exp);
        }

        /// <summary>
        /// used to call a method on the client for execution on the server 
        /// sends an attack from the client to the server
        /// </summary>
        /// <param name="enemy"></param>
        /// <param name="damage"></param>
        /// <param name="entity"></param>
        
        [Command]
        public void CmdAttackFromClient(Entity enemy, float damage, Entity entity)
        {
            enemy.Attacked(damage, entity);
        }

        public void DeleteHPBar()
        {
            if (hpBar != null)
            {
                Destroy(hpBar.gameObject);
            }
        }

        protected void CoolDown(out float coolDown, float postCoolDown)
        {
            coolDown = postCoolDown;
            if (coolDown > 0f)
            {
                coolDown -= Time.deltaTime;
            }
            else if(_state != StateEntity.Idle)
            {
                _state = StateEntity.Idle;
            }
        }

        /// <summary>
        /// used to call a method on the server 
        /// handles damage
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="entity"></param>
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
            if(_timeAttackCoolDown <= 0 && _state != StateEntity.Stan)
            {
                // animate
                //detected hit enemys in range of attack
                Collider[] colliders = Physics.OverlapSphere(transform.position + _attackOffset, _attackRange, layer);
                // damage
                foreach (Collider collider in colliders)
                {
                    if (collider.gameObject != gameObject && !collider.isTrigger)
                    {
                        if (isServer)
                        {
                            collider.GetComponent<Entity>().Attacked(_attackDamage, this);
                            //RpcAttack(collider.GetComponent<Entity>(), _attackDamage, this);
                        }
                        else
                        {
                            CmdAttackFromClient(collider.GetComponent<Entity>(), _attackDamage, this);
                        }
                    }
                }
                _timeAttackCoolDown = _attackCollDown;
                _state = StateEntity.Attack;
            }
        }

        [Server]
        protected virtual void DeathObject()
        {
            if(hpBar != null)
            {
                Destroy(hpBar.gameObject);
            }
            List<Collider> expColliders = Physics.OverlapSphere(transform.position, expRange, layerXP).ToList();
            for (int c = 0; c != expColliders.Count; c++)
            {
                if(expColliders[c].gameObject != gameObject && !expColliders[c].isTrigger)
                { 
                    if(isClient)
                    {
                        CmdSetExp(experienceDeath / expColliders.Count, expColliders[c].GetComponent<Player>());
                    }
                    else
                    {
                        expColliders[c].GetComponent<Player>().SetExp(experienceDeath / expColliders.Count);
                    }
                }
            }
            NetworkServer.Destroy(gameObject);
            
        }

        protected virtual void GoToDirection(MoveFunk moveFunk,Vector3 dir)
        {
            
            if(_state != StateEntity.Stan)
            {
                _state = StateEntity.Move;
                //anim movement

                moveFunk(dir);
            }
        }

        protected virtual void Stan()
        {
            _timeStanCoolDown = _stanCollDown;
            _state = StateEntity.Stan;
            //animation stan
        }

        protected virtual void Idle()
        {
            _state = StateEntity.Idle;
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
        #region delegates and enums
        public enum StateEntity
        {
            Stan,
            Idle,
            Move,
            Sprint,
            Attack
        }

        public delegate void MoveFunk(Vector3 dir);
        #endregion
    }
}