using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;

namespace Aquapunk
{
    public class Player : Entity
    {
        #region Fields
        public Joystick joystick;
        public TextMeshProUGUI textWaterCounter;
        public EntityMovement entityMovenent;
        public List<Item> items;

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
            if (timeAttackCoolDown > 0)
            {
                timeAttackCoolDown -= Time.deltaTime;
            }
            if(timeStanCoolDown > 0)
            {
                timeStanCoolDown -= Time.deltaTime;
            }
            if (timeStanCoolDown <= 0 && state == StateEntity.Stan || _rigidbody.velocity == Vector3.zero)
            {
                Idle();
            }
        }
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            entityMovenent = GetComponent<EntityMovement>();
        }

        #endregion
        #endregion
        #region enums
        public enum StateMovement
        {
            Idle,
            Move
        }
        #endregion
    }
}