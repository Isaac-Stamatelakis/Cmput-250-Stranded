using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TitleScreen {
    public class LineZombiePattern : TitleScreenZombiePattern
    {
        public LineZombiePattern(Vector2Int direction, Vector2 position, TitleScreenZombie prefab) : base(direction, position, prefab)
        {
        }

        public override IEnumerator createPattern()
        {
            int width = 2;
            for (int y = -width; y <= width; y++) {
                TitleScreenZombie zombie = GameObject.Instantiate(prefab);
                zombie.initalize(5,direction);
                zombie.transform.position = position + new Vector2(0,2*y);
            }
            yield return null;
        }
    }
}
