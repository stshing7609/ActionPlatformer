using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saver : MonoBehaviour
{
    /* Need to save:
     * players location
     * Player's inventory
     * lock object location 
     * lock object's open/close status
     * if lock object is open, it needs to know which key was used
     * pick up objects position
     * 
     * Each object needs to have a dependency structure, where if you reset one,
     * you have to reset the others.
     */
    // Start is called before the first frame update

    public Transform player;
    private Inventory inventory;
    public List<LockObject> locks = new List<LockObject>; // Must add each lock to this to track their states.
    public List<GameObject> keys = new List<GameObject>; // Must add each key to this to track their states.
    private List<int> itemIDs;
    void Start()
    {
        inventory = GetComponent<Inventory>();
        itemIDs = new List<int>();
    }

    // Update is called once per frame
    void Update()
    {
        itemIDs = ExtractIDs(inventory.Items);
    }

    public List<int> ExtractIDs(List<GameObject> goList)
    {
        List<int> localIDs = new List<int>();
        foreach (GameObject go in goList)
        {
            int goID = go.GetInstanceID();
        }
        return localIDs;
    }

    public void reloadInventory()
    {
        List<int> keysToAdd = new List<int>();
        foreach (LockObject lockObject in locks) {
            if (!(lockObject.keyIdUsed == -1) && !lockObject.isOpen) // Lock is closed and has a key in it.
            {
                // Get the key object and add it to inventory.
                int keyID = lockObject.keyIdUsed;
            }
            foreach (GameObject key in keys)
            {
                if (keysToAdd.Contains(key.GetInstanceID())) {
                    inventory.AddItem(key);
                }
            }


            //if (!pickupObjects.Contains(key) && !lockObject.isOpen && lockObject.firstOpen) {
            // You don't have the key and the door is open and the door has been opened before
            // so we give you back the key and close the door
            //    inventory.AddItem
        }

    }

    public void DropInventory()
    /* Drops all items where the player currently stands;
     */
    {
        Vector2 currentPosition = new Vector2(player.position.x, player.position.y);
        foreach (GameObject item in inventory.Items)
        {
            inventory.DropItem(currentPosition);
        }
    }



}
