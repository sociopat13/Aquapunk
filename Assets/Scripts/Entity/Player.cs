using Cinemachine;
using Mirror;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Aquapunk
{
    public class Player : Entity
    {
        #region Fields
        public GameObject HPBarPrefab;
        public HPBarUI HPBar;

        public RpgNetworkManager nm;

        public Joystick joystick;
        public CinemachineVirtualCamera camera;
        public EntityMovement entityMovenent;
        [SerializeField] private Vector3 offsetCamera;

        [Header("Inventory")]
        [SyncVar]
        public List<Item> items;
        public List<Item> KitItems;
        public SetPostItem setNewItem;

        public TextMeshProUGUI textWaterCounter;
        public float WaterCounter { get; set; }
        #endregion
        #region Methods
        #region Class Methods
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

        public void InstantiateHPBar(Entity entity)
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

        public override void Attacked(float damage, Entity entity)
        {
            base.Attacked(damage, entity);
            UpdateHPBar(healthCurrent / healthMax);
        }

        protected override void DeathObject()
        {
            Destroy(hpBar);
            NetworkManager.singleton.StopClient();
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
                CoolDown(out _timeAttackCoolDown, _timeAttackCoolDown);

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
                HPBar = FindObjectOfType<PlayerUI>().hpbar;
                camera.Follow = gameObject.transform;
                camera.LookAt = gameObject.transform;
                FindObjectOfType<PlayerInfo>().player = this;
                ExpDeathSet();
                nm = FindObjectOfType<RpgNetworkManager>();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject != gameObject && other.GetComponent<Entity>() && !other.isTrigger)
            {
                InstantiateHPBar(other.GetComponent<Entity>());
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject != gameObject && other.GetComponent<Entity>() && !other.isTrigger)
            {
                other.GetComponent<Entity>().DeleteHPBar();
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