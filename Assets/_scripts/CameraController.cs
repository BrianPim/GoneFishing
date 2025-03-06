using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public ParticleSystem splash;

    public GameObject hook;
    private Camera cam;
    private float diff;

    public bool isUnderwater = false;

    public MeshRenderer distortion;
    public Material distort;
    private float distortAlpha;
    private bool isChangingAlpha = false;

    Coroutine co;
    
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        diff = cam.transform.position.y - hook.transform.position.y;

        distort = distortion.material;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        cam.transform.position = Vector3.Lerp(cam.transform.position, new Vector3(cam.transform.position.x, hook.transform.position.y + diff, cam.transform.position.z), Time.deltaTime * 2);

        if (!isUnderwater && transform.position.y < 0.5f)
        {
            if (!splash.isPlaying)
                splash.Play();

            if (true)
            {
                if (co != null)
                {
                    StopCoroutine(co);
                    isChangingAlpha = false;
                }

                isChangingAlpha = true;

                

                co = StartCoroutine("Distort", 1);
                
            }

            isUnderwater = true;
        }
        else if (isUnderwater && transform.position.y > 0.5f)
        {
            if (!splash.isPlaying)
                splash.Play();

            if(true)
            {
                if (co != null)
                {
                    StopCoroutine(co);
                    isChangingAlpha = false;
                }

                co = StartCoroutine("Distort", 0);
                isChangingAlpha = true;
            }

        isUnderwater = false;
        }
    }

    //changes alpha of distortion over time when entering or exiting water
    public virtual IEnumerator Distort(float newAlpha)
    {
        distortAlpha = distort.GetFloat("_Alpha");

        while (Mathf.Abs(distortAlpha - newAlpha) > 0.01f)
        {
            distortAlpha = Mathf.Lerp(distortAlpha, newAlpha, Time.deltaTime * 2f);

            distort.SetFloat("_Alpha", distortAlpha);

            yield return null;
        }

        isChangingAlpha = false;
    }
}
