using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class SkyBoxController : MonoBehaviour
{
    public Color indoorBackgroundColor;
    public Color outdoorBackgroundColor;
    public Camera myCamera;
    public Light2D light; //3

    BoxCollider2D collider;

    bool outDoor = false;

    private void Start() {
        collider = GetComponent<BoxCollider2D>();
        myCamera.backgroundColor = indoorBackgroundColor;
        light.intensity = 0;
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
        int intensity;

        if (outDoor)
        {
            toFade = outdoorBackgroundColor;
            intensity = 3;
        }
        else
        {
            toFade = indoorBackgroundColor;
            intensity = 0;
        }

        while (elapsed <= 1)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / .75f);
            myCamera.backgroundColor = Color.Lerp(myCamera.backgroundColor, toFade, t);
            light.intensity = Mathf.Lerp(light.intensity, intensity, t);
            yield return null;
        }
    }
}
