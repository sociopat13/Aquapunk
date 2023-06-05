using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using RPGCharacterAnims;
using RPGCharacterAnims.Actions;
using RPGCharacterAnims.Lookups;
using UnityEngine.InputSystem.OnScreen;

namespace Aquapunk
{
    public class Entity : MonoBehaviour
    {
        public bool switchProcess;
        protected bool endAttack;
        #region Fields
        //public Canvas canvasWorld;
        public Vector3 offsetHPBar;
        protected Rigidbody _rigidbody;
        [SerializeField] protected StateEntity _state = StateEntity.Idle;

        [Header("Attack")]
        protected RPGCharacterController rpgCharacterController;
        public GameObject trigger = null;

        public LayerMask layer;
        public Transform atackPoint;

        public List<GameObject> enemys;

        public bool notBurn;

        public float projectileDeflection;
        public float recoil;
        public GameObject projectile;
        [SerializeField]
        protected Vector3 _projetileSpawnOffser;

        public GameObject flameEffect;
        protected GameObject flame;


        [SerializeField]
        protected TypeAttack _typeAttack;
        [SerializeField]protected float _melleHitAnimTime = 1.1f;
        [SerializeField] protected float _forceRangeMultiplyRange = 10, 
            _forceRangeMultiplyMelee = 100, _forceRangeMultiplyTick = 50;
        [SerializeField] protected float _attackRange = 1.5f, _attackDamage = 20f;
        public float _timeAttackCoolDown, _attackCollDown = 0.5f, 
            _timeStanCoolDown, _stanCollDown = 0.3f,
            _timeForceCoolDown, _forceCoolDown = 0.3f,
            _timeProjectileCoolDown, _projectileCoolDown = 0.1f;
        [SerializeField] protected Vector3 _attackOffset;

        [Header("Level system")]

        //public float level;
        //public float experienceLevel;
        //
        //protected float maxExpLevel = 100;
        //protected float experienceDeath;
        //protected float procentExp = 10;
        //protected float expRange;
        //protected LayerMask layerXP;

        [Header("HP system")]

        public float healthMax = 100f;
        public float healthCurrent;

        public HPBar hpBar;

        public float TimeAttackCoolDown { 
            get 
            {
                return _timeAttackCoolDown;
            } 
            private set {} 
        }
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

        //public void DeleteHPBar()
        //{
        //    if (hpBar != null)
        //    {
        //        Destroy(hpBar.gameObject);
        //    }
        //}

        protected void CoolDown(out float coolDown, float postCoolDown)
        {
            coolDown = postCoolDown;
            if (coolDown > 0f)
            {
                coolDown -= Time.fixedDeltaTime;
            }
        }


        public virtual void RangeArmed()
        {
            if (!rpgCharacterController.HandlerExists(HandlerTypes.SwitchWeapon)) { return; }

            endAttack = false;
            var doSwitch = false;

            _typeAttack = TypeAttack.Range;
            // Create a new SwitchWeaponContext with the switch settings.
            var switchWeaponContext = new SwitchWeaponContext();

            foreach (var weapon in WeaponGroupings.Range)
            {
                if (rpgCharacterController.rightWeapon != weapon)
                {
                    var label = weapon.ToString();
                    if (label.StartsWith("TwoHand")) { label = label.Replace("TwoHand", "2H "); }
                    //if (GUI.Button(new Rect(1115, offset, 100, 30), label))
                    //{
                    doSwitch = true;
                    switchWeaponContext.type = "Switch";
                    switchWeaponContext.side = "None";
                    switchWeaponContext.leftWeapon = Weapon.Unarmed;
                    switchWeaponContext.rightWeapon = weapon;
                    //}
                }
                //offset += 30;
            }
            // Instant weapon toggle.
            //useInstant = true;// GUI.Toggle(new Rect(1000, 310, 100, 30), useInstant, "Instant");
            //if (useInstant) {
            switchWeaponContext.type = "Instant"; //}

            // Perform the weapon switch using the SwitchWeaponContext created earlier.
            if (doSwitch) { rpgCharacterController.TryStartAction(HandlerTypes.SwitchWeapon, switchWeaponContext); }
            if (rpgCharacterController.CanEndAction(HandlerTypes.SwitchWeapon)) { 
                switchProcess = true;
            }
        }

        public virtual void Armed()
        {
            //if (!rpgCharacterController.HandlerExists(HandlerTypes.SwitchWeapon)) { return; }

            endAttack = false;
            var doSwitch = false;

            
            _typeAttack = TypeAttack.Melee;
            // Create a new SwitchWeaponContext with the switch settings.
            var switchWeaponContext = new SwitchWeaponContext();

            foreach (var weapon in WeaponGroupings.TwoHandedWeapons)
            {
                if (rpgCharacterController.rightWeapon != weapon)
                {
                    var label = weapon.ToString();
                    if (label.StartsWith("TwoHand")) { label = label.Replace("TwoHand", "2H "); }
                    //if (GUI.Button(new Rect(1115, offset, 100, 30), label))
                    //{
                    doSwitch = true;
                    switchWeaponContext.type = "Switch";
                    switchWeaponContext.side = "None";
                    switchWeaponContext.leftWeapon = Weapon.Unarmed;
                    switchWeaponContext.rightWeapon = weapon;
                    //}
                }
                //offset += 30;
            }
            // Instant weapon toggle.
            //useInstant = true;// GUI.Toggle(new Rect(1000, 310, 100, 30), useInstant, "Instant");
            //if (useInstant) {
            switchWeaponContext.type = "Instant"; //}

            // Perform the weapon switch using the SwitchWeaponContext created earlier.
            if (doSwitch) { rpgCharacterController.TryStartAction(HandlerTypes.SwitchWeapon, switchWeaponContext); }

            if (rpgCharacterController.CanEndAction(HandlerTypes.SwitchWeapon)) { switchProcess = true; }

        }

        public void EndAttack()
        {
            endAttack = true;
        }



        public virtual void Unarmed()
        {
            switchProcess = false;
            if (!rpgCharacterController.HandlerExists(HandlerTypes.SwitchWeapon)) { return; }

            var doSwitch = false;

            // Create a new SwitchWeaponContext with the switch settings.
            var switchWeaponContext = new SwitchWeaponContext();

            // Unarmed.
            if (rpgCharacterController.rightWeapon != Weapon.Unarmed
                || rpgCharacterController.leftWeapon != Weapon.Unarmed)
            {
                //if (GUI.Button(new Rect(1115, 280, 100, 30), "Unarmed"))
                //{
                doSwitch = true;
                switchWeaponContext.type = "Switch";
                switchWeaponContext.side = "Both";
                switchWeaponContext.leftWeapon = Weapon.Unarmed;
                switchWeaponContext.rightWeapon = Weapon.Unarmed;
                //}
            }

            switchWeaponContext.type = "Instant"; //}

            // Perform the weapon switch using the SwitchWeaponContext created earlier.
            if (doSwitch) { rpgCharacterController.TryStartAction(HandlerTypes.SwitchWeapon, switchWeaponContext); }
        }
        public virtual void setDamage(float damage, Entity entity)
        {
            healthCurrent -= damage;

            if (healthCurrent <= 0)
            {
                DeathObject();
            }
        }

        public virtual void Attack()
        {
            if (_timeAttackCoolDown <= 0 && _timeStanCoolDown <= 0)
            {
                switch (typeAttack)
                {
                    case TypeAttack.Melee:
                        _attackCollDown = 1.3f;
                        MelleAttack();
                        break;
                    case TypeAttack.RangeTick:
                        RangeTickAttack();
                        break;
                    case TypeAttack.Range:
                        _attackCollDown = 0.4f;
                        RangeAttack();
                        break;
                }
            }
        }

        protected virtual void MelleAttack()
        {
            if(_timeAttackCoolDown <= 0)
            {
                _timeAttackCoolDown = _attackCollDown;
                rpgCharacterController.StartAction(HandlerTypes.Attack, new AttackContext("Attack", Side.None));
            }
        }


        public void MelleHit()
        {
            Collider[] enemysAtack = Physics.OverlapSphere(atackPoint.position + _attackOffset, _attackRange, layer);
            // animate
            // damage
            foreach (Collider enemy in enemysAtack)
            {
                if (enemy.gameObject != gameObject && !enemy.isTrigger
                    && _attackRange > (enemy.transform.position - transform.position).magnitude)
                {
                    enemy.GetComponent<Entity>().setDamage(_attackDamage, this);
                }
            }
        }

        protected virtual void RangeAttack()
        {
            if(_timeAttackCoolDown <= 0)
            {
                rpgCharacterController.StartAction(HandlerTypes.Attack, new AttackContext("Attack", Side.None));
                Vector3 direction = transform.forward;
                if (trigger)
                {
                    direction = trigger.transform.position - transform.position;
                    print(transform.position);
                    print(direction);
                    rpgCharacterController.StartAction(HandlerTypes.Navigation, direction.normalized);
                }

                RangeProjectile projectileObj = Instantiate(projectile, transform.forward.normalized + _projetileSpawnOffser + transform.position, new Quaternion(0, 0, 0, 0)).GetComponent<RangeProjectile>();
                direction.y = 0;
                projectileObj.direction = direction +
                    new Vector3(
                        UnityEngine.Random.Range(0, projectileDeflection), 
                        UnityEngine.Random.Range(0, projectileDeflection), 
                        UnityEngine.Random.Range(0, projectileDeflection));
                projectileObj.owner = this;
                //_rigidbody.AddForce((transform.position - trigger.transform.position).normalized * recoil);
                _timeAttackCoolDown = _attackCollDown;
            }
            
        }

        protected virtual void RangeTickAttack()
        {
            flame = Instantiate(flameEffect, transform.position, transform.rotation);
            flame.GetComponent<FlameScript>().rider = transform;
        }

        public virtual void SortTrigger()
        {
            if (enemys.Count == 0)
            {
                trigger = null;
            }
        }

        protected virtual void DeathObject()
        {
            //if(hpBar != null)
            //{
            //    Destroy(hpBar.gameObject);
            //}

            if (flame)
            {
                Destroy(flame);
            }
            //GiveExp();
            Destroy(gameObject);
        }
        //protected virtual void GiveExp()
        //{
        //    List<Collider> expColliders = Physics.OverlapSphere(transform.position, expRange, layerXP).ToList();
        //    for (int c = 0; c != expColliders.Count; c++)
        //    {
        //        if (expColliders[c].gameObject != gameObject && !expColliders[c].isTrigger)
        //        {
        //            expColliders[c].GetComponent<Player>().SetExp(experienceDeath / expColliders.Count);
        //        }
        //    }
        //}

        protected virtual void ProcessCooldown()
        {
            CoolDown(out _timeAttackCoolDown, _timeAttackCoolDown);
            
            CoolDown(out _timeStanCoolDown, _timeStanCoolDown);

            CoolDown(out _timeForceCoolDown, _timeForceCoolDown);
        }

        protected virtual void ProcessStates()
        {
            if (_state != StateEntity.Idle && _rigidbody.velocity == Vector3.zero
                && _timeAttackCoolDown <= 0 && _timeStanCoolDown <= 0f && _state != StateEntity.Death)
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
                setDamage(5f * Time.deltaTime, other.transform.parent.GetComponent<FlameScript>().rider.GetComponent<Entity>()); // уменьшаем здоровье игрока со временем
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Entity>())
            {
                enemys.Add(other.gameObject);
                trigger = other.gameObject;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (enemys.Contains(other.gameObject))
            {
                enemys.Remove(other.gameObject);
                trigger = null;
            }
        }

        private void Awake()
        {
            rpgCharacterController = GetComponent<RPGCharacterController>();
            healthCurrent = healthMax;
        }
        #endregion
        #endregion
        #region delegates and enums
        public enum StateEntity
        {
            Idle,
            Move,
            Sprint,
            Death
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