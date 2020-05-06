using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DestructiblePlatform : MonoBehaviour
{
    public SpriteRenderer rend;

    protected float timeToBreak;
    protected bool permanentDestroy;
    protected Color targetColor;

    protected void DestroyMe()
    {
        Destroy(gameObject);

        // if we want to permanently destroy this object, mark off whatever data file we're using to track that
        // if(permmanentDestroy)
    }

    protected IEnumerator DestroySequence(float time)
    {
        float elapsedTime = 0;
        Color defaultColor = rend.color;

        while(elapsedTime < time)
        {
            rend.color = Color.Lerp(defaultColor, targetColor, elapsedTime / time);
            elapsedTime += Time.deltaTime;

            yield return null;
        }
       
        DestroyMe();
    }
}
