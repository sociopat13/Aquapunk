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
        public new Camera camera;
        public LayerMask layerMask;
        
        [SerializeField] private List<Item> items;
        [SerializeField] private float waterCounter;
        [SerializeField] private Vector3 offsetCamera;
        #endregion
        #region Properties
        public Item Item
        {
            get { return items[-1]; }
            set { 
                items.Add(value); 
            }
        }
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
                entityMovenent.Movement(new Vector3(joystick.Horizontal, 0, joystick.Vertical));
            }
        }
        private void Update()
        {
            if (Input.GetMouseButton(0) && joystick.Direction == Vector2.zero)
            {
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                
                if(Physics.Raycast(ray, out hit, 300, layerMask))
                {
                    entityMovenent.MoveToPoint(hit.point);
                }
            }
            if(timeStanCoolDown <= 0)
            {
                if (timeAttackCoolDown > 0)
                {
                    timeAttackCoolDown -= Time.deltaTime;
                }
            }
            else
            {
                timeStanCoolDown -= Time.deltaTime;
            }
        }
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            camera = Camera.main;
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