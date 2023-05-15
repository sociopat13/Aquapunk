using Cinemachine;
using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

namespace Aquapunk
{
    public class Player : Entity
    {
        #region Fields

        public StructureManager structureBuilding;

        public GameObject HPBarPrefab;
        public HPBarUI HPBar;

        public Joystick joystick;
        public CinemachineVirtualCamera camera;
        public EntityMovement entityMovenent;

        public bool buildMod;

        [SerializeField] private Vector3 offsetCamera;

        [Header("Inventory")]
        public List<Item> items;
        public List<Item> KitItems;
        public SetPostItem setNewItem;

        public TextMeshProUGUI textWaterCounter;
        public float WaterCounter { get; set; }
        #endregion
        #region Methods
        #region Class Methods
        public void InstantiateHPBar(Entity entity)
        {
            if (entity != this)
            {
                GameObject hpBar = Instantiate(HPBarPrefab, canvasWorld.gameObject.transform);
                HPBar hpBarScript = hpBar.GetComponent<HPBar>();
                hpBarScript.target = entity.gameObject;
                hpBarScript.offset = entity.offsetHPBar;
                hpBarScript.SetHP(entity.healthCurrent / entity.healthMax);
                entity.hpBar = hpBarScript;
            }
        }


        public void UpdateWaterCount()
        {
            textWaterCounter.text = WaterCounter.ToString();
        }

        public void UpdateHPBar(float value)
        {
            if (HPBar != null)
            {
                HPBar.SetHP(value);
            }
        }

        public void BuildStructure(GameObject plane)
        {
            structureBuilding.BuildStructure(plane);
        }

        public override void setDamage(float damage, Entity entity)
        {
            base.setDamage(damage, entity);

            UpdateHPBar(healthCurrent / healthMax);
        }

        public virtual void SetExp(float exp)
        {
            experienceLevel += exp;
            if (experienceLevel >= maxExpLevel)
            {
                level++;
                experienceLevel = 0 + experienceLevel - maxExpLevel;
                if (level % 5 == 0)
                {
                    procentExp++;
                }
                maxExpLevel += maxExpLevel / 100 * procentExp;
                ExpDeathSet();
            }
        }

        public virtual void Roll()
        {
            entityMovenent.Roll();
        }

        public override void Attack()
        {
            base.Attack();
            Collider[] colliders = Physics.OverlapSphere(transform.position + _attackOffset, _attackRange, layer);
            // damage
            foreach (Collider enemy in colliders)
            {
                if (enemy.GetComponent<Entity>() &&
                    enemy.GetComponent<Entity>().hpBar == null &&
                    enemy.gameObject != gameObject && !enemy.isTrigger )
                {
                    InstantiateHPBar(enemy.GetComponent<Entity>());
                }
            }
        }


        public void SetStructure(StructureManager structure)
        {
            structureBuilding = structure;
        }

        public void SetItem(Item item)
        {
            foreach(Item MainItem in KitItems)
            {
                if(MainItem.type == item.type)
                if(MainItem.type == item.type)
                {
                    MainItem.ResetParameters();
                    KitItems.Remove(MainItem);

                    break;
                }
            }
            item.SetParameters();
            KitItems.Add(item);
            setNewItem?.Invoke(item);
        } 

        private void ExpDeathSet()
        {
            experienceDeath = maxExpLevel / 4;
        }

        protected override void DeathObject()
        {
            print("death " + name);
            Destroy(hpBar);
        }


        #endregion
        #region Unity Methods
        private void FixedUpdate()
        {
            if(_timeStanCoolDown <= 0 && _timeAttackCoolDown <= 0)
            {
                if (joystick != null && joystick.Direction != Vector2.zero)
                {
                    MoveState();
                    entityMovenent.Movement(new Vector3(joystick.Horizontal, 0, joystick.Vertical));
                }
                else if (joystick.Direction == Vector2.zero && _state != StateEntity.Idle)
                {
                    IdleState();
                }
            }
        }
        private void Update()
        {
            ProcessCooldown();
            ProcessStates();
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Roll();
            }
        }
        private void Start()
        {
            canvasWorld = FindObjectOfType<WorldCanvas>().GetComponent<Canvas>();
            _rigidbody = GetComponent<Rigidbody>();
            joystick = FindObjectOfType<FixedJoystick>();
            textWaterCounter = FindObjectOfType<PlayerUI>().waterCounter;
            FindObjectOfType<PlayerUI>().attackButton.onClick.AddListener(() => Attack());
            camera = FindObjectOfType<CinemachineVirtualCamera>();
            HPBar = FindObjectOfType<PlayerUI>().hpbar;
            camera.Follow = gameObject.transform;
            camera.LookAt = gameObject.transform;
            FindObjectOfType<PlayerInfo>().player = this;
            ExpDeathSet();
            //nm = FindObjectOfType<RpgNetworkManager>();
            FindObjectOfType<CameraModifier>().player = this;
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