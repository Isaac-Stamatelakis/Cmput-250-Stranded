using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TitleScreen {
    public enum ZombiePattern {
        Single,
        Arrow,
        Circle,
        Line
    }
    public class TitleScreenZombieController : MonoBehaviour
    {
        [SerializeField] private TitleScreenZombie zombiePrefab;
        [SerializeField] private GameObject datePlayer;
        [SerializeField] private GameObject player;
        public static readonly int SCREEN_WIDTH = 32;
        public static readonly int SCREEN_HEIGHT = 14;
        public static readonly int SCREEN_HEIGHT_OFFSET = 4;
        
        public int MIN_SPAWN_TIME = 25;
        public int SPAWN_TIME = 500;
        private int counter;
        public int SPAWN_TIME_DEREASE = 10;
        int spawns = 0;
        public void Start() {
            counter = SPAWN_TIME;
        }

        public void FixedUpdate() {
            counter ++;
            if (counter < SPAWN_TIME-SPAWN_TIME_DEREASE*spawns || counter < MIN_SPAWN_TIME) {
                return;
            }
            TitleScreenZombiePattern pattern = createRandomPattern();
            StartCoroutine(pattern.createPattern());
            counter = 0;
            spawns++;
        }

        private TitleScreenZombiePattern createRandomPattern() {
            // Randomly picks between left and right
            int dir = UnityEngine.Random.Range(0,2);
            if (dir == 0) {
                dir = -1;
            }
            Vector2Int direction = new Vector2Int(dir,0);

            ZombiePattern patternType = ZombiePatternFactory.getRandomType();
            
            int yOffset = UnityEngine.Random.Range(-SCREEN_HEIGHT/2+SCREEN_HEIGHT_OFFSET,SCREEN_HEIGHT/2-SCREEN_HEIGHT_OFFSET+1); // add one as exclusive
            return ZombiePatternFactory.createPattern(patternType,direction,new Vector2(-dir* SCREEN_WIDTH/2,yOffset),zombiePrefab);
        }
    }
}

