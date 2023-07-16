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

        public Action melleAttack;
        public Action rangeAttack;
        public Action onBeginRangeAttackAction;

        public bool isButtonMelleAttackPressed;
        public bool isButtonRangeAttackPressed;

        public bool isRangeAttack;
        [SerializeField]private float shotCount;
        private void Update()
        {
            if (isButtonMelleAttackPressed && !player.isAttack && player._timeAttackCoolDown <= 0)
            {
                melleAttack.Invoke();
            }

            if (shotCount >= 3 && !isButtonRangeAttackPressed && isRangeAttack && player.endShot)
            {
                isRangeAttack = false;
                player.Unarmed();
            }
            if (isRangeAttack && player._timeAttackCoolDown <= 0)
            {
                rangeAttack.Invoke();
                if (shotCount != 3)
                {
                    shotCount+=1;
                }
            }
        }

        public void OnBeginRangeAttack()
        {
            onBeginRangeAttackAction.Invoke();
            isRangeAttack = true;
            shotCount = 0;
            isButtonRangeAttackPressed = true;
        }

        public void OnEndRangeAttack()
        {
            isButtonRangeAttackPressed = false;
        }

        public void OnBeginAttack()
        {
            isButtonMelleAttackPressed = true;
        }


        public void OnEndAttack()
        {
            isButtonMelleAttackPressed = false;

        }

    }
}

