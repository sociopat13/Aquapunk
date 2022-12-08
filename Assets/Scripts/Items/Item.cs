using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

namespace Aquapunk
{
    public class Item : MonoBehaviour
    {
        #region Fields
        public int id;
        public new string name;

        [SerializeField] protected float _count;
        #endregion

        #region Properties
        public float Count
        {
            get { return _count; }
            set { _count = value; }
        }
        #endregion
        #region Methods
        #region Class Methods
        protected virtual void Obtaining(GameObject game)
        {
            game.GetComponent<Player>().Item = this;
            Destroy(gameObject);
        }
        #endregion
        #region Unity Methods
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Player>())
            {
                Obtaining(other.gameObject);
            }
                
        }
        #endregion
        #endregion
    }
}

