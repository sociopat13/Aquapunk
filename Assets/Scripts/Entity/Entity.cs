using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Aquapunk
{
    public class Entity : MonoBehaviour
    {
        public GameObject HPbarPrefab;
        public Canvas canvas;
        [SerializeField] protected GameObject hpbar;
        [SerializeField] protected Image hpBarImage;

        [SerializeField] protected LayerMask layer;
        [SerializeField] protected Rigidbody _rigidbody;
        [SerializeField] protected float speed = 5.5f;
        [SerializeField] protected float healthMax = 100f;
        [SerializeField] protected float healthCurrent;
        [SerializeField] protected float attackRange = 0.5f;
        [SerializeField] protected float attackDamage = 5f;
        [SerializeField] protected float timeAttackCoolDown, attackCollDown = 0.5f;
        [SerializeField] protected float timeStanCoolDown, stanCollDown = 0.5f;
        [SerializeField] protected Vector3 attackOffset;
        [SerializeField] protected Vector3 HPBarOffset;

        public virtual void Attacked(float damage)
        {
            healthCurrent -= damage;
            timeStanCoolDown = stanCollDown;
            if(damage >= healthCurrent)
            {
                DeathObject();
            }
            hpBarImage.fillAmount = healthCurrent / healthMax;
        }

        public virtual void Attack()
        {
            if(timeAttackCoolDown <= 0)
            {
                // animate
                //detected hit enemys in range of attack
                Collider[] colliders = Physics.OverlapSphere(transform.position + attackOffset, attackRange, layer);
                // damage
                foreach (Collider collider in colliders)
                {
                    if (collider.gameObject != gameObject)
                    {
                        collider.GetComponent<Entity>().Attacked(attackDamage);
                        timeAttackCoolDown = attackCollDown;
                    }
                }
            }
        }

        protected virtual void Movement(Vector3 moveToDirection)
        {
            //move to directional on joistick
            moveToDirection = new Vector3(moveToDirection.x,0,moveToDirection.z);
            Vector3 dir = moveToDirection.normalized;
            _rigidbody.velocity = (moveToDirection * speed * Time.fixedDeltaTime);
            //rotate to directional movement
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 1);
        }

        protected virtual void DeathObject()
        {
            Destroy(gameObject);
        }

        private void Awake()
        {
            hpbar = Instantiate(HPbarPrefab, canvas.transform);
            hpBarImage = hpbar.transform.GetChild(0).GetComponent<Image>();
            healthCurrent = healthMax;
        }

        private void LateUpdate()
        {
            hpbar.transform.position = gameObject.transform.position + HPBarOffset;
        }
    }
}