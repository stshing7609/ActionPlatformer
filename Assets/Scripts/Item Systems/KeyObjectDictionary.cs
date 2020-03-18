using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KeyItems
{ 
    Toolbox,
    Fuse,
    DoorHandle,
    DoorKnob,
    Glue,
    Soap,
    HotChocolate
}

public class KeyObjectDictionary : MonoBehaviour
{
    public static Dictionary<KeyItems, Sprite> keyItems;

    // Start is called before the first frame update
    void Start()
    {
        keyItems = new Dictionary<KeyItems, Sprite>();

        Sprite[] props4 = Resources.LoadAll<Sprite>("Sprites/Props/Props4");
        Sprite[] props5 = Resources.LoadAll<Sprite>("Sprites/Props/Props5");
 
        keyItems.Add(KeyItems.Toolbox, props4[1]);
        keyItems.Add(KeyItems.Fuse, props4[6]);
        keyItems.Add(KeyItems.DoorHandle, props4[4]);
        keyItems.Add(KeyItems.DoorKnob, props4[3]);
        keyItems.Add(KeyItems.Glue, props4[5]);
        keyItems.Add(KeyItems.Soap, props4[8]);
        keyItems.Add(KeyItems.HotChocolate, props5[0]);

    }
}
