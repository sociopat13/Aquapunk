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

        public Action onAttack;

        private bool isButtonPressed = false;

        private void Update()
        {
            if (isButtonPressed)
            {
                onAttack.Invoke();
            }
        }

        public void OnBeginAttack()
        {
            isButtonPressed = true;
        }

        public void OnEndAttack()
        {
            isButtonPressed = false;
        }
    }
}

