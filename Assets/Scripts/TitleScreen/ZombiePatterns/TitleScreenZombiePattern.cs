using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TitleScreen {
    public abstract class TitleScreenZombiePattern
    {
        protected Vector2Int direction;
        protected Vector2 position;
        protected TitleScreenZombie prefab;

        public TitleScreenZombiePattern(Vector2Int direction, Vector2 position, TitleScreenZombie prefab)
        {
            this.direction = direction;
            this.position = position;
            this.prefab = prefab;
        }

        public abstract IEnumerator createPattern();
    }
}

