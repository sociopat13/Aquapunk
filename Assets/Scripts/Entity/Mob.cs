using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Aquapunk
{
    public class Mob : Entity
    {
        #region fields
        public GameObject Area;
        public List<GameObject> DropItems;
        public MobMovement mobMovement;

        [SerializeField] private bool agreed = true;
        [SerializeField] private GameObject trigger;
        [SerializeField] private Vector3 startPos;
        [SerializeField] private float radiusPatrol;
        [SerializeField] private float minMagnitudeStartPos;
        [SerializeField] private float timeWaitPatrol;
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
            while (true)
            {
                Vector3 point = startPos + (Random.insideUnitSphere * radiusPatrol);
                mobMovement.MoveToPoint(point);
                //print(point);
                yield return new WaitForSeconds(timeWaitPatrol);
                mobMovement.agent.ResetPath();
            }
        }
        public void ReturnToTheArea()
        {
            NoTrigger();
        }

        public override void Attacked(float damage, Entity entity)
        {
            base.Attacked(damage, entity);
            trigger = entity.gameObject;

        }

        //clears all triggers and returns the object to the region
        private void NoTrigger()
        {
            agreed = false;
            enemys.Clear();
            trigger = null;
            MovementInTheArea();
        }

        private void SortTrigger()
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

        private void MovementInTheArea()
        { 
            mobMovement.MoveToPoint(startPos);
        }

        [Server]
        protected override void DeathObject()
        {
            foreach(GameObject item in DropItems)
            {
                GameObject itemObject = Instantiate(item, transform.position, item.transform.rotation);
                NetworkServer.Spawn(itemObject);
            }
            StopAllCoroutines();
            base.DeathObject();
        }
        #endregion
        #region Unity Methods

        private void OnTriggerEnter(Collider other)
        {
            if(trigger == null && other.GetComponent<Entity>() && other.GetComponent<Entity>().GetType().ToString() != "Aquapunk.Mob" && agreed)
            {
                trigger = other.gameObject;
                enemys.Add(other.gameObject);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if(enemys.Contains(other.gameObject))
            {
                enemys.Remove(trigger);
                trigger = null;
                SortTrigger();
            }
            
        }

        private void Update()
        { 
            if (!agreed && (transform.position - startPos).magnitude <= minMagnitudeStartPos)
            {
                agreed = true;
            }
            if (trigger != null && _state != StateEntity.Stan)
            {        
                float distance = (trigger.transform.position - transform.position).magnitude;
                if (distance <= _attackRange)
                {
                    Attack();
                }

                else if (_state != StateEntity.Attack)
                {
                    GoToDirection(mobMovement.Movement, trigger.transform.position - transform.position);
                }
            }

            //if (_timeAttackCoolDown > 0f)
            //{
            //    _timeAttackCoolDown -= Time.deltaTime;
            //}
            CoolDown(out _timeAttackCoolDown, _timeAttackCoolDown);

            //if (_timeStanCoolDown > 0f)
            //{
            //    _timeStanCoolDown -= Time.deltaTime;
            //}
            CoolDown(out _timeStanCoolDown, _timeStanCoolDown);
            
            if (_timeStanCoolDown <= 0f || _rigidbody.velocity == Vector3.zero && _state != StateEntity.Stan )
            {
                Idle();
            }
        }

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            mobMovement = GetComponent<MobMovement>();
            startPos = Area.transform.position;
            radiusPatrol = Area.GetComponent<SphereCollider>().radius;
        }
        #endregion
        #endregion
    }

}
