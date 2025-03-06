using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Shop : MonoBehaviour
{
    public GameObject shopButton;
    public GameObject shopWindow;
    public GameObject fishpediaUI;
    public GameObject pauseUI;
    public GameObject hook;
    public GameObject container;
    public GameObject sinkPriceLabel;
    public GameObject rodNameLabel;
    public GameObject rodPriceLabel;
    public GameObject poorPersonWarn;
    public GameObject acceptAudio;
    public GameObject selectAudio;
    public GameObject plopAudio;
    public GameObject rodObj;
    public GameObject secondHook;
    public GameObject hookButton;
    public Sprite[] rods;

    public Sprite normal;
    public Sprite close;

    int sinkPrice;
    int rodPrice;

    bool isOpen;

    Vector3 origPos;
    // Start is called before the first frame update
    void Start()
    {
        shopWindow.SetActive(false);
        poorPersonWarn.SetActive(false);
        isOpen = false;
        sinkPrice = 100;
        rodPrice = 200;
        origPos = shopButton.transform.position;
    }

    public void ToggleStore()
    {
        if (isOpen == false)
        {
            shopWindow.SetActive(true);
            fishpediaUI.SetActive(false);
            pauseUI.SetActive(false);
            shopButton.GetComponent<Image>().sprite = close;
            selectAudio.GetComponent<AudioSource>().Play();
            shopButton.transform.position = pauseUI.transform.GetChild(0).gameObject.transform.position;
        }
        else
        {
            shopWindow.SetActive(false);
            fishpediaUI.SetActive(true);
            pauseUI.SetActive(true);
            shopButton.GetComponent<Image>().sprite = normal;
            acceptAudio.GetComponent<AudioSource>().Play();
            shopButton.transform.position = origPos;
        }
        isOpen = !isOpen;
    }

    public void PurchaseItem(string itemName)
    {
        if (itemName == "bait" && hook.GetComponent<HookController>().money >= 10)
        {
            hook.GetComponent<HookController>().bait++;
            hook.GetComponent<HookController>().money -= 10;
        }
        else if (itemName == "bait10" && hook.GetComponent<HookController>().money >= 90)
        {
            hook.GetComponent<HookController>().bait += 10;
            hook.GetComponent<HookController>().money -= 90;
        }
        else if (itemName == "sinkFaster" && hook.GetComponent<HookController>().money >= sinkPrice)
        {
            hook.GetComponent<HookController>().baseSinkSpeed += 0.25f;
            hook.GetComponent<HookController>().money -= sinkPrice;
            sinkPrice = sinkPrice*2;
            sinkPriceLabel.GetComponent<TextMeshProUGUI>().text = "$" + sinkPrice.ToString();
        }
        else if (itemName == "flashlight" && hook.GetComponent<HookController>().money >= 500)
        {
            hook.transform.GetChild(0).gameObject.SetActive(true);
            hook.GetComponent<HookController>().flashlight = true;
            hook.GetComponent<HookController>().money -= 500;
            container.transform.GetChild(4).gameObject.transform.GetChild(3).gameObject.SetActive(false);
        }
        else if (itemName == "rod" && hook.GetComponent<HookController>().money >= rodPrice)
        {
            hook.GetComponent<HookController>().rodLevel++;
            hook.GetComponent<HookController>().money -= rodPrice;
            rodPrice = rodPrice*2;

            if (hook.GetComponent<HookController>().rodLevel <= 3)
                rodPriceLabel.GetComponent<TextMeshProUGUI>().text = "$" + rodPrice.ToString();
            
            if (hook.GetComponent<HookController>().rodLevel == 2)
            {
                rodNameLabel.GetComponent<TextMeshProUGUI>().text = "Great Rod";
                rodObj.GetComponent<SpriteRenderer>().sprite = rods[0];
            }
            else if (hook.GetComponent<HookController>().rodLevel == 3)
            {
                rodNameLabel.GetComponent<TextMeshProUGUI>().text = "Super Rod";
                rodObj.GetComponent<SpriteRenderer>().sprite = rods[1];
            }
            else if (hook.GetComponent<HookController>().rodLevel == 4)
            {
                container.transform.GetChild(2).gameObject.transform.GetChild(3).gameObject.SetActive(false);
                rodObj.GetComponent<SpriteRenderer>().sprite = rods[2];
            }
        }
        else if (itemName =="hook" && hook.GetComponent<HookController>().money >= 50)
        {
            hook.GetComponent<HookController>().money -= 50;
            hook.GetComponent<HookController>().hookCount = 2;
            secondHook.SetActive(true);
            container.transform.GetChild(5).gameObject.transform.GetChild(3).gameObject.SetActive(false);
            hookButton.GetComponent<Image>().color = Color.grey;
        }
        else
            StartCoroutine(warning());

        hook.GetComponent<HookController>().UpdateResourcesUI();
        plopAudio.GetComponent<AudioSource>().Play();
    }

    IEnumerator warning()
    {
        poorPersonWarn.SetActive(true);
        yield return new WaitForSeconds(1f);
        poorPersonWarn.SetActive(false);
    }
}
