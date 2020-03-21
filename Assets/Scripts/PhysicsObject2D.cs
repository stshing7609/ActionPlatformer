using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// base for all objects that move around
public class PhysicsObject2D : MonoBehaviour
{
    public float gravityModifier = 2f;
    public float minGroundNormalY = 0.65f;

    protected const float MIN_MOVE_DISTANCE = 0.001f;                           // minimum distance necessary before we treat calculate movement
    protected const float SKIN = 0.01f;

    protected bool grounded;                                                    // is this physics object ground
    protected Vector2 targetVelocity;
    protected Vector2 groundNormal;
    protected Rigidbody2D rb2d;                                                 // our rigidbody: make sure this is kinematic
    protected Vector2 velocity;                                                 // velocity used to move the object
    protected ContactFilter2D contactFilter;                                    // a contact filter used for predicting movement    
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];                  // an array buffer for the contact filter to use - only check a max of 16 collisions
    protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);    // a list of for the hits to store and for us to use - use a list in case don't fully populate the hitBuffer array
    protected bool jumping;
    protected float ungroundedTime = 0;
    protected bool wallStick;

    private void OnEnable()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
    }

    protected virtual void Update()
    {
        targetVelocity = Vector2.zero;
        ComputeVelocity();
    }

    protected virtual void ComputeVelocity()
    {

    }

    protected virtual void FixedUpdate()
    {
        velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
        velocity.x = targetVelocity.x;

        if (gravityModifier == 0)
            velocity.y = targetVelocity.y;

        grounded = false;
        wallStick = false;

        Vector2 deltaPosition = velocity * Time.deltaTime;

        // do vertical movement first to check for grounded
        Vector2 move = Vector2.up * deltaPosition.y;
        Movement(move, true);

        // do horizontal movement
        // this accounts for sloped ground
        Vector2 moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);

        move = moveAlongGround * deltaPosition.x;
        Movement(move, false);
    }

    void Movement(Vector2 move, bool yMovement)
    {
        float distance = move.magnitude;

        // do nothing if the distance asked to move is below a certain threshold
        if (distance > MIN_MOVE_DISTANCE)
        {
            // cast the rigidbody ahead the distance we want to move
            int count = rb2d.Cast(move, contactFilter, hitBuffer, distance + SKIN);

            //Debug.Log(gameObject.name + "'s count: " + count);

            // clear our hit buffer list
            hitBufferList.Clear();

            // populate our hit buffer list
            for (int i = 0; i < count; i++)
            {
                hitBufferList.Add(hitBuffer[i]);
            }

            for (int i = 0; i < hitBufferList.Count; i++)
            {
                // ignore any collisions with the object itself
                if (transform.Find(hitBufferList[i].transform.name))
                {
                    continue;
                }

                Vector2 currentNormal = hitBufferList[i].normal;
                if (currentNormal.y > minGroundNormalY)
                {
                    grounded = true;
                    jumping = false;
                    ungroundedTime = 0;
                    if (yMovement)
                    {
                        groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }

                //if (!yMovement)
                //{
                    //Debug.Log(currentNormal);
                    //CheckWallSticking();
                    //Debug.Log(wallStick);
                //}

                float projection = Vector2.Dot(velocity, currentNormal);
                if (projection < 0)
                {
                    velocity = velocity - projection * currentNormal;
                }

                // check the distance before any of our hits and if that distance is shorter than our movement distance, change distance to the distance before the hit
                // Require the distance to be greater than that of our minimum move distance (to account for crates fitting snuggly by other objects)
                // Always check the distance if we're grounded to avoid clipping through walls
                float modifiedDistance = hitBufferList[i].distance > MIN_MOVE_DISTANCE ? hitBufferList[i].distance - SKIN : distance;

                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }

            rb2d.position = rb2d.position + move.normalized * distance;
        }
    }

    void CheckWallSticking()
    {
        if (grounded || targetVelocity.x == 0)
        {
            wallStick = false;
            return;
        }

        wallStick = true;
    }

    protected bool CheckOnLadder(bool facingRight)
    {
        bool checkWrongDir;

        if (facingRight)
            checkWrongDir = targetVelocity.x >= 0;
        else
            checkWrongDir = targetVelocity.x <= 0;

        if (checkWrongDir)
        {
            return false;
        }

        return true;
    }
}