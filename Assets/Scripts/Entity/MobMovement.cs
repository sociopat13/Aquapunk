using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Aquapunk
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class MobMovement : EntityMovement
    {
        private NavMeshAgent _agent;

        public void MoveToPoint(Vector3 point)
        {
            _agent.SetDestination(point);
        }

        public override void Movement(Vector3 moveToDirection)
        {
            _agent.isStopped = true;
            _agent.ResetPath();
            base.Movement(moveToDirection);
            _agent.isStopped = false;
        }

        private void Start()
        {
            _agent = GetComponent<NavMeshAgent>(); 
            _rigidbody = GetComponent<Rigidbody>();
        }
    }
}