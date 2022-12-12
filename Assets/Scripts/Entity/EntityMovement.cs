using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Aquapunk
{
    public class EntityMovement : MonoBehaviour
    {
        protected Rigidbody _rigidbody;

        [SerializeField] protected float speed = 5.5f;

        public virtual void Movement(Vector3 moveToDirection)
        {
            //move to directional on joistick
            moveToDirection = new Vector3(moveToDirection.x, 0, moveToDirection.z);
            Vector3 dir = moveToDirection.normalized;
            _rigidbody.velocity = (moveToDirection * speed * Time.fixedDeltaTime);
            //rotate to directional movement
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 1);
        }

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }
    }
}