using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionSensor : MonoBehaviour
{
    PlayerPlatformerController player;
    
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerPlatformerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.CompareTag("Player"))
        {
            player.pickUpObject = transform.parent.GetComponent<PickUpObject>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            player.pickUpObject = null;
        }
    }
}
