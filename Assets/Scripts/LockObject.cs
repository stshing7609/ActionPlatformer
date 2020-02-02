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
    public Collider2D myCollider;
    public int[] validKeys;
    public PickUpObject myObject;
    public Sprite closedSprite;
    public Sprite openSprite;
    SpriteRenderer renderer;


    int keyIdUsed = -1;
    bool firstOpen = false;
    // Enemy[] myEnemies
    
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = closedSprite;
        validKeys = new int[] { 0, 1, 2 };
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
                    renderer.sprite = openSprite;
                    myCollider.enabled = false;

                    if (!firstOpen)
                    {
                        firstOpen = true;
                        // trigger text
                    }

                    return keyIdUsed;
                }
            }
        }

        return -1;
    }

    public int Close()
    {
        isOpen = false;
        myCollider.enabled = true;
        renderer.sprite = closedSprite;

        int temp = keyIdUsed;
        keyIdUsed = -1;

        myObject.DestroyIt();

        return temp;
    }
}
