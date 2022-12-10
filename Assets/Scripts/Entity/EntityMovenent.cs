using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EntityMovenent : MonoBehaviour
{
    private NavMeshAgent _agent;
    private Rigidbody _rigidbody;

    [SerializeField] protected float speed = 5.5f;


    public void MoveToPoint(Vector3 point)
    {
        _agent.SetDestination(point);
    }

    public void Movement(Vector3 moveToDirection)
    {
        _agent.isStopped = true;
        _agent.ResetPath();
        //move to directional on joistick
        moveToDirection = new Vector3(moveToDirection.x, 0, moveToDirection.z);
        Vector3 dir = moveToDirection.normalized;
        _rigidbody.velocity = (moveToDirection * speed * Time.fixedDeltaTime);
        //rotate to directional movement
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 1);
        _agent.isStopped = false;
    }

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _rigidbody = GetComponent<Rigidbody>();
    }
}
