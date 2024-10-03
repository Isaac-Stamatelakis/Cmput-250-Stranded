using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue {
    [CreateAssetMenu(fileName = "New Dialogue Tree", menuName = "Dialogue/Tree")]
    public class DialogueTree : DialogObject
    {    
        public List<DialogResponse> responses;
    }

    [System.Serializable]
    public class DialogResponse {
        [TextArea(10, 20)] public string Text;
        public DialogObject dialog;
    }
}

