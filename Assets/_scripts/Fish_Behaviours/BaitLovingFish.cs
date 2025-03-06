using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaitLovingFish : FishParent
{
    public HookController hook;

    public float left = -4f;
    public float right = 4f;

    public float target;

    public float senseBaitDistance = 5f;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        target = right;

        hook = FindObjectOfType<HookController>();
    }

    
    public override void Move()
    {
        bool hasBait = (hook.bait > 0);

    

        if (Vector2.Distance(hook.transform.position, transform.position) > senseBaitDistance)
        {
            Vector2 movePosition = transform.position;
            if (movePosition.x < left)
            {
                if (!isFlipped)
                {
                    if (isFlipping && co != null)
                    {
                        StopCoroutine(co);
                        isFlipping = false;
                    }

                    isFlipping = true;
                    co = StartCoroutine("Flip", 180);
                    isFlipped = true;
                }

                target = right;
                movePosition.x = Mathf.MoveTowards(transform.position.x, target, speed * Time.deltaTime);
                rb.AddForce(movePosition - (Vector2)transform.position, ForceMode2D.Impulse);
            }
            else if (movePosition.x > right)
            {
                if (isFlipped)
                {
                    if (isFlipping && co != null)
                    {
                        StopCoroutine(co);
                        isFlipping = false;
                    }

                    isFlipping = true;
                    co = StartCoroutine("Flip", 0);
                    isFlipped = false;
                }

                target = left;
                movePosition.x = Mathf.MoveTowards(transform.position.x, target, speed * Time.deltaTime);
                rb.AddForce(movePosition - (Vector2)transform.position, ForceMode2D.Impulse);
            }

            movePosition.x = Mathf.MoveTowards(transform.position.x, target, speed * Time.deltaTime);

            rb.AddForce(movePosition - (Vector2)transform.position, ForceMode2D.Force);
        }
        else
        {
            if (hasBait)
            {
                target = hook.transform.position.x;
            }
            else
            {
                if (transform.position.x < hook.transform.position.x)
                {
                    target = left * 2;
                }
                else
                {
                    target = right * 2;
                }
            }

            Vector2 movePosition = transform.position;
            if (movePosition.x < target)
            {
                if (!isFlipped && !isFlipping)
                {
                    isFlipping = true;
                    StartCoroutine("Flip", 180);
                    isFlipped = true;
                }

                movePosition.x = Mathf.MoveTowards(transform.position.x, target, speed * Time.deltaTime);
                rb.AddForce(movePosition - (Vector2)transform.position, ForceMode2D.Impulse);
            }
            else if (movePosition.x > target)
            {
                if (isFlipped && !isFlipping)
                {
                    isFlipping = true;
                    StartCoroutine("Flip", 0);
                    isFlipped = false;
                }

                movePosition.x = Mathf.MoveTowards(transform.position.x, target, speed * Time.deltaTime);
                rb.AddForce(movePosition - (Vector2)transform.position, ForceMode2D.Impulse);
            }

            movePosition.x = Mathf.MoveTowards(transform.position.x, target, speed * Time.deltaTime);

            rb.AddForce(movePosition - (Vector2)transform.position, ForceMode2D.Force);

        }
    }
}
