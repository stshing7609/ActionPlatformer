using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

// valid keys
// sequence
// enemy held
// broken state

public class LockObject : MonoBehaviour
{
    public bool isOpen = false;
    public KeyItems validKey;
    public Light2D[] lights;
    public PickUpObject myObject;
    public Sprite closedSprite;
    public Sprite openSprite;
    public int cannotOpenDialogueId = -1;
    public int openedDialogueId = -1;
    SpriteRenderer rend;

    Collider2D myCollider;
    int keyIdUsed = -1;
    bool firstOpen = false;
    // Enemy[] myEnemies
    
    // Start is called before the first frame update
    void Start()
    {
        myCollider = GetComponent<Collider2D>();
        rend = GetComponent<SpriteRenderer>();
        rend.sprite = closedSprite;
        //myCollider.enabled = true;
        DisableLights();
    }

    public void Open()
    {
        isOpen = true;
        rend.sprite = openSprite;
        myCollider.enabled = false;
        VictoryTracker.Instance.lockCount--;
        EnableLights();

        if (!firstOpen)
        {
            firstOpen = true;
            if (openedDialogueId >= 0)
                DialogueCreator.Instance.InitDialogue(openedDialogueId);
        }
    }

    public KeyItems Close()
    {
        isOpen = false;
        myCollider.enabled = true;
        rend.sprite = closedSprite;
        DisableLights();

        keyIdUsed = -1;
        VictoryTracker.Instance.lockCount++;

        myObject = null;

        return validKey;
    }

    void DisableLights()
    {
        if(lights.Length != 0)
        {
            foreach(Light2D light in lights)
                light.enabled = false;
        }
    }

    void EnableLights()
    {
        if (lights.Length != 0)
        {
            foreach (Light2D light in lights)
            {
                light.enabled = true;
            }
        }
    }
}
