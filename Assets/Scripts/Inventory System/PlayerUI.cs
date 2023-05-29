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
        public TextMeshProUGUI waterCounter;
        public HPBarUI hpbar;

        public Action OnAttackAction;
        public Action OnBeginAttackAction;
        public Action OnEndAttackAction;

        private bool isButtonPressed = false;

        private void Update()
        {
            if (isButtonPressed)
            {
                OnAttackAction.Invoke();
            }
        }

        public void OnBeginAttack()
        {
            isButtonPressed = true;
            OnBeginAttackAction.Invoke();
        }

        public void OnEndAttack()
        {
            isButtonPressed = false;
            OnEndAttackAction.Invoke();
        }
    }
}

