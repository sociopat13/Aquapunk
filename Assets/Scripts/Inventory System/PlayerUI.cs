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

        private int shotCount;
        private bool isRangeButtonPressed;
        private bool isMelleButtonPressed;

        private void Update()
        {
            if (player.switchProcess && player._timeAttackCoolDown <= 0)
            {
                OnAttackAction();
                shotCount++;
            }
            if(shotCount >= 3 && !isRangeButtonPressed && !isMelleButtonPressed)
            {
                OnEndAttackAction.Invoke();
            }
        }

        public void OnBeginRangeAttack()
        {
            isRangeButtonPressed = true;
            shotCount = 0;
            OnBeginRengeAttack.Invoke();
        }

        public void OnEndRangeAttack()
        {
            isRangeButtonPressed = false;
        }

        public void OnBeginAttack()
        {
            OnBeginAttackAction.Invoke();

            isMelleButtonPressed = true;
        }


        public void OnEndAttack()
        {
            isMelleButtonPressed = false;
        }

    }
}

