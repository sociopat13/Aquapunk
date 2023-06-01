using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Aquapunk
{
    public class PlayerUI : MonoBehaviour
    {
        public Player player;
        public TextMeshProUGUI waterCounter;
        public HPBarUI hpbar;

        public Action OnAttackAction;
        public Action OnBeginAttackAction;

        public Action OnEndAttackAction;

        public Action OnBeginRengeAttack;
        public Action OnRangeAttack;

        private bool isButtonPressed = false;
        private bool isRangeButtonPressed = false;

        private void Update()
        {
            if (player.switchProcess)
            {
                OnAttackAction();
            }
        }

        public void OnBeginRangeAttack()
        {
            isRangeButtonPressed = true;

            OnBeginRengeAttack.Invoke();
        }

        public void OnEndRangeAttack()
        {
            isRangeButtonPressed = false;
            OnEndAttackAction.Invoke();
            //StartCoroutine(EndRangeAttack());
        }

        private IEnumerator EndRangeAttack()
        {
            yield return new WaitForSeconds(2f);
            OnEndAttackAction.Invoke();
        }

        public void OnBeginAttack()
        {

            OnBeginAttackAction.Invoke();
        }


        public void OnEndAttack()
        {
            OnEndAttackAction.Invoke();
        }
    }
}

