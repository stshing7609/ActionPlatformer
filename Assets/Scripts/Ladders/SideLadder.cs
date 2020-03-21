using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideLadder : MonoBehaviour
{
    [SerializeField] bool facingRight = true;
    [SerializeField] EdgeCollider2D myCollider;
    [SerializeField] SpriteRenderer rend;
    bool[] objectsAbove; // [above, ahead and above] - checking if there's any platform above the ladder and if there's clearance above where the player would have to move to climb onto a ledge
    bool[] objectsBelow; // [below, ahead and below] - checking if there's any platform below the ladder and if there's a platform for the player to step on upon dismounting
    SideLadderEnd topEnd;
    SideLadderEnd bottomEnd;

    float edgeX;
    float height;
    public bool FacingRight { get => facingRight; }
    public float EdgeX { get => edgeX; }
    public float Height { get => height; }
    public bool[] ObjectsAbove { get => objectsAbove; }
    public bool[] ObjectsBelow { get => objectsBelow; }

    private void Start()
    {
        topEnd = transform.Find("Top").GetComponent<SideLadderEnd>();
        bottomEnd = transform.Find("Bottom").GetComponent<SideLadderEnd>();

        height = rend.size.y;

        Vector2[] newPoints;

        if (facingRight)
        {
            edgeX = 0.5f;
            newPoints = new Vector2[2] { new Vector2(edgeX, height), new Vector2(edgeX, 0) };
        }
        else
        {
            edgeX = -0.5f;
            newPoints = new Vector2[2] { new Vector2(edgeX, height), new Vector2(edgeX, 0) };
        }

        myCollider.points = newPoints;

        transform.Find("CheckCollider").GetComponent<BoxCollider2D>().offset = new Vector2(0, height / 2);
        transform.Find("CheckCollider").GetComponent<BoxCollider2D>().size = new Vector2(1, height);

        topEnd.Init(this);
        bottomEnd.Init(this);

        objectsAbove = CheckForObjects(true);
        objectsBelow = CheckForObjects(false);

        //Debug.Log(gameObject.name + "'s above: " + objectsAboveLayer[0] + ", " + objectsAboveLayer[1]);
        //Debug.Log(gameObject.name + "'s below: " + objectsBelowLayer[0] + ", " + objectsBelowLayer[1]);
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            //collision.GetComponent<PlayerPlatformerController>().LadderCheck(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerPlatformerController>().ExitLadder();
        }
    }

    bool[] CheckForObjects(bool above)
    {
        Vector2 origin;
        Vector2 direction;
        float dist = 1f;
        float skin = 0.01f;
        bool[] toReturn = new bool[] { false, false };
        
        if(above)
        {
            origin = (Vector2)transform.position + new Vector2(0, myCollider.points[0].y + skin);
            direction = Vector2.up;
        }
        else
        {
            origin = (Vector2)transform.position + new Vector2(0, myCollider.points[1].y - skin);
            direction = Vector2.down;
        }

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, dist, 1 << LayerMask.NameToLayer("Platforms"));

        // check if there's anything directly above or below first
        if(hit)
        {
            toReturn[0] = true;
        }

        //Debug.DrawRay(origin, direction, Color.white, 200);

        if (above)
        {
            origin = (Vector2)transform.position + myCollider.points[0] + new Vector2(0, skin);
        }
        else
        {
            origin = (Vector2)transform.position + myCollider.points[1] - new Vector2(0, skin);
        }

        hit = Physics2D.Raycast(origin, direction, dist, 1 << LayerMask.NameToLayer("Platforms"));

        // check if there's anything above or below to the side the ladder is facing
        if (hit)
        {
            toReturn[1] = true;
        }

        //Debug.DrawRay(origin, direction, Color.magenta, 200);

        return toReturn;
    }
}
