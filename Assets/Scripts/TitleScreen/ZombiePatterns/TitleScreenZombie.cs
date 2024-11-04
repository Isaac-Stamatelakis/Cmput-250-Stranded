using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TitleScreen {
    public class TitleScreenZombie : MonoBehaviour
    {
        private float speed;
        private Vector3 direction;
        private float time = 0;
        public void initalize(float speed, Vector2Int direction) {
            this.speed = speed;
            this.direction = new Vector3(direction.x,direction.y,0);
            GetComponent<SpriteRenderer>().flipX = direction == Vector2Int.left;
        }

        public void Update() {
            gameObject.transform.position += direction * speed * Time.deltaTime;
            time += Time.deltaTime;
            if (time > 10) {
                GameObject.Destroy(gameObject);
            }
        }

        public void kill() {
            GameObject.Destroy(gameObject);
        }
    }

}
