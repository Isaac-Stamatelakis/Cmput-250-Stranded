using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class BackgroundIntroductionListener : MonoBehaviour
{
    private PlayableDirector director;
    // Start is called before the first frame update
    void Start()
    {
        director = GetComponent<PlayableDirector>();
        director.stopped += onFinish;
    }

    public void onFinish(PlayableDirector playableDirector) {
        SceneManager.LoadScene("LevelScene");
    }

    public void Update() {
        if (Input.GetKey(KeyCode.Space)) {
            director.time += 5*Time.deltaTime;
        }
    }
    
}
