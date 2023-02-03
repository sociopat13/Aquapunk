using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    #region Fields
    public GameObject target;
    public Vector3 offset;
    public Image hpBar;
    #endregion
    #region Methods
    #region Class methods
    public void SetHP(float hp)
    {
        hpBar.fillAmount = hp;
    }
    #endregion
    #region Unity methods
    private void Update()
    {
        transform.position = target.transform.position + offset;
    }
    #endregion
    #endregion
}
