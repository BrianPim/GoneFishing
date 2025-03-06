using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FishParent : MonoBehaviour
{
    public readonly float value;

    public Rigidbody2D rb;

    public float speed;

    public SpriteRenderer sr;
    public BoxCollider2D bc2d;

    GameObject lehook;

    public bool isFlipping = false;
    public bool isFlipped = false;

    public bool hooked = false;
    public int Price = 10;
    public int DepthSpawnedAt = 1;

    public Coroutine co;

    // Start is called before the first frame update
    public virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        bc2d = GetComponent<BoxCollider2D>();
        lehook = GameObject.FindGameObjectsWithTag("hook")[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (!hooked) Move();
        else
        {
            rb.velocity = Vector2.zero;

            transform.localRotation = new Quaternion(transform.localRotation.x, transform.localRotation.y, -90, 90);


            //Vector3 mouth = bc2d.bounds.center;
            Vector3 mouth = new Vector3(lehook.transform.position.x, lehook.transform.position.y,0);
            transform.position = mouth;
            sr.flipX = false;
        }
    }

    public abstract void Move();

    public virtual IEnumerator Flip(float rotDegrees)
    {
        while (Mathf.Abs(transform.rotation.eulerAngles.y - rotDegrees) > 1f)
        {
            Quaternion targetRot = Quaternion.AngleAxis(rotDegrees, transform.up);

            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, speed * Time.deltaTime);

            yield return null;
        }

        isFlipping = false;
    }
}
