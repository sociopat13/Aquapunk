using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Aquapunk
{
    public class Entity : MonoBehaviour
    {
        #region Fields
        public Canvas canvasWorld;
        public Vector3 offsetHPBar;
        protected Rigidbody _rigidbody;
        [SerializeField] protected StateEntity _state = StateEntity.Idle;

        [Header("Attack")]

        public LayerMask layer;
        public Transform atackPoint;

        public List<GameObject> enemys;

        public bool notBurn;

        [SerializeField]
        protected TypeAttack _typeAttack;

        [SerializeField] protected float _forceRangeMultiplyRange = 10, 
            _forceRangeMultiplyMelee = 100, _forceRangeMultiplyTick = 50;
        [SerializeField] protected float _attackRange = 1.5f, _attackDamage = 20f;
        [SerializeField] protected float _timeAttackCoolDown, _attackCollDown = 0.5f, 
            _timeStanCoolDown, _stanCollDown = 0.3f,
            _timeForceCoolDown, _forceCoolDown = 0.3f;
        [SerializeField] protected Vector3 _attackOffset;

        [Header("Level system")]

        public float level;
        public float experienceLevel;

        protected float maxExpLevel = 100;
        protected float experienceDeath;
        protected float procentExp = 10;
        protected float expRange;
        protected LayerMask layerXP;

        [Header("HP system")]

        public float healthMax = 100f;
        public float healthCurrent;

        public HPBar hpBar;
        #endregion
        #region Methods
        #region Class Methods
        public TypeAttack typeAttack
        {
            get
            {
                return _typeAttack;
            }
            private set { }
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
                coolDown -= Time.fixedDeltaTime;
            }
        }

        /// <summary>
        /// used to call a method on the server 
        /// handles damage
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="entity"></param>
        public virtual void setDamage(float damage, Entity entity)
        {
            healthCurrent -= damage;

            if(_timeForceCoolDown <= 0)
            {
                _rigidbody.velocity = Vector3.zero;

                float forceRangeMultiply = _forceRangeMultiplyMelee;

                if (entity)
                {
                    switch (entity.typeAttack)
                    {
                        case TypeAttack.Range:
                            forceRangeMultiply = _forceRangeMultiplyRange;
                            break;
                        case TypeAttack.RangeTick:
                            forceRangeMultiply = _forceRangeMultiplyTick;
                            break;
                        default:
                            _timeStanCoolDown = _stanCollDown;
                            break;
                    }
                }

                print(forceRangeMultiply);

                _rigidbody.AddForce((transform.position - entity.transform.position).normalized * forceRangeMultiply);
                _timeForceCoolDown = _forceCoolDown;
            }

            if (hpBar)
            {
                hpBar.SetHP(healthCurrent / healthMax);
            }
            if (healthCurrent <= 0)
            {
                DeathObject();
            }
        }


        public virtual void Attack()
        {
            if (_timeAttackCoolDown <= 0 && _timeStanCoolDown <= 0)
            {
                Collider[] enemysAtack = Physics.OverlapSphere(atackPoint.position + _attackOffset, _attackRange, layer);
                // animate
                // damage
                foreach (Collider enemy in enemysAtack)
                {
                    if (enemy.gameObject != gameObject && !enemy.isTrigger && 
                        _attackRange > (enemy.transform.position - transform.position).magnitude)
                    {
                        enemy.GetComponent<Entity>().setDamage(_attackDamage, this);
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
            Destroy(gameObject);
            
        }
        protected virtual void GiveExp()
        {
            List<Collider> expColliders = Physics.OverlapSphere(transform.position, expRange, layerXP).ToList();
            for (int c = 0; c != expColliders.Count; c++)
            {
                if (expColliders[c].gameObject != gameObject && !expColliders[c].isTrigger)
                {
                    expColliders[c].GetComponent<Player>().SetExp(experienceDeath / expColliders.Count);
                }
            }
        }

        protected virtual void ProcessCooldown()
        {
            CoolDown(out _timeAttackCoolDown, _timeAttackCoolDown);
            
            CoolDown(out _timeStanCoolDown, _timeStanCoolDown);

            CoolDown(out _timeForceCoolDown, _timeForceCoolDown);
        }

        protected virtual void ProcessStates()
        {
            if (_state != StateEntity.Idle && _rigidbody.velocity == Vector3.zero
                && _timeAttackCoolDown <= 0 && _timeStanCoolDown <= 0f)
            {
                IdleState();
            }
        }


        protected virtual void IdleState()
        {
            _state = StateEntity.Idle;
            _rigidbody.velocity = Vector3.zero;
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
                print("Burn!");
                setDamage(5f * Time.deltaTime, other.transform.parent.GetComponent<FlameScript>().rider.GetComponent<Entity>()); // уменьшаем здоровье игрока со временем
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
            Idle,
            Move,
            Sprint
        }

        public enum TypeAttack
        {
            Range,
            Melee,
            RangeTick
        }

        public delegate void MoveFunk(Vector3 dir);
        #endregion
    }
}