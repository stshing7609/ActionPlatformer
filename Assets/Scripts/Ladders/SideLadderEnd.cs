using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideLadderEnd : MonoBehaviour
{
    public bool top; // is this the top or bottom of the ladder
    [SerializeField] EdgeCollider2D myCollider;

    // Start is called before the first frame update
    public void Init(SideLadder ladder)
    {
        Vector2[] newPoints = new Vector2[2] { new Vector2(ladder.EdgeX, myCollider.points[0].y), new Vector2(ladder.EdgeX, myCollider.points[1].y) };

        myCollider.points = newPoints;

        if(top)
        {
            transform.localPosition = new Vector2(0, ladder.Height);
        }
        else
        {
            transform.localPosition = new Vector2(0, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerPlatformerController>().LadderEndCheck(top);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerPlatformerController>().ExitLadderEnd();
        }
    }
}
