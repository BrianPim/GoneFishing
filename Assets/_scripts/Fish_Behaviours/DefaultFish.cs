using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultFish : FishParent
{
    public float left = -4f;
    public float right = 4f;

    public float target;

  
    

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        target = left;
    }

    
    public override void Move()
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
                StopCoroutine(co);

                co = StartCoroutine("Flip", 0);
                isFlipped = false;
            }

            target = left;
            movePosition.x = Mathf.MoveTowards(transform.position.x, target, speed * Time.deltaTime);
            rb.AddForce(movePosition - (Vector2)transform.position, ForceMode2D.Impulse);
        }

        movePosition.x = Mathf.MoveTowards(transform.position.x, target, speed * Time.deltaTime);

        rb.AddForce(movePosition - (Vector2)transform.position, ForceMode2D.Force);

        /* if (rb.velocity.x > 0 && !isFlipped && !isFlipping)
            {
                isFlipping = true;
                StartCoroutine("Flip", 180);
                isFlipped = true;
            }
            else if (rb.velocity.x < 0 && isFlipped && !isFlipping)
            {
                isFlipping = true;
                StartCoroutine("Flip", 0);
                isFlipped = false;
            }*/

    }
}
