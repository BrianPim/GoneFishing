using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    int cloudLimit = 10;

    public List<CloudController> clouds = new List<CloudController>();
    
    public GameObject cloud;
    public Sprite[] cloudTypes;
    void Start()
    {
        for (int i = 0; i < cloudLimit; i++)
        {
            var c = Instantiate(cloud);
            c.transform.parent = transform;
            c.GetComponent<SpriteRenderer>().sprite = cloudTypes[(int)Random.Range(0, cloudTypes.Length)];
            c.transform.localPosition = new Vector3(Random.Range(-100, 100), Random.Range(0, 20), Random.Range(-10, 30));
            clouds.Add(c.GetComponent<CloudController>());

        }
    }

    private void Update()
    {
        if (clouds.Count < cloudLimit)
        {
            var c = Instantiate(cloud);
            c.transform.parent = transform;
            c.GetComponent<SpriteRenderer>().sprite = cloudTypes[(int)Random.Range(0, 3)];
            float[] xOptions = { -99, 99 };
            c.transform.localPosition = new Vector3(xOptions[Random.Range(0,2)], Random.Range(0, 20), Random.Range(-10, 30));
            clouds.Add(c.GetComponent<CloudController>());
        }

    }


}
