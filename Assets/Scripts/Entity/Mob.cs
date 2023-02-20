using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Aquapunk
{
    public class Mob : Entity
    {
        #region fields
        public Vector3 startPosition;

        public GameObject trigger = null;
        public List<GameObject> dropItems;
        
        public bool isPatrolling = true;
        public bool agreed = true;
        public float radiusPatrol;

        protected Coroutine patroling;
        protected MobMovement _mobMovement;
        [SerializeField] protected float _minStartPosDistance;
        [SerializeField] protected float _timeWaitPatrol;
        #endregion
        #region Properties
        public bool Agreed
        {
            get { return agreed; }
            set { agreed = value; }
            
        }
        #endregion
        #region Methods
        #region Class Methods

        public IEnumerator TerritoryPatrol()
        {
            // Move to a random point if not attacking or stunned
            while (isPatrolling)
            {
                Debug.Log("patrol");
                if (_state != StateEntity.Stan || _state != StateEntity.Attack)
                {
                    if (trigger == null)
                    {
                        Vector3 point = startPosition + (Random.insideUnitSphere * radiusPatrol);

                        try
                        {
                            _mobMovement.MoveToPoint(point);
                        }
                        catch (Exception e)
                        {
                            Debug.LogError($"Error moving to point {point}: {e.Message}");
                        }
                    }
                }

                yield return new WaitForSeconds(_timeWaitPatrol);

                // Reset the agent's path
                if (_mobMovement.agent.path != null)
                {
                    _mobMovement.agent.ResetPath();
                }
            }
        }

        public void StartPatrol()
        {
            agreed = true;
            isPatrolling = true;
            patroling = StartCoroutine(TerritoryPatrol());
        }

        public void StopPatrol()
        {
            agreed = false;
            isPatrolling = false;
            StopCoroutine(patroling);
        }

        public override void Attacked(float damage, Entity entity)
        {
            base.Attacked(damage, entity);
            trigger = entity.gameObject;
        }

        public void SortTrigger()
        {
            foreach (GameObject entity in enemys)
            {
                switch (entity.GetComponent<Entity>().GetType().ToString())
                {
                    case "Aquapunk.Player":
                        trigger = entity;
                        break;
                }
            }
        }

        [Server]
        protected override void DeathObject()
        {
            foreach(GameObject item in dropItems)
            {
                GameObject itemObject = Instantiate(item, transform.position, item.transform.rotation);
                NetworkServer.Spawn(itemObject);
            }
            StopAllCoroutines();
            base.DeathObject();
        }

        protected virtual void BehaveAtTrigger()
        {
            if (trigger != null && (_state != StateEntity.Stan || _state != StateEntity.Attack))
            {

                float distance = (trigger.transform.position - transform.position).magnitude;
                if (distance <= _attackRange)
                {
                    Attack();
                }

                else
                {
                    _state = StateEntity.Move;
                    _mobMovement.Movement(trigger.transform.position - transform.position);
                }
            }
        }

        
        #endregion
        #region Unity Methods

        private void OnTriggerEnter(Collider other)
        {
            if(trigger == null && other.GetComponent<Entity>() && other.GetComponent<Entity>().GetType() != typeof(Mob) && agreed && !other.isTrigger)
            {
                isPatrolling = false;
                StopCoroutine(TerritoryPatrol());
                trigger = other.gameObject;
                enemys.Add(other.gameObject);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if(enemys.Contains(other.gameObject))
            {
                isPatrolling = true;
                StartCoroutine(TerritoryPatrol());
                enemys.Remove(trigger);
                trigger = null;
                SortTrigger();
            }
            
        }

        private void Update()
        {
            BehaveAtTrigger();
            ProcessCooldown();
        }

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _mobMovement = GetComponent<MobMovement>();

            startPosition = transform.position;

            StartPatrol();
        }
        #endregion
        #endregion
    }

}
