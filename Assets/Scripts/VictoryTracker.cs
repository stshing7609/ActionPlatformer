using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryTracker : MonoBehaviour
{
    private static VictoryTracker instance;
    public static VictoryTracker Instance { get => instance; }

    public int lockCount = 12;

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

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Return))
            SceneManager.LoadScene(0);
    }
}
