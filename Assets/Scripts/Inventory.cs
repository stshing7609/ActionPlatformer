using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    List<PickUpObject> items;
    const int MAX_ITEMS = 4;
    
    // Start is called before the first frame update
    void Start()
    {
        items = new List<PickUpObject>();
    }

    public void AddItem(PickUpObject item)
    {
        if (items.Count > MAX_ITEMS)
        {
            Debug.Log("Max inventory is 4");
            return;
        }

        item.PickUp(transform);
        items.Add(item);
    }

    public void RemoveItem(PickUpObject item)
    {
        if (items.Contains(item))
            items.Remove(item);
    }

    public void RemoveAll()
    {
        items.Clear();
    }

    public PickUpObject UseItem(int keyCode)
    {
        foreach(PickUpObject item in items)
        {
            if (item.KeyCode == keyCode)
            {
                item.Use();
                items.Remove(item);
                return item;
            }
        }

        return null;
    }
}
