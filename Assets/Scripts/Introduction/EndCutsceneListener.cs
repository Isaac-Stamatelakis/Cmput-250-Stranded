using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
using UnityEngine.UI;
using Rooms;

public class EndCutsceneListener : MonoBehaviour
{
    private PlayableDirector director;
    [SerializeField] private Button button;
    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(() => {
            LevelManager.getInstance().reset();
            SceneManager.LoadScene("TitleScene");
        });
        director = GetComponent<PlayableDirector>();
        director.stopped += onFinish;
    }

    public void onFinish(PlayableDirector playableDirector) {
        button.gameObject.SetActive(true);
    }

    public void Update() {
        if (Input.GetKey(KeyCode.Space)) {
            director.time += 5*Time.deltaTime;
        }
    }
}
