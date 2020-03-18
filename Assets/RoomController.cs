using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class RoomController : MonoBehaviour
{
    public bool status;
    public Volume volume;

    BoxCollider2D trigger;

    private void Start()
    {
        trigger = GetComponent<BoxCollider2D>();
    }

    public void FixRoom(bool isFixed)
    {
        status = isFixed;
        UpdateRoom();
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            UpdateRoom();
        }
    }

    void UpdateRoom()
    {
        if (status)
        {
            StopCoroutine("FadeIn");
            StartCoroutine("FadeIn", 0);
        }
        else
        {
            StopCoroutine("FadeIn");
            StartCoroutine("FadeIn", 1);
        }
    }

    IEnumerator FadeIn(int weight)
    {
        float elapsed = 0;

        while (elapsed <= 1)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / 5f);
            volume.weight = Mathf.Lerp(volume.weight, weight, t);
            yield return null;
        }
    }
}
