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
        textMesh.text = "Lock Count: " + VictoryTracker.Instance.lockCount;   
    }
}
