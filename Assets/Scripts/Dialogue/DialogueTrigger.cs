using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public int id;
    public bool onlyOnce;
    bool triggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (onlyOnce && triggered)
            return;
        
        if (collision.transform.CompareTag("Player"))
        {
            triggered = true;
            DialogueCreator.Instance.InitDialogue(id);
        }
    }
}
