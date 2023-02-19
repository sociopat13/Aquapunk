using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

namespace Aquapunk
{
    public class EntityMovement : MonoBehaviour
    {
        #region Fields
        public float speedRotate;

        protected Rigidbody _rigidbody;
        [SerializeField] protected float _speed = 5.5f;
        #endregion
        #region Methods
        #region Class Methods
        public virtual void Movement(Vector3 moveToDirection)
        {
            //move to directional on joistick
            moveToDirection = new Vector3(moveToDirection.x, 0, moveToDirection.z);
            Vector3 dir = moveToDirection.normalized;
            _rigidbody.velocity = (dir * _speed * Time.fixedDeltaTime);
            //rotate to directional movement
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
            transform.DORotateQuaternion(Quaternion.Lerp(transform.rotation, lookRotation, 1), speedRotate);
        }
        #endregion
        #region Unity Methods
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }
        #endregion
        #endregion
    }
}