using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject window;
    public GameObject button;
    public GameObject shopUI;
    public GameObject fishpediaUI;
    public GameObject acceptAudio;
    public GameObject selectAudio;
    public GameObject theMan;
    public Sprite normal;
    public Sprite close;
    bool isOpen;
    // Start is called before the first frame update
    void Start()
    {
        isOpen = false;
        window.SetActive(false);
    }

    public void Toggle()
    {
        if (isOpen == false)
        {
            window.SetActive(true);
            shopUI.SetActive(false);
            fishpediaUI.SetActive(false);
            theMan.SetActive(false);
            button.GetComponent<Image>().sprite = close;
            selectAudio.GetComponent<AudioSource>().Play();
        }
        else
        {
            window.SetActive(false);
            shopUI.SetActive(true);
            fishpediaUI.SetActive(true);
            theMan.SetActive(true);
            button.GetComponent<Image>().sprite = normal;
            acceptAudio.GetComponent<AudioSource>().Play();
        }
        isOpen = !isOpen;
    }

    public void Continue()
    {
        window.SetActive(false);
        shopUI.SetActive(true);
        fishpediaUI.SetActive(true);
        theMan.SetActive(true);
        button.GetComponent<Image>().sprite = normal;
        isOpen = false;
        selectAudio.GetComponent<AudioSource>().Play();
    }

    public void Quit()
    {
        Application.Quit();
        acceptAudio.GetComponent<AudioSource>().Play();
    }
}
