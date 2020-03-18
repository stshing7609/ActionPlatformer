using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDInventory : MonoBehaviour
{
    public GameObject pickUpObjectPrefab;

    Dictionary<KeyItems, int> items;
    List<Transform> displayItems;
    Color fadedColor = new Color(1, 1, 1, .5f);

    // Start is called before the first frame update
    void Start()
    {
        items = new Dictionary<KeyItems, int>();
        displayItems = new List<Transform>();
        for (int i = 0; i < transform.childCount; i++)
        {
            displayItems.Add(transform.GetChild(i));
        }
    }

    public void AddItem(KeyItems id)
    {
        if (items.ContainsKey(id))
            items[id]++;
        else
        {
            items.Add(id, 1);
            Debug.Log(id);
        }

        UpdateDisplay(id);
    }

    public bool UseItem(KeyItems id, LockObject lockObject)
    {
        Debug.Log(id);
        Debug.Log(items.ContainsKey(id) + " and " + items[id]);
        
        if(items.ContainsKey(id) && items[id] > 0)
        {
            Debug.Log("what");
            
            GameObject instance = Instantiate(pickUpObjectPrefab);
            
            instance.GetComponent<PickUpObject>().Use(id, lockObject);

            items[id]--;
            UpdateDisplay(id);

            return true;
        }

        return false;
    }

    public void RemoveAll()
    {
        items.Clear();
    }

    public bool CheckInventoryEmpty()
    {
        return items.Count == 0;
    }

    void UpdateDisplay(KeyItems id)
    {
        int numId = (int)id;
        Transform curr = displayItems[numId];
        Transform textTrans = curr.GetChild(0);

        if (items[id] > 0)
        {
            curr.GetComponent<Image>().color = Color.white;
        }
        else
            curr.GetComponent<Image>().color = fadedColor;

        if (items[id] > 1)
        {
            textTrans.gameObject.SetActive(true);
            textTrans.GetComponent<Text>().text = items[id].ToString();
        }
        else
        {
            textTrans.gameObject.SetActive(false);
        }
    }
}
