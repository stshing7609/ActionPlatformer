using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryTrigger : MonoBehaviour
{
    int lockId = 3;
    int winId = 29;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            int show = -1;
            if (VictoryTracker.Instance.lockCount <= 0)
                show = winId;
            else
                show = lockId;

            
            DialogueCreator.Instance.InitDialogue(show);
        }
    }
}
