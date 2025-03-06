using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ExtraHook : MonoBehaviour
{
    private GameObject audioHolder;
    public HookController hook;

    private void Start()
    {
        if (hook == null) hook = transform.parent.gameObject.GetComponent<HookController>();
        audioHolder = hook.audioHolder;
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Fish"))
        {
            if (hook.hookedFish.Count < hook.hookCount && !hook.hookedFish.Contains(collision.gameObject.GetComponent<FishParent>()))
            {
                var fish = collision.gameObject;
                var script = fish.GetComponent<FishParent>();
                hook.hookedFish.Add(script);
                audioHolder.transform.Find("Rod_Caught").GetComponent<AudioSource>().Play();
                script.hooked = true;
                fish.transform.parent = transform;
            }
        }
    }

}
