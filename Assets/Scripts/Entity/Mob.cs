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
        public GameObject Area;
        [SerializeField] private bool agreed = true;
        [SerializeField] private GameObject trigger;
        [SerializeField] private List<GameObject> entitys;
        [SerializeField] private Vector3 startPos;
        [SerializeField] private float minMagnitudeStartPos;

        public bool Agreed
        {
            get { return agreed; }
            set { agreed = value; }
        }

        public void returnToTheArea()
        {
            NoTrigger();
        }

        private void NoTrigger()
        {
            agreed = false;
            entitys.Clear();
            trigger = null;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(trigger == null && other.GetComponent<Entity>() && other.GetComponent<Entity>().GetType().ToString() != "Aquapunk.Mob" && agreed)
            {
                trigger = other.gameObject;
                entitys.Add(other.gameObject);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if(entitys.Contains(other.gameObject))
            {
                entitys.Remove(trigger);
                trigger = null;
                SelectTrigger();
            }
            
        }

        private void SelectTrigger()
        {
            foreach(GameObject entity in entitys)
            {
                switch (entity.GetComponent<Entity>().GetType().ToString())
                {
                    case "Aquapunk.Player":
                        trigger = entity;
                        break;
                }
            }
        }

        private void Update()
        {
            
        }

        private void FixedUpdate()
        {
            if(!agreed && transform.position != startPos)
            {
                Movement(startPos - transform.position);
            }
            if(!agreed && (transform.position - startPos).magnitude <= minMagnitudeStartPos)
            {
                agreed = true;
            }
            if (timeStanCoolDown <= 0)
            {
                if (trigger != null)
                {
                    Movement(trigger.transform.position - transform.position);

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
    }

}
