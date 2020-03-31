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
    public List<Light2D> lights;
    public GameObject myObject;
    public Sprite closedSprite;
    public Sprite openSprite;
    public int cannotOpenDialogueId = -1;
    public int openedDialogueId = -1;
    public RoomController room;
    [SerializeField] GameObject[] platforms;

    SpriteRenderer rend;
    Collider2D myCollider;
    //int keyIdUsed = -1;
    bool firstOpen = false;
    // Enemy[] myEnemies
    
    // Start is called before the first frame update
    void Start()
    {
        myCollider = GetComponent<Collider2D>();
        rend = GetComponent<SpriteRenderer>();
        rend.sprite = closedSprite;
        //myCollider.enabled = true;
        ToggleLights(false);
    }

    public bool CheckKey(KeyItems id)
    {
        return validKey == id;
    }

    public void Open(GameObject pickUp)
    {
        isOpen = true;

        if (room)
            room.FixRoom(isOpen);

        myObject = pickUp;
        rend.sprite = openSprite;
        myCollider.enabled = false;
        VictoryTracker.Instance.lockCount--;
        ToggleLights(isOpen);

        if (!firstOpen)
        {
            firstOpen = true;
            if (openedDialogueId >= 0)
                DialogueCreator.Instance.InitDialogue(openedDialogueId);
        }

        // is a staircase
        if (validKey == KeyItems.Toolbox)
            TogglePlatforms(isOpen);
    }

    public KeyItems Close()
    {
        if (myObject == null)
            return KeyItems.Invalid;
        
        isOpen = false;
        if (room)
            room.FixRoom(isOpen);
        myCollider.enabled = true;
        rend.sprite = closedSprite;
        ToggleLights(isOpen);

        //keyIdUsed = -1;
        VictoryTracker.Instance.lockCount++;

        Destroy(myObject);
        myObject = null;

        // is a staircase
        if (validKey == KeyItems.Toolbox)
            TogglePlatforms(isOpen);

        return validKey;
    }

    void ToggleLights(bool turnOn)
    {
        List<int> indicesToRemove = new List<int>();
        
        if(lights.Count != 0)
        {
            for(int i = 0; i < lights.Count; i++)
            {
                if (lights[i] == null)
                {
                    indicesToRemove.Add(i);
                    continue;
                }
                
                if(turnOn)
                    lights[i].enabled = true;
                else
                    lights[i].enabled = false;
            }
        }

        if (indicesToRemove.Count > 0)
        {
            foreach (int i in indicesToRemove)
            {
                lights.RemoveAt(i);
            }
        }
    }

    void TogglePlatforms(bool turnOn)
    {
        // avoid errors in case there's no platform
        if (platforms.Length < 1)
            return;
        
        foreach(GameObject go in platforms)
        {
            go.SetActive(turnOn);
        }
    }
}
