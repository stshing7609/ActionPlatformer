using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBoxController : MonoBehaviour
{
    public Color indoorBackgroundColor;
    public Color outdoorBackgroundColor;
    public Camera myCamera;

    BoxCollider2D collider;

    bool outDoor = false;

    private void Start() {
        collider = GetComponent<BoxCollider2D>();
        myCamera.backgroundColor = indoorBackgroundColor;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        outDoor = true;

        StopCoroutine("FadeColor");
        StartCoroutine("FadeColor");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        outDoor = false;

        StopCoroutine("FadeColor");
        StartCoroutine("FadeColor");
    }

    IEnumerator FadeColor()
    {
        float elapsed = 0;
        Color toFade;

        if (outDoor)
            toFade = outdoorBackgroundColor;
        else
            toFade = indoorBackgroundColor;

        while (elapsed <= 1)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / .75f);
            myCamera.backgroundColor = Color.Lerp(myCamera.backgroundColor, toFade, t);
            yield return null;
        }
    }
}
