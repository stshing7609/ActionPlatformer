using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// valid keys
// sequence
// enemy held
// broken state

public class LockObject : MonoBehaviour
{
    public bool isOpen = false;
    Collider2D myCollider;
    public int[] validKeys;
    public PickUpObject myObject;
    public Sprite closedSprite;
    public Sprite openSprite;
    public int cannotOpenDialogueId = -1;
    public int openedDialogueId = -1;
    SpriteRenderer rend;

    public int keyIdUsed { get; private set; } = -1;
    bool firstOpen = false;
    // Enemy[] myEnemies
    
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        rend.sprite = closedSprite;
        myCollider = GetComponent<Collider2D>();
        //validKeys = new int[] { 0, 1, 2 };
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
                    rend.sprite = openSprite;
                    myCollider.enabled = false;

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
        myCollider.enabled = true;
        rend.sprite = closedSprite;

        keyIdUsed = -1;

        GameObject temp = myObject.gameObject;
        myObject = null;

        return temp;
    }
}
