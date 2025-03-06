using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicHandler : MonoBehaviour
{
    public GameObject[] music;
    int currentlyPlaying;
    // Start is called before the first frame update
    void Start()
    {
        music[0].GetComponent<AudioSource>().Play();
        currentlyPlaying = 0;
    }

    public void PlayMusic(int musicIndex)
    {
        StartCoroutine(TransitionMusic(musicIndex));
    }

    IEnumerator TransitionMusic(int musicIndex)
    {
        music[musicIndex].GetComponent<AudioSource>().Play();
        music[musicIndex].GetComponent<AudioSource>().volume = 0f;

        for (int i = 0; i < 10; i++)
        {
            music[musicIndex].GetComponent<AudioSource>().volume += 0.1f;
            music[currentlyPlaying].GetComponent<AudioSource>().volume -= 0.1f;

            yield return new WaitForSeconds(0.05f);
        }
        
        music[currentlyPlaying].GetComponent<AudioSource>().Stop();
        currentlyPlaying = musicIndex;
    }
}
