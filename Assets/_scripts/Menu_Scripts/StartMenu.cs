using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    public GameObject acceptAudio;
    public GameObject selectAudio;
    public GameObject audioHolder;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < gameObject.transform.parent.gameObject.transform.childCount; i++)
        {
            if (gameObject.transform.parent.gameObject.transform.GetChild(i).gameObject != this.gameObject)
                gameObject.transform.parent.gameObject.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void Continue()
    {
        for(int i = 0; i < gameObject.transform.parent.gameObject.transform.childCount; i++)
        {
            if (gameObject.transform.parent.gameObject.transform.GetChild(i).gameObject == this.gameObject || i == 0)
                gameObject.transform.parent.gameObject.transform.GetChild(i).gameObject.SetActive(false);
            else
                gameObject.transform.parent.gameObject.transform.GetChild(i).gameObject.SetActive(true);
        }
        selectAudio.GetComponent<AudioSource>().Play();
        audioHolder.GetComponent<MusicHandler>().PlayMusic(1);
    }

    public void Quit()
    {
        acceptAudio.GetComponent<AudioSource>().Play();
        Application.Quit();
    }
}
