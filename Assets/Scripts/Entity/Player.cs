using Cinemachine;
using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using RPGCharacterAnims;
using RPGCharacterAnims.Actions;
using RPGCharacterAnims.Lookups;

namespace Aquapunk
{
    public class Player : Entity
    {
        #region Fields
        public float runSpeed = 1;
        public float walkSpeed = 0.5f;
        
        private bool useInstant;

        //public StructureManager structureBuilding;

        public CinemachineVirtualCamera camera;

        //public bool buildMod;

        [SerializeField] private Vector3 offsetCamera;

        //[Header("Inventory")]
        //public List<Item> items;
        //public List<Item> KitItems;
        //public SetPostItem setNewItem;

        //public TextMeshProUGUI textWaterCounter;
        //public float WaterCounter { get; set; }
        #endregion
        #region Methods
        #region Class Methods
        //public void InstantiateHPBar(Entity entity)
        //{
        //    if (entity != this)
        //    {
        //        GameObject hpBar = Instantiate(HPBarPrefab, canvasWorld.gameObject.transform);
        //        HPBar hpBarScript = hpBar.GetComponent<HPBar>();
        //        hpBarScript.target = entity.gameObject;
        //        hpBarScript.offset = entity.offsetHPBar;
        //        hpBarScript.SetHP(entity.healthCurrent / entity.healthMax);
        //        entity.hpBar = hpBarScript;
        //    }
        //}


        //public void UpdateWaterCount()
        //{
        //    textWaterCounter.text = WaterCounter.ToString();
        //}

        //public void UpdateHPBar(float value)
        //{
        //    if (HPBar != null)
        //    {
        //        HPBar.SetHP(value);
        //    }
        //}

        //public void BuildStructure(GameObject plane)
        //{
        //    structureBuilding.BuildStructure(plane);
        //}

        public override void setDamage(float damage, Entity entity)
        {
            base.setDamage(damage, entity);

            //UpdateHPBar(healthCurrent / healthMax);
        }


        //public override void Attack()
        //{
        //    if(_timeAttackCoolDown <= 0)
        //    {
        //        rpgCharacterController.StartAction(HandlerTypes.Attack, new AttackContext("Attack", Side.None));
        //        endAttack = false;
        //    }
        //    base.Attack();
        //}

        //public virtual void SetExp(float exp)
        //{
        //    experienceLevel += exp;
        //    if (experienceLevel >= maxExpLevel)
        //    {
        //        level++;
        //        experienceLevel = 0 + experienceLevel - maxExpLevel;
        //        if (level % 5 == 0)
        //        {
        //            procentExp++;
        //        }
        //        maxExpLevel += maxExpLevel / 100 * procentExp;
        //        ExpDeathSet();
        //    }
        //}


        //public override void Armed()
        //{
        //    GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
        //    base.Attack();
        //}
        //
        //public override void Unarmed()
        //{
        //    base.Unarmed();
        //    GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        //}


        //public void SetStructure(StructureManager structure)
        //{
        //    structureBuilding = structure;
        //}

        //public void SetItem(Item item)
        //{
        //    foreach(Item MainItem in KitItems)
        //    {
        //        if(MainItem.type == item.type)
        //        if(MainItem.type == item.type)
        //        {
        //            MainItem.ResetParameters();
        //            KitItems.Remove(MainItem);
        //
        //            break;
        //        }
        //    }
        //    item.SetParameters();
        //    KitItems.Add(item);
        //    setNewItem?.Invoke(item);
        //} 

        //private void ExpDeathSet()
        //{
        //    experienceDeath = maxExpLevel / 4;
        //}

        protected override void DeathObject()
        {
            print("death " + name);
            //Destroy(hpBar);
            Destroy(gameObject);
        }

        public override void SortTrigger()
        {
            base.SortTrigger();
            if (enemys.Count > 0)
            {
                foreach (GameObject entity in enemys)
                {
                    if (entity && trigger != null && 
                        (entity.transform.position - transform.position).magnitude < 
                        (trigger.transform.position - transform.position).magnitude)
                    {
                        trigger = entity;
                    }
                    else
                    {
                        trigger = entity;
                    }
                }
            }
        }

        #endregion
        #region Unity Methods
        private void FixedUpdate()
        {
            if(_timeStanCoolDown <= 0 && _timeAttackCoolDown <= 0)
            {
                if (_rigidbody.velocity.magnitude > 0.01)
                {
                    MoveState();
                }
                else if (_rigidbody.velocity.magnitude < 0.01 && _state != StateEntity.Idle)
                {
                    IdleState();
                }
            }
        }
        private void Update()
        {
            ProcessCooldown();
            ProcessStates();
            if(trigger == null && enemys.Count > 0)
            {
                SortTrigger();
            }
            if(_timeAttackCoolDown <= 0 && endAttack)
            {
                Unarmed();
            }
        }

        public void RangeAttackNew()
        {
            rpgCharacterController.StartAction(HandlerTypes.Attack, new AttackContext("Attack", Side.None));
        }

        private void Start()
        {
            FindObjectOfType<PlayerUI>().player = this;
            _rigidbody = GetComponent<Rigidbody>();
            FindObjectOfType<CameraModifier>().player = this;
            FindObjectOfType<PlayerUI>().OnAttackAction = () => Attack();
            FindObjectOfType<PlayerUI>().OnBeginAttackAction = () => Armed();
            FindObjectOfType<PlayerUI>().OnEndAttackAction = () => EndAttack();
            FindObjectOfType<PlayerUI>().OnRangeAttack = () => RangeAttackNew();
            FindObjectOfType<PlayerUI>().OnBeginRengeAttack = () => RangeArmed();
            camera = FindObjectOfType<CinemachineVirtualCamera>();
            camera.Follow = gameObject.transform;
            camera.LookAt = gameObject.transform;
            FindObjectOfType<PlayerInfo>().player = this;
            //canvasWorld = FindObjectOfType<WorldCanvas>().GetComponent<Canvas>();
            //textWaterCounter = FindObjectOfType<PlayerUI>().waterCounter;
            //HPBar = FindObjectOfType<PlayerUI>().hpbar;
            //ExpDeathSet();
            //nm = FindObjectOfType<RpgNetworkManager>();
        }

        #endregion
        #endregion
        #region enums and delegates
        //public enum StateMovement
        //{
        //    Idle,
        //    Move
        //}

        public delegate void SetPostItem(Item item);
        #endregion
    }
}