using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Aquapunk
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class MobMovement : EntityMovement
    {
        #region Fields
        private NavMeshAgent _agent;
        #endregion

        #region methods
        #region class methods
        public void MoveToPoint(Vector3 point)
        {
            _agent.ResetPath();
            _agent.SetDestination(point);
        }

        public override void Movement(Vector3 moveToDirection)
        {
            _agent.isStopped = !_agent.isStopped;
            _agent.ResetPath();
            base.Movement(moveToDirection);
            _agent.isStopped = !_agent.isStopped;
        }

        private void Start()
        {
            _agent = GetComponent<NavMeshAgent>(); 
            _rigidbody = GetComponent<Rigidbody>();
        }
        #endregion
        #endregion
    }
}