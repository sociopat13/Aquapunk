using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

namespace Aquapunk
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class MobMovement : EntityMovement
    {
        #region Fields
        public NavMeshAgent agent;
        #endregion

        #region methods
        #region class methods
        public void MoveToPoint(Vector3 point)
        {
            if(agent.path != null)
            {
                agent.ResetPath();
            }
            //Quaternion lookRotation = Quaternion.LookRotation(new Vector3(point.x, 0, point.z));
            //transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 1);

            //agent.SetDestination(new Vector3(point.x, 0.5f, point.z));
            StartCoroutine(MoveAgent(new Vector3(point.x, 0.5f, point.z)));
        }

        public IEnumerator MoveAgent(Vector3 point)
        {
            Vector3 direction = (point - transform.position).normalized;

            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            //transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 1);
            transform.DORotateQuaternion(Quaternion.Lerp(transform.rotation, lookRotation, 1), 0.5f);

            //yield return new WaitForSeconds(1);

            agent.SetDestination(new Vector3(point.x, 0.5f, point.z));

            yield break;
        }

        public override void Movement(Vector3 moveToDirection)
        {
            agent.isStopped = !agent.isStopped;
            agent.ResetPath();
            base.Movement(moveToDirection);
            agent.isStopped = !agent.isStopped;
        }

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>(); 
            _rigidbody = GetComponent<Rigidbody>();
        }
        #endregion
        #endregion
    }
}