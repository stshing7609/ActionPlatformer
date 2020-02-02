using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    const int MAX_ITEMS = 3;

    public GameObject pickUpObjectPrefab;

    public List<GameObject> Items { get; private set; }

    Vector2[] slotPositions = new Vector2[] { new Vector2(0.505f, 0.59f), new Vector2(0.75f, 0.125f), new Vector2(0.505f, -0.3f) };
    
    // Start is called before the first frame update
    void Start()
    {
        Clear();
    }

    public void Clear()
    {
        Items = new List<GameObject>();
    }
    public void AddItem(GameObject item)
    {
        if (Items.Count > MAX_ITEMS)
        {
            Debug.Log("Max inventory is 3");
            return;
        }

        item.GetComponent<PickUpObject>().PickUp(transform, slotPositions[Items.Count]);
        Items.Add(item);
    }

    public void UseItem(int id, LockObject lockObject)
    {
        foreach (GameObject item in Items)
        {
            PickUpObject puo = item.GetComponent<PickUpObject>();

            if (puo.Id == id)
            {
                puo.Use(lockObject);
                Items.Remove(item);
                UpdatePositions();
                return;
            }
        }
    }

    public void DropItem(Vector2 dropSpot)
    {
        PickUpObject puo = Items[0].GetComponent<PickUpObject>();
        int id = puo.Id;

        //items[0].GetComponent<PickUpObject>().DestroyIt();

        Items.RemoveAt(0);
        UpdatePositions();

        puo.Init(dropSpot);
    }

    public void DropItemByID(int id, Vector2 dropSpot)
        // Drops the item referenced by id if it exists.
    {
        GameObject itemToDrop = null;
        int index = 0;
        foreach (GameObject item in Items) // Find the object
        {
            index = 0;
            if (item.GetInstanceID() == id)
            {
                itemToDrop = item;
                break;
            }
            else
            {
                index++;
            }
        }

        if (!(itemToDrop is null)) {
            itemToDrop.GetComponent<PickUpObject>().DestroyIt();

            Items.RemoveAt(index);
            UpdatePositions();
        }

        GameObject instance = Instantiate(pickUpObjectPrefab);
        instance.GetComponent<PickUpObject>().Init(dropSpot);
    }
    public void RemoveAll()
    {
        Items.Clear();
    }

    public List<int> GetItemIds()
    {
        List<int> ids = new List<int>();

        foreach (GameObject item in Items)
        {
            ids.Add(item.GetComponent<PickUpObject>().Id);
        }
        return ids;
    }

    public bool CheckInventoryFull()
    {
        return Items.Count >= MAX_ITEMS;
    }

    public bool CheckInventoryEmpty()
    {
        return Items.Count == 0;
    }

    void UpdatePositions()
    {
        for(int i = 0; i < Items.Count; i++)
        {
            Items[i].GetComponent<PickUpObject>().UpdatePosition(slotPositions[i]);
        }
    }

}
