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
    Collider2D myCollider;
    public int[] validKeys;
    public Light2D[] lights;
    public PickUpObject myObject;
    public Sprite closedSprite;
    public Sprite openSprite;
    public int cannotOpenDialogueId = -1;
    public int openedDialogueId = -1;
    SpriteRenderer rend;

    public RoomController room;

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

    // takes in a set of keys
    // returns the index of the key used
    public int Open(List<int> ids)
    {
        for(int i = 0; i < ids.Count; i++)
        {
            for(int j = 0; j < validKeys.Length; j++)
            {
                if (ids[i] == validKeys[j])
                {
                    keyIdUsed = ids[i];

                    isOpen = true;
                    if(room)
                        room.FixRoom(isOpen);
                    rend.sprite = openSprite;
                    myCollider.enabled = false;
                    VictoryTracker.Instance.lockCount--;
                    EnableLights();

                    if (!firstOpen)
                    {
                        firstOpen = true;
                        if(openedDialogueId >= 0)
                            DialogueCreator.Instance.InitDialogue(openedDialogueId);
                    }

                    return keyIdUsed;
                }
            }
        }
        
        DialogueCreator.Instance.InitDialogue(cannotOpenDialogueId);

        return -1;
    }

    public GameObject Close()
    {
        isOpen = false;
        if (room)
            room.FixRoom(isOpen);
        myCollider.enabled = true;
        rend.sprite = closedSprite;
        DisableLights();

        keyIdUsed = -1;
        VictoryTracker.Instance.lockCount++;

        GameObject temp = myObject.gameObject;
        myObject = null;

        return temp;
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
