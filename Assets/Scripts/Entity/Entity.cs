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

        /// <summary>
        /// The hook attribute can be used to specify a function to be called when the SyncVar changes value on the client.
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        [Client]
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
        //[Server]
        public virtual void setDamage(float damage, Entity entity)
        {
            healthCurrent -= damage;
            if (healthCurrent <= 0)
            {
                //FindObjectOfType<RpgNetworkManager>().StopClient();
                DeathObject();
                return;
            }
            StanState();
        }

        public virtual void Attack()
        {
            
            if (_timeAttackCoolDown <= 0 && _state != StateEntity.Stan)
            {
                // animate
                //detected hit enemys in range of attack
                Collider[] colliders = Physics.OverlapSphere(transform.position + _attackOffset, _attackRange, layer);
                // damage
                foreach (Collider collider in colliders)
                {
                    if (collider.gameObject != gameObject && !collider.isTrigger)
                    {
                        collider.GetComponent<Entity>().setDamage(_attackDamage, this);
                    }
                }
                AttackState();
            }
        }

        protected virtual void DeathObject()
        {
            if(hpBar != null)
            {
                Destroy(hpBar.gameObject);
            }
            GiveExp();
            //if (isClient)
            //{
            //    FindObjectOfType<RpgNetworkManager>().OnServerDisconnect(GetComponent<NetworkIdentity>().connectionToClient);
            //}
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

            if ((_timeStanCoolDown <= 0f || _rigidbody.velocity == Vector3.zero || _timeAttackCoolDown <= 0) && _state != StateEntity.Idle)
            {
                IdleState();
            }
        }
        protected virtual void AttackState()
        {
            _timeAttackCoolDown = _attackCollDown;
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
        #endregion
        #region Unity Methods

        void OnTriggerStay(Collider other)
        {
            
            if (other.CompareTag("flame") && !notBurn)
            {
                setDamage(5f * Time.deltaTime, other.transform.parent.GetComponent<Entity>()); // уменьшаем здоровье игрока со временем
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

        public delegate void MoveFunk(Vector3 dir);
        #endregion
    }
}