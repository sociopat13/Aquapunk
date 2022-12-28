using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Events;

namespace Aquapunk
{
    public class Player : Entity
    {
        #region Fields
        public Joystick joystick;
        public TextMeshProUGUI textWaterCounter;
        public new CinemachineVirtualCamera camera;
        public EntityMovement entityMovenent;
        [Header("Inventory")]
        public List<Item> items;
        //items
        public List<Item> KitItems;
        public SetPostItem setNewItem;
        [SerializeField] private float waterCounter;
        [SerializeField] private Vector3 offsetCamera;
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
        #endregion
        #region Unity Methods
        private void FixedUpdate()
        {
            if(joystick != null && joystick.Direction != Vector2.zero)
            {
                GoToDir(entityMovenent.Movement, new Vector3(joystick.Horizontal, 0, joystick.Vertical));
                //entityMovenent.Movement(new Vector3(joystick.Horizontal, 0, joystick.Vertical));
            }
        }
        private void Update()
        {
            if (timeAttackCoolDown > 0f)
            {
                timeAttackCoolDown -= Time.deltaTime;
            }
            if(timeStanCoolDown > 0f)
            {
                timeStanCoolDown -= Time.deltaTime;
            }
            if (timeStanCoolDown <= 0f && state == StateEntity.Stan || state != StateEntity.Stan && _rigidbody.velocity == Vector3.zero)
            {
                Idle();
            }
        }
        private void Start()
        {
            if (isLocalPlayer)
            {
                canvas = FindObjectOfType<Canvas>();
                joystick = FindObjectOfType<FixedJoystick>();
                textWaterCounter = FindObjectOfType<PlayerUI>().waterCounter;
                FindObjectOfType<PlayerUI>().attackButton.onClick.AddListener(() => Attack());
                camera = FindObjectOfType<CinemachineVirtualCamera>();
                camera.Follow = gameObject.transform;
                camera.LookAt = gameObject.transform;
                FindObjectOfType<PlayerInfo>().player = this;
            }
        }

        #endregion
        #endregion
        #region enums
        public enum StateMovement
        {
            Idle,
            Move
        }

        public delegate void SetPostItem(Item item);
        #endregion
    }
}