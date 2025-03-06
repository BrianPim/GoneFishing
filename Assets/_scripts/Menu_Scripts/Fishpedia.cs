using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Fishpedia : MonoBehaviour
{
    public GameObject window;
    public GameObject title;
    public GameObject moneyUI;
    public GameObject shop;
    public GameObject menu;
    public GameObject theMan;
    public GameObject template;
    public GameObject button;
    public GameObject acceptAudio;
    public GameObject selectAudio;
    public Sprite[] fish;
    public Sprite normal;
    public Sprite close;

    int width = Screen.width;

    public Dictionary<string,bool> caught;

    bool isOpen;

    Vector3 origPos;

    // Start is called before the first frame update
    void Start()
    {
        isOpen = false;
        window.SetActive(false);
        title.SetActive(false);
        caught = new Dictionary<string,bool>();
        origPos = button.transform.position;

        Transform pos = template.transform;
        Vector3 prev = pos.position;
        float origX = pos.position.x;
        for (int i = 0; i < fish.Length; i++)
        {
            GameObject item = Instantiate(template, pos);
            item.transform.SetParent(window.transform.GetChild(0).gameObject.transform);
            item.SetActive(true);
            item.transform.GetChild(2).gameObject.GetComponent<Image>().sprite = fish[i];

            if (item.transform.GetChild(2).gameObject.GetComponent<Image>().sprite.name.Length > 0)
                item.name = item.transform.GetChild(2).gameObject.GetComponent<Image>().sprite.name;
            else
                item.name = "Item " + (i+1).ToString();
            caught.Add(item.name, false);

            if (i%5 == 0 && i != 0)
                item.transform.position = new Vector3(origX,prev.y - width/6,0);
            else
                item.transform.position = new Vector3(prev.x,prev.y,0);
            prev = item.transform.position + new Vector3(width/6,0,0);
        }
    }

    public void Toggle()
    {
        if (isOpen == false)
        {
            window.SetActive(true);
            title.SetActive(true);
            shop.SetActive(false);
            menu.SetActive(false);
            theMan.SetActive(false);
            moneyUI.SetActive(false);
            button.GetComponent<Image>().sprite = close;
            selectAudio.GetComponent<AudioSource>().Play();
            button.transform.position = menu.transform.GetChild(0).gameObject.transform.position;
        }
        else
        {
            window.SetActive(false);
            title.SetActive(false);
            shop.SetActive(true);
            menu.SetActive(true);
            theMan.SetActive(true);
            moneyUI.SetActive(true);
            button.GetComponent<Image>().sprite = normal;
            acceptAudio.GetComponent<AudioSource>().Play();
            button.transform.position = origPos;
        }
        isOpen = !isOpen;
    }

    public void FoundFish(string fishName, int depth, int price, bool bait)
    {
        caught[fishName] = true;
        window.transform.GetChild(0).Find(fishName).gameObject.transform.GetChild(2).gameObject.GetComponent<Image>().color = Color.white;
        window.transform.GetChild(0).Find(fishName).gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = FirstLetterToUpper(fishName);
        window.transform.GetChild(0).Find(fishName).gameObject.transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>().text = "Depth Level " + depth;
        window.transform.GetChild(0).Find(fishName).gameObject.transform.GetChild(5).gameObject.GetComponent<TextMeshProUGUI>().text = price.ToString();
        window.transform.GetChild(0).Find(fishName).gameObject.transform.GetChild(5).gameObject.SetActive(true);
        window.transform.GetChild(0).Find(fishName).gameObject.transform.GetChild(4).gameObject.SetActive(true);

        if (bait)
            window.transform.GetChild(0).Find(fishName).gameObject.transform.GetChild(6).gameObject.SetActive(true);
    }

    public string FirstLetterToUpper(string str)
{
    if (str == null)
        return null;

    if (str.Length > 1)
        return char.ToUpper(str[0]) + str.Substring(1);

    return str.ToUpper();
}
}
