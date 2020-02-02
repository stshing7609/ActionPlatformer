using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    const int MAX_ITEMS = 3;

    public GameObject pickUpObjectPrefab;

    List<GameObject> items;

    Vector2[] slotPositions = new Vector2[] { new Vector2(0.505f, 0.59f), new Vector2(0.75f, 0.125f), new Vector2(0.505f, -0.3f) };
    
    // Start is called before the first frame update
    void Start()
    {
        items = new List<GameObject>();
    }

    public void AddItem(GameObject item)
    {
        if (items.Count > MAX_ITEMS)
        {
            Debug.Log("Max inventory is 3");
            return;
        }

        item.GetComponent<PickUpObject>().PickUp(transform, slotPositions[items.Count]);
        items.Add(item);
    }

    public void AddItem(int id)
    {
        if (items.Count > MAX_ITEMS)
        {
            Debug.Log("Max inventory is 3");
            return;
        }

        GameObject instance = Instantiate(pickUpObjectPrefab);
        instance.GetComponent<PickUpObject>().Init(id);
        instance.GetComponent<PickUpObject>().PickUp(transform, slotPositions[items.Count]);
        items.Add(instance);
    }

    public void RemoveItem(int id, LockObject lockObject)
    {
        foreach (GameObject item in items)
        {
            PickUpObject puo = item.GetComponent<PickUpObject>();

            if (puo.Id == id)
            {
                puo.Use(lockObject);
                items.Remove(item);
                UpdatePositions();
                return;
            }
        }
    }

    public void RemoveAll()
    {
        items.Clear();
    }

    public List<int> GetItemIds()
    {
        List<int> ids = new List<int>();

        foreach (GameObject item in items)
        {
            ids.Add(item.GetComponent<PickUpObject>().Id);
        }
        return ids;
    }

    public bool CheckInventoryFull()
    {
        return items.Count >= MAX_ITEMS;
    }

    void UpdatePositions()
    {
        for(int i = 0; i < items.Count; i++)
        {
            items[i].GetComponent<PickUpObject>().UpdatePosition(slotPositions[i]);
        }
    }
}
