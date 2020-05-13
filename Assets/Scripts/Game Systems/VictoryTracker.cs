using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryTracker : MonoBehaviour
{
    private static VictoryTracker instance;
    public static VictoryTracker Instance { get => instance; }

    public int lockCount = 12;
    public GameObject winPanel;

    // Start is called before the first frame update
    void Awake()
    {
        //Establish Singleton Pattern
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void TriggerWin()
    {
        winPanel.SetActive(true);
    }
}
