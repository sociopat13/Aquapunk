using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace Aquapunk
{
    public class Mob : Entity
    {
        #region fields
        public GameObject Area;
        public GameObject WaterItem;
        [SerializeField] private bool agreed = true;
        [SerializeField] private GameObject trigger;
        [SerializeField] private Vector3 startPos;
        [SerializeField] private float minMagnitudeStartPos;
        [SerializeField] private float waterCount;
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
        public void returnToTheArea()
        {
            NoTrigger();
        }

        public override void Attacked(float damage, Entity entity)
        {
            base.Attacked(damage, entity);
            trigger = entity.gameObject;

        }

        private void NoTrigger()
        {
            agreed = false;
            enemys.Clear();
            trigger = null;
            MovementInTheArea();
        }

        private void SelectTrigger()
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
            if (!agreed)
            {
                entityMovenent.MoveToPoint(startPos);
            }
        }

        protected override void DeathObject()
        {
            Consumable water = Instantiate(WaterItem, transform.position, WaterItem.transform.rotation).GetComponent<Consumable>();
            water.Count = waterCount;
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
                SelectTrigger();
            }
            
        }

        private void FixedUpdate()
        {
            if (!agreed && (transform.position - startPos).magnitude <= minMagnitudeStartPos)
            {
                agreed = true;
            }
            if (timeStanCoolDown <= 0)
            {
                if (trigger != null)
                {
                    entityMovenent.MoveToPoint(trigger.transform.position);

                    float distance = (trigger.transform.position - transform.position).magnitude;
                    if (distance < attackRange)
                    {
                        Attack();
                    }
                }
                if (timeAttackCoolDown > 0)
                {
                    timeAttackCoolDown -= Time.deltaTime;
                }
            }
            else
            {
                timeStanCoolDown -= Time.deltaTime;
            }
        }

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            startPos = Area.transform.position;
        }
        #endregion
        #endregion
    }

}
