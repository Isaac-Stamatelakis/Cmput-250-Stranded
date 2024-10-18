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
            Player.Instance.GetComponent<Animator>().SetBool("isRight", true);
            StartCoroutine(hardCode());
            PlayableDirector director = GetComponent<PlayableDirector>();
            var output = director.playableAsset.outputs;
            foreach (var binding in output) {
                director.SetGenericBinding(binding.sourceObject, Player.Instance.GetComponent<Animator>());
                break;
            }
            
            
            director.stopped += onFinish;
        }

        private IEnumerator hardCode() {
            yield return new WaitForSeconds(6.5f);
            Player.Instance.GetComponent<Animator>().SetBool("isRight", false);
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

