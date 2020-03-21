using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackLadder : MonoBehaviour
{
    [SerializeField] BoxCollider2D myCollider;
    [SerializeField] SpriteRenderer rend;

    bool objectAbove;
    bool objectBelow;
    BackLadderEnd topEnd;
    BackLadderEnd bottomEnd;

    float edgeX;
    float height;

    public float EdgeX { get => edgeX; }
    public float Height { get => height; }
    public bool ObjectAbove { get => objectAbove; }
    public bool ObjectBelow { get => objectBelow; }

    private void Start()
    {
        topEnd = transform.Find("Top").GetComponent<BackLadderEnd>();
        bottomEnd = transform.Find("Bottom").GetComponent<BackLadderEnd>();

        height = rend.size.y;

        topEnd.Init(this);
        bottomEnd.Init(this);

        objectAbove = CheckForObjects(true);
        objectBelow = CheckForObjects(false);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerPlatformerController>().LadderEnter(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerPlatformerController>().ExitLadder();
        }
    }

    bool CheckForObjects(bool above)
    {
        Vector2 origin;
        Vector2 direction;
        float dist = 1f;
        float skin = 0.01f;

        if (above)
        {
            origin = (Vector2)transform.position + new Vector2(0, myCollider.bounds.extents.y*2 + skin);
            direction = Vector2.up;
        }
        else
        {
            origin = (Vector2)transform.position + new Vector2(0, -skin);
            direction = Vector2.down;
        }

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, dist, 1 << LayerMask.NameToLayer("Platforms"));

        //Debug.DrawRay(origin, direction, Color.magenta, 200);

        // check if there's anything directly above or below first
        if (hit)
        {
            return true;
        }

        return false;
    }
}
