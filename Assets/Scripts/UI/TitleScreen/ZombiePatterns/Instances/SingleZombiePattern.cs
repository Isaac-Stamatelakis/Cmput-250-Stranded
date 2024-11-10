using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TitleScreen {
    public class SingleZombiePattern : TitleScreenZombiePattern
    {
        public SingleZombiePattern(Vector2Int direction, Vector2 position, TitleScreenZombie prefab) : base(direction, position, prefab)
        {
        }

        public override IEnumerator createPattern()
        {
            TitleScreenZombie element = GameObject.Instantiate(prefab);
            element.transform.position = position;
            element.initalize(5,direction);
            yield return null;
        }
        
    }
}
