using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TitleScreen {
    
    public class PlayerMouseTitleScreen : MonoBehaviour
    {
        int enemyLayer;
        public void Start() {
            enemyLayer = 1<< LayerMask.NameToLayer("Enemy");
        }
        void Update()
        {
            if (!Input.GetMouseButtonDown(0)) {
                return;
            }
            Vector3 mousePosition = Input.mousePosition;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition); 
            RaycastHit2D hit = Physics2D.Raycast(worldPosition,Vector2.zero,Mathf.Infinity,enemyLayer);
            if (hit.collider != null) {
                TitleScreenZombie titleScreenZombie = hit.collider.GetComponent<TitleScreenZombie>();
                if (titleScreenZombie == null) {
                    return;
                }
                titleScreenZombie.kill();
            }
        }
    }
}

