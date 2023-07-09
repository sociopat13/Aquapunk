using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Aquapunk
{

    public class SwitchSceneScript : MonoBehaviour
    {
        public string sceneName;
        public GameObject switchScenePanel;
        public GameObject playerUI;

        public void Yes()
        {
            SceneManager.LoadScene(sceneName);
        }

        public void No()
        {
            switchScenePanel.SetActive(false);
            playerUI.SetActive(true);
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Player>())
            {
                playerUI.SetActive(false);
                switchScenePanel.SetActive(true);
            }
        }
    }
}
