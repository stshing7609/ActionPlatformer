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
        // Reset inventory
        inventory.Clear();

        foreach (int itemID in itemIDs) 
        {
            inventory.AddItem(itemID);
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
