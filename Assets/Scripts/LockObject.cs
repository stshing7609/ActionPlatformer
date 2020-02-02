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
    Animator anim;
    int keyIdUsed = -1;
    // Enemy[] myEnemies
    
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
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
                    anim.SetTrigger("Open");

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
        anim.SetTrigger("Close");

        int temp = keyIdUsed;
        keyIdUsed = -1;

        myObject.DestroyIt();

        return temp;
    }

    public void Hide()
    {
        myCollider.enabled = false;
    }
}
