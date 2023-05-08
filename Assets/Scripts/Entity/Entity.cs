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

        public bool notBurn;

        [SerializeField]
        protected TypeAttack _typeAttack;

        [SerializeField] protected float _attackRange = 1.5f, _attackDamage = 20f;
        [SerializeField] protected float _timeAttackCoolDown, _attackCollDown = 0.5f, _timeStanCoolDown, _stanCollDown = 0.5f;
        [SerializeField] protected Vector3 _attackOffset;

        [Header("Level system")]

        [SyncVar(hook = nameof(SyncLevel))]
        public float level;
        [SyncVar(hook = nameof(SyncExp))]
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

        public void SyncExp(float oldValue, float newValue)
        {
            experienceLevel = newValue;
        }

        public TypeAttack typeAttack
        {
            get
            {
                return _typeAttack;
            }
            private set { }
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
            if (healthCurrent <= 0)
            {
                DeathObject();
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
        }

        /// <summary>
        /// used to call a method on the server 
        /// handles damage
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="entity"></param>
        [Server]
        public virtual void setDamage(float damage, Entity entity)
        {
            healthCurrent -= damage;
            
            StanState();

            if(entity.typeAttack == TypeAttack.Melee)
            {
                _rigidbody.AddForce((transform.position - entity.transform.position).normalized * 100, ForceMode.Force);
            }
        }

        [Command]
        public virtual void CmdSetDamage(Entity enemy,float damage, Entity entity)
        {
            enemy.setDamage(damage, entity);
        }

        public virtual void Attack()
        {
            if (_timeAttackCoolDown <= 0 && _state != StateEntity.Stan)
            {
                AttackState();
                // animate
                // damage
                foreach (GameObject enemy in enemys)
                {
                    if (enemy.GetComponent<Collider>().gameObject != gameObject 
                        && !enemy.GetComponent<Collider>().isTrigger && 
                        _attackRange > (enemy.GetComponent<Collider>().transform.position - transform.position).magnitude)
                    {
                        if (isServer)
                        {
                            enemy.GetComponent<Entity>().setDamage(_attackDamage, this);
                        }
                        else
                        {
                            CmdSetDamage(enemy.GetComponent<Entity>(), _attackDamage, this);
                        }
                    }
                }
                _timeAttackCoolDown = _attackCollDown;
            }
        }

        protected virtual void DeathObject()
        {
            if(hpBar != null)
            {
                Destroy(hpBar.gameObject);
            }
            GiveExp();
            NetworkServer.Destroy(gameObject);
            
        }
        [Command]
        protected virtual void GiveExp()
        {
            List<Collider> expColliders = Physics.OverlapSphere(transform.position, expRange, layerXP).ToList();
            for (int c = 0; c != expColliders.Count; c++)
            {
                if (expColliders[c].gameObject != gameObject && !expColliders[c].isTrigger)
                {
                    if (isClient)
                    {
                        CmdSetExp(experienceDeath / expColliders.Count, expColliders[c].GetComponent<Player>());
                    }
                    else
                    {
                        expColliders[c].GetComponent<Player>().SetExp(experienceDeath / expColliders.Count);
                    }
                }
            }
        }

        protected virtual void ProcessCooldown()
        {
            CoolDown(out _timeAttackCoolDown, _timeAttackCoolDown);
            
            CoolDown(out _timeStanCoolDown, _timeStanCoolDown);

            //if(_attackCollDown)
        }

        protected virtual void ProcessStates()
        {
            if (_state != StateEntity.Idle && _rigidbody.velocity == Vector3.zero
                && (_timeAttackCoolDown <= 0 || _timeStanCoolDown <= 0f))
            {
                IdleState();
            }
        }
        protected virtual void AttackState()
        {
            _state = StateEntity.Attack;
        }

        protected virtual void StanState()
        {
            _timeStanCoolDown = _stanCollDown;
            _state = StateEntity.Stan;
        }

        protected virtual void IdleState()
        {
            _state = StateEntity.Idle;
        }

        protected virtual void MoveState()
        {
            _state = StateEntity.Move;
        }
        #endregion
        #region Unity Methods

        void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("flame") && !notBurn)
            {
                setDamage(5f * Time.deltaTime, other.transform.parent.GetComponent<Entity>()); // уменьшаем здоровье игрока со временем
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Entity>())
            {
                enemys.Add(other.gameObject);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (enemys.Contains(other.gameObject))
            {
                enemys.Remove(other.gameObject);
            }
        }

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

        public enum TypeAttack
        {
            Range,
            Melee
        }

        public delegate void MoveFunk(Vector3 dir);
        #endregion
    }
}