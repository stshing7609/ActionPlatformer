using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataController : MonoBehaviour
{
    private static DataController instance;

    public static DataController Instance { get => instance; }

    public bool reloaded = false;
    public bool disableControls = false;

    void Awake()
    {
        //Establish Singleton Pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Update()
    {
        if (!disableControls && Input.GetKeyUp(KeyCode.Return))
        {
            reloaded = true;
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }
}
