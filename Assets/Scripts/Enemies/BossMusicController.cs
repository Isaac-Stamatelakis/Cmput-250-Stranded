using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMusicController : MonoBehaviour
{
    public AudioSource bossMusic;
    public float fadeDuration = 2f;
    public float startVolume = 0.2f;
    public float lowVolume = 0.4f;
    public float highVolume = 0.6f;

    private bool musicFadingOut = false;

    void Start()
    {
        bossMusic.Play();
        StartCoroutine(FadeMusicToVolume(startVolume));
    }

    public void StartDialogue()
    {
        StartCoroutine(FadeMusicToVolume(lowVolume));
    }

    public void EndDialogue()
    {
        StartCoroutine(FadeMusicToVolume(highVolume));
    }

    public void BossDefeated()
    {
        if (!musicFadingOut)
        {
            musicFadingOut = true;
            StartCoroutine(FadeMusicToVolume(0));
        }
    }

    private IEnumerator FadeMusicToVolume(float targetVolume)
    {
        float startVolume = bossMusic.volume;
        float elapsedTime = 0;

        while (elapsedTime < fadeDuration)
        {
            bossMusic.volume = Mathf.Lerp(startVolume, targetVolume, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        bossMusic.volume = targetVolume;
    }
}
