using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace PlayerModule {
    public class PlayerDeathScreenUI : MonoBehaviour
    {
        [SerializeField] private Button button;
        public void Start() {
            button.onClick.AddListener(() => {
                int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
                SceneManager.LoadScene(currentSceneIndex);
            });
        }
    }
}

