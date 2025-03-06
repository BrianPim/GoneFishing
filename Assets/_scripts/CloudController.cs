using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudController : MonoBehaviour
{
    public float xMove = 0;
    CloudSpawner cs;
    private void Start()
    {
        cs = GetComponentInParent<CloudSpawner>();
        while (xMove < 1 && xMove > -1)
            xMove = Random.Range(-4f, 4f);
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x + (xMove * Time.deltaTime), transform.position.y, transform.position.z);
        if (transform.localPosition.x < -100 || transform.localPosition.x > 100)
        {
            cs.clouds.Remove(this);
            Destroy(gameObject);
        }

    }
}
