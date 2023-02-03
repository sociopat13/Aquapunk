using Cinemachine;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Jobs.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Aquapunk
{
    public class Player : Entity
    {
        #region Fields
        public GameObject HPBarPrefab;

        public Joystick joystick;
        public CinemachineVirtualCamera camera;
        public EntityMovement entityMovenent;
        [SerializeField] private Vector3 offsetCamera;
        public RPGInteresManager im;

        [Header("Inventory")]
        [SyncVar]
        public List<Item> items;
        public List<Item> KitItems;
        public SetPostItem setNewItem;

        public TextMeshProUGUI textWaterCounter;
        [SerializeField] private float waterCounter;
        #endregion
        #region Properties
        public float WaterCounter
        {
            get { return waterCounter; }
            set 
            { 
                waterCounter += value;
                textWaterCounter.text = waterCounter.ToString();
            }
        }
        #endregion
        #region Methods
        #region Class Methods

        [Server]
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

        [Client]
        public void SetHPBar(Entity entity)
        {
            if (isLocalPlayer && entity != this)
            {
                GameObject hpBar = Instantiate(HPBarPrefab, canvasWorld.gameObject.transform);
                HPBar hpBarScript = hpBar.GetComponent<HPBar>();
                hpBarScript.target = entity.gameObject;
                hpBarScript.offset = entity.offsetHPBar;
                hpBarScript.SetHP(entity.healthCurrent/entity.healthMax);
                entity.hpBar = hpBarScript;
            }
        }

        public void SetItem(Item item)
        {
            foreach(Item MainItem in KitItems)
            {
                if(MainItem.typeItem == item.typeItem)
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
        #endregion
        #region Unity Methods
        private void FixedUpdate()
        {
            if(joystick != null && joystick.Direction != Vector2.zero)
            {
                GoToDirection(entityMovenent.Movement, new Vector3(joystick.Horizontal, 0, joystick.Vertical));
            }
        }
        private void Update()
        {
            if (isLocalPlayer)
            {
                //if (_timeAttackCoolDown > 0f)
                //{
                //    _timeAttackCoolDown -= Time.deltaTime;
                //}
                CoolDown(out _timeAttackCoolDown, _timeAttackCoolDown);

                //if(_timeStanCoolDown > 0f)
                //{
                //    _timeStanCoolDown -= Time.deltaTime;
                //}
                CoolDown(out _timeStanCoolDown, _timeStanCoolDown);

                if (_timeStanCoolDown <= 0f && _state == StateEntity.Stan || _state != StateEntity.Stan && _rigidbody.velocity == Vector3.zero)
                {
                    Idle();
                }
            }
            
        }
        private void Start()
        {
            if (isLocalPlayer)
            {
                canvasWorld = FindObjectOfType<WorldCanvas>().GetComponent<Canvas>();
                _rigidbody = GetComponent<Rigidbody>();
                joystick = FindObjectOfType<FixedJoystick>();
                textWaterCounter = FindObjectOfType<PlayerUI>().waterCounter;
                FindObjectOfType<PlayerUI>().attackButton.onClick.AddListener(() => Attack());
                camera = FindObjectOfType<CinemachineVirtualCamera>();
                camera.Follow = gameObject.transform;
                camera.LookAt = gameObject.transform;
                FindObjectOfType<PlayerInfo>().player = this;
                ExpDeathSet();
                im = FindObjectOfType<RPGInteresManager>();
                im.LocalPlayer = this;
            }
        }

        #endregion
        #endregion
        #region enums and delegates
        public enum StateMovement
        {
            Idle,
            Move
        }

        public delegate void SetPostItem(Item item);
        #endregion
    }
}