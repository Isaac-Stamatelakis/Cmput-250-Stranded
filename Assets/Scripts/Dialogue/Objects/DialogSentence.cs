using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue {
    [CreateAssetMenu(fileName = "New Dialogue Sentence", menuName = "Dialogue/Sentence")]
    public class DialogSentence : DialogObject
    {
        public DialogObject nextDialog;
    }
}

