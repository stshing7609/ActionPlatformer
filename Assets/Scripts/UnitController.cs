using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : PhysicsObject2D
{
    public GameObject ledgeDetector;
    public LayerMask groundCollisionMask;
    protected Vector3 ledgePosition;
    protected Vector3 rightLedgeDetectorPos = new Vector3(-0.103f, -0.496f, 0);
    protected Vector3 leftLedgeDetectorPos = new Vector3(0.084f, -0.496f, 0);

    protected bool onLedge;

    protected SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected void Flip()
    {
        spriteRenderer.flipX = !spriteRenderer.flipX;
        if(spriteRenderer.flipX)
            ledgeDetector.transform.localPosition = rightLedgeDetectorPos;
        else
            ledgeDetector.transform.localPosition = leftLedgeDetectorPos;
    }

    // check if we're now over a ledge
    protected bool IsOnLedge()
    {
        Vector2 pos = ledgeDetector.transform.position;
        float dist = 0.05f; // how far are we checking
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.down, dist, groundCollisionMask);

        //Debug.DrawRay(pos, GameController.instance.Dir.ToVector2(), Color.green);

        // if we hit nothing, then we're over a ledge
        if (hit.collider == null)
        {
            return true;
        }

        return false;
    }
}
