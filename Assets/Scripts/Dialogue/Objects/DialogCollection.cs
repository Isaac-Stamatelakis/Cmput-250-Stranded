using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue {
    [CreateAssetMenu(fileName = "New Dialogue Collection", menuName = "Dialogue/Collection")]
    public class DialogCollection : ScriptableObject
    {
        public List<DialogObject> dialogs;
    }
}

