using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerModule {
    public class Player : MonoBehaviour
    {
        private static Player instance;
        public static Player Instance => instance;
        public DatePlayer DatePlayer;
        private bool inCutscene;
        private bool inDialog;
        public bool CanMove => !inCutscene && !inDialog;
        public void setCutscene(bool inCutscene) {
            this.inCutscene = inCutscene;
        }
        public void setDialog(bool inDialog) {
            this.inDialog = inDialog;
        }
        public void Awake() {
            instance = this;
        }
        public void SetPosition(Vector3 position) {
            this.transform.position = position;
            DatePlayer.transform.position = position;
        }

        public void Heal(float amount) {
            GetComponent<PlayerHealth>().Heal(amount);
        }
    }

}
