using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenuController : MonoBehaviour
{
    [SerializeField] GameObject mainButtonHolder;
    [SerializeField] GameObject instructionsGameObject;

    PlayerPlatformerController player;

    // Start is called before the first frame update
    void Start()
    {
        if (DataController.Instance.reloaded)
        {
            gameObject.SetActive(false);
            return;
        }
 
        DataController.Instance.disableControls = true;
    }

    public void StartPlay()
    {
        DataController.Instance.disableControls = false;
        gameObject.SetActive(false);
    }

    public void HowToPlay()
    {
        mainButtonHolder.SetActive(false);
        instructionsGameObject.SetActive(true);
    }

    public void Back()
    {
        mainButtonHolder.SetActive(true);
        instructionsGameObject.SetActive(false);
    }
}
