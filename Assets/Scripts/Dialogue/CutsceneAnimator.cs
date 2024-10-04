using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerModule;
using UnityEngine.Playables;

namespace Dialogue {
    public class CutsceneAnimator : MonoBehaviour
    {
        void Start()
        {
            Player.Instance.setCutscene(true);
            PlayableDirector director = GetComponent<PlayableDirector>();
            director.stopped += onFinish;
        }

        public void onFinish(PlayableDirector playableDirector) {
            Player.Instance.setCutscene(false);
            GameObject.Destroy(gameObject);
        }



    }
}

