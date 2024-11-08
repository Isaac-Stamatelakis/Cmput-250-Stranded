using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerModule;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Dialogue {
    public class CutsceneAnimator : MonoBehaviour
    {
        void Start()
        {
            Player.Instance.setCutscene(true);
            PlayableDirector director = GetComponent<PlayableDirector>();
            var output = director.playableAsset.outputs;
            foreach (var binding in output) {
                director.SetGenericBinding(binding.sourceObject, Player.Instance.GetComponent<Animator>());
                break;
            }
            director.stopped += onFinish;
        }
        
        public void onFinish(PlayableDirector playableDirector) {
            finishActions();
            Player.Instance.setCutscene(false);
            GameObject.Destroy(gameObject);
        }

        protected virtual void finishActions() {

        }



    }
}

