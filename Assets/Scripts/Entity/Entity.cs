using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Aquapunk
{
    public class Entity : MonoBehaviour
    {
        #region Fields
        public GameObject HPbarPrefab;
        public Canvas canvas;

        [SerializeField] protected GameObject hpbar;
        [SerializeField] protected Image hpBarImage;

        [SerializeField] protected LayerMask layer;
        [SerializeField] protected Rigidbody _rigidbody;
        [SerializeField] protected float healthMax = 100f;
        [SerializeField] protected float healthCurrent;
        [SerializeField] protected float attackRange = 0.5f;
        [SerializeField] protected float attackDamage = 5f;
        [SerializeField] protected float timeAttackCoolDown, attackCollDown = 0.5f, 
            timeStanCoolDown, stanCollDown = 0.5f;
        [SerializeField] protected Vector3 attackOffset;
        [SerializeField] protected Vector3 HPBarOffset;
        [SerializeField] protected List<GameObject> enemys;
        [SerializeField] protected StateEntity state = StateEntity.Idle;
        #endregion
        #region Methods
        #region Class Methods
        public virtual void Attacked(float damage, Entity entity)
        {
            if (damage >= healthCurrent)
            {
                DeathObject();
                return;
            }
            healthCurrent -= damage;
            Stan();

            hpBarImage.fillAmount = healthCurrent / healthMax;
        }

        public virtual void Attack()
        {
            if(timeAttackCoolDown <= 0 && state != StateEntity.Stan)
            {
                // animate
                //detected hit enemys in range of attack
                Collider[] colliders = Physics.OverlapSphere(transform.position + attackOffset, attackRange, layer);
                // damage
                foreach (Collider collider in colliders)
                {
                    if (collider.gameObject != gameObject && !collider.isTrigger)
                    {
                        collider.GetComponent<Entity>().Attacked(attackDamage, this);
                        timeAttackCoolDown = attackCollDown;
                    }
                }
            }
        }

        protected virtual void GoToDir(MoveFunk moveFunk,Vector3 dir)
        {
            
            if(state != StateEntity.Stan)
            {
                state = StateEntity.Move;
                //anim movement

                moveFunk(dir);
            }
        }

        protected virtual void Stan()
        {
            timeStanCoolDown = stanCollDown;
            state = StateEntity.Stan;
            //animation stan
        }

        protected virtual void Idle()
        {
            state = StateEntity.Idle;
            //anim state
        }

        protected virtual void DeathObject()
        {
            Destroy(hpbar);
            Destroy(gameObject);
        }
        #endregion
        #region Unity Methods

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
        #endregion
        #endregion

        public enum StateEntity
        {
            Stan,
            Idle,
            Move,
            Sprint
        }

        public delegate void MoveFunk(Vector3 dir);
    }
}