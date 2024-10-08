using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue {
    [CreateAssetMenu(fileName = "New Character", menuName = "Dialogue/Character")]
    public class Character : ScriptableObject
    {
        public Sprite sprite;
    }
}

