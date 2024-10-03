using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue {
    public abstract class DialogObject : ScriptableObject
    {
        public Character Speaker;
        [TextArea(10, 20)] public string Text;
    }
}

