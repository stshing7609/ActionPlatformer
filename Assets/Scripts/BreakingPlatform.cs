using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakingPlatform : DestructiblePlatform
{
    void Start()
    {
        targetColor = new Color(1, 1, 1, 0);
        permanentDestroy = false;
        timeToBreak = .75f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(DestroySequence(timeToBreak));
    }
}
