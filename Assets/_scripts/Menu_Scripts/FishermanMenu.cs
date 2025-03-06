using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class FishermanMenu : MonoBehaviour
{
    public GameObject menu;
    public bool isOpen = false;
    public string desiredFish;
    private HookController hook;
    public GameObject[] Fish;
    public GameObject reqFish;
    public Sprite[] talkFrames;
    public GameObject theMan;
    public GameObject button;
    public GameObject acceptAudio;
    public GameObject grunt;
    public GameObject fishpedia;
    public Sprite normal;
    public Sprite close;
    void Start()
    {
        menu.SetActive(false);
        hook = GameObject.Find("hook").GetComponent<HookController>();
        Fish = GameObject.Find("FishSpawner").GetComponent<FishSpawner>().Fish;
        NewFish();
    }

    public void ToggleStore()
    {
        if (isOpen == false)
        {
            menu.SetActive(true);
            button.GetComponent<Image>().sprite = close;
            grunt.GetComponent<AudioSource>().Play();
        }
        else
        {
            menu.SetActive(false);
            button.GetComponent<Image>().sprite = normal;
            acceptAudio.GetComponent<AudioSource>().Play();
        }
        isOpen = !isOpen;
    }

    public void NewFish()
    {
        GameObject chosenFish;
        do
        {
            GameObject[] potFish = Fish.Where(x => x.GetComponent<FishParent>().DepthSpawnedAt <= hook.rodLevel).ToArray();
            chosenFish = potFish[UnityEngine.Random.Range(0, potFish.Count())];
        } while (chosenFish.name == desiredFish);
        var sprite = chosenFish.GetComponent<SpriteRenderer>().sprite;
        var box = reqFish;
        var boxImg = box.GetComponent<Image>();
        boxImg.sprite = sprite;
	    desiredFish = chosenFish.name;
        if (fishpedia.GetComponent<Fishpedia>().caught[chosenFish.gameObject.name] == false)
            boxImg.color = Color.black;
        else
            boxImg.color = Color.white;
    }

    int spriteState = 0;
    private float nextUpdate = 0.12f;
    private void Update()
    {
        if(Time.time >= nextUpdate)
        {
            nextUpdate = (Time.time+0.12f);
            spriteState = Math.Abs(spriteState - 1);
            theMan.GetComponent<Image>().sprite = talkFrames[spriteState];
        }
    }
}
