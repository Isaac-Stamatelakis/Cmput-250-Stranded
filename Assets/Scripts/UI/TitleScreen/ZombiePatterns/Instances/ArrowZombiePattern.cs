using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TitleScreen {
    public class ArrowZombiePattern : TitleScreenZombiePattern
    {
        public ArrowZombiePattern(Vector2Int direction, Vector2 xOffset, TitleScreenZombie prefab) : base(direction, xOffset, prefab)
        {
        }

        public override IEnumerator createPattern()
        {
            int size = 2;
            for (int y = 0; y <= size; y++) {
                TitleScreenZombie zombie = GameObject.Instantiate(prefab);
                zombie.initalize(5,direction);
                zombie.transform.position = position + new Vector2(0,2*y);
                
                if (y > 0) {
                    TitleScreenZombie zombie1 = GameObject.Instantiate(prefab);
                    zombie1.initalize(5,direction);
                    zombie1.transform.position = position + new Vector2(0,-2*y);
                }
                yield return new WaitForSeconds(0.25f);
                
            }
            yield return null;
        }
    }
}
