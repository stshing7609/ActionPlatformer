using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrittlePlatform : DestructiblePlatform
{
    void Start()
    {
        targetColor = new Color(1, 1, 1, 0);
        permanentDestroy = false;
        timeToBreak = .5f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            if(collision.transform.GetComponent<PlayerPlatformerController>().SlowMoving)
            {
                //Do rumble animation
            }
            else
                StartCoroutine(DestroySequence(timeToBreak));
        }
    }

    // Make rumble animation function
}
