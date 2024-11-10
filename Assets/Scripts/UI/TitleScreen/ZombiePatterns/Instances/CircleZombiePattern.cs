using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TitleScreen {
    public class CirlceZombiePattern : TitleScreenZombiePattern
    {
        public CirlceZombiePattern(Vector2Int direction, Vector2 xOffset, TitleScreenZombie prefab) : base(direction, xOffset, prefab)
        {
        }

        public override IEnumerator createPattern()
        {
            int size = 2;
            for (int r = -size; r <= size; r++)
            {
                int y = size - Mathf.Abs(r);
                int numberOfZombies = 2 * y + 1;


                for (int i = 0; i < numberOfZombies; i++)
                {
                    TitleScreenZombie zombie = GameObject.Instantiate(prefab);
                    zombie.initalize(5, direction);
                    Vector2 zombiePosition = position + new Vector2(0, 2*(y-i));
                    zombie.transform.position = zombiePosition;
                }

                yield return new WaitForSeconds(0.25f);
            }
            yield return null;
        }
    }
}
