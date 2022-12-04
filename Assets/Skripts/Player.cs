using System.Collections;
using System.Collections.Generic;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;

namespace Aquapunk
{
    public class Player : MonoBehaviour
    {
        #region Fields
        public Joystick joystick;
        public Camera camera;

        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float speed = 5.5f;
        [SerializeField] private Vector3 offsetCamera;
        #endregion
        #region Methods
        #region Class Methods
        private void BindPositionCamera()
        {
            camera.transform.position = transform.position + offsetCamera;
        }

        private void Movement(Vector3 moveTo)
        {
            _rigidbody.velocity = (moveTo * speed * Time.fixedDeltaTime);
            Vector3 dir = moveTo.normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 1);
        }
        #endregion
        #region Unity Methods
        private void FixedUpdate()
        {
            if(joystick != null && joystick.Direction != Vector2.zero)
            {
                Movement(new Vector3(joystick.Horizontal, 0, joystick.Vertical));
            }
        }
        private void Update()
        {
            BindPositionCamera();
        }

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
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