using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDInventory : MonoBehaviour
{
    public GameObject pickUpObjectPrefab;
    public GameObject highlighter;

    Dictionary<KeyItems, int> items;
    List<Transform> displayItems;
    Color fadedColor = new Color(.5f, .5f, .5f, .5f);
    int selectedItem = 0;

    // Start is called before the first frame update
    void Start()
    {
        items = new Dictionary<KeyItems, int>();
        displayItems = new List<Transform>();

        Transform contentTransform = transform.Find("Content");
        for (int i = 0; i < contentTransform.childCount; i++)
        {
            displayItems.Add(contentTransform.GetChild(i));
        }
    }

    public void AddItem(KeyItems id)
    {
        if (items.ContainsKey(id))
            items[id]++;
        else
        {
            items.Add(id, 1);
        }

        UpdateDisplay(id);
    }

    public bool[] UseItem(KeyItems id, LockObject lockObject)
    {
        // ensure it's valid key and that we have at least one
        if(lockObject.CheckKey(id) && items.ContainsKey(id) && items[id] > 0)
        {
            GameObject instance = Instantiate(pickUpObjectPrefab);
            instance.GetComponent<PickUpObject>().Use(id, lockObject);
            lockObject.Open(instance);

            items[id]--;

            UpdateDisplay(id);

            // check if we no longer have any of the item used
            return new bool[2] { true, items[id] == 0 };
        }

        return new bool[2] { false, false };
    }

    public KeyItems SelectItem()
    {
        // search for the next valid item in our list
        while (true)
        {
            selectedItem++;

            if(selectedItem > displayItems.Count)
            {
                selectedItem = 0;
            }

            KeyItems id = (KeyItems)selectedItem;

            if (items.ContainsKey(id) && items[id] > 0)
            {
                highlighter.transform.localPosition = displayItems[selectedItem].localPosition;
                return id;
            }
        }
    }

    public void RemoveAll()
    {
        items.Clear();
    }

    public bool CheckInventoryEmpty()
    {
        bool isEmpty = true;
        
        foreach(KeyValuePair<KeyItems, int> entry in items)
        {
            if (entry.Value > 0)
                return false;
        }

        return isEmpty;
    }

    void UpdateDisplay(KeyItems id)
    {
        int numId = (int)id;
        Transform curr = displayItems[numId];
        Transform textTrans = curr.GetChild(0);

        // if we have any of the item, then it's visible, otherwise set it to half visible
        if (items[id] > 0)
        {
            curr.GetComponent<Image>().color = Color.white;
        }
        else
            curr.GetComponent<Image>().color = fadedColor;

        // if we have more than one of the item, display a number to show how many
        if (items[id] > 1)
        {
            textTrans.gameObject.SetActive(true);
            textTrans.GetComponent<Text>().text = items[id].ToString();
        }
        else
        {
            textTrans.gameObject.SetActive(false);
        }

        // if the inventory is empty, hide the highlighter
        if (CheckInventoryEmpty())
        {
            highlighter.SetActive(false);
        }
        // otherwise if the inventory isn't emtpy and the highlighter isn't displaying, show it
        else if(!highlighter.activeSelf)
        {
            highlighter.SetActive(true);
            selectedItem = (int)id;
            highlighter.transform.localPosition = displayItems[selectedItem].localPosition;
        }
    }
}
