using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LockCounterDisplay : MonoBehaviour
{
    public TextMeshProUGUI textMesh;

    // Update is called once per frame
    void Update()
    {
        textMesh.text = "Things to Fix: " + VictoryTracker.Instance.lockCount;   
    }
}
