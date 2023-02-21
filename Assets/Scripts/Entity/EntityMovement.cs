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

        public void RotateTo(Vector3 direction)
        {
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.DORotateQuaternion(Quaternion.Lerp(transform.rotation, lookRotation, 1), speedRotate);
        }

        public void RotateTo(Vector3 direction, TweenCallback tweenCallback)
        {
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.DORotateQuaternion(Quaternion.Lerp(transform.rotation, lookRotation, 1), speedRotate).OnComplete(tweenCallback);
        }

        public virtual void Movement(Vector3 moveToDirection)
        {
            //move to directional on joistick
            moveToDirection = new Vector3(moveToDirection.x, 0, moveToDirection.z);
            Vector3 dir = moveToDirection.normalized;
            _rigidbody.velocity = (dir * _speed * Time.fixedDeltaTime);
            //rotate to directional movement
            RotateTo(dir);
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