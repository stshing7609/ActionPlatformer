using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerPlatformerController : UnitController
{
    public float maxSpeed = 3;
    public float[] jumpTakeOffSpeeds = new float[] { 4.7f, 4 };
    public float wallJumpTakeOffSpeed = 4;
    public float wallJumpImpulse = 2;
    public float wallFallSpeedMultiplier = 0.3f;
    public float stompImpulse = -5;
    public bool flipSprite;
    public GameObject pickUpObject;
    public LockObject lockInRange;

    Vector2 move;
    float jumpForgivenessTime;
    int jumpCount = 0;

    bool stomping;
    float stompDuration = .25f;
    float stompCooldown = 1.5f;

    private Animator animator;
    private BoxCollider2D myCollider;
    private Inventory inventory;

    // Use this for initialization
    void Awake()
    {
        animator = GetComponent<Animator>();
        myCollider = GetComponent<BoxCollider2D>();
        inventory = transform.Find("Inventory").GetComponent<Inventory>();
        jumpForgivenessTime = Time.deltaTime * 4;
    }

    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.E))
        {
           if(pickUpObject != null)
                PickUp();

            if (lockInRange != null)
                UseLock();
        }

        if (Input.GetKeyDown(KeyCode.Q) && grounded && !jumping && jumpCount == 0)
        {
            DropItem();
        }
    }

    protected override void ComputeVelocity()
    {
        move = Vector2.zero;

        move.x = Input.GetAxis("Horizontal");

        CalculateJump();
        WallStick();
        //Stomp();

        flipSprite = spriteRenderer.flipX ? move.x < -MIN_MOVE_DISTANCE : move.x > MIN_MOVE_DISTANCE;
        if (flipSprite)
        {
            Flip();
        }

        animator.SetBool("grounded", grounded);
        animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);

        targetVelocity = move * maxSpeed;
    }

    private void CalculateJump()
    {
        if (!grounded && !jumping)
        {
            if (ungroundedTime > jumpForgivenessTime)
            {
                grounded = false;
                jumping = true;
                jumpCount = 2;
                ungroundedTime = 0;
            }
            else
            {
                grounded = true;
                ungroundedTime += Time.deltaTime;
            }
        }

        if (!wallStick)
        {
            if (Input.GetButtonDown("Jump") && (grounded || (jumping && jumpCount < 2)))
            {
                velocity.y = jumpTakeOffSpeeds[jumpCount];
                jumping = true;
                jumpCount++;
            }
            else if (Input.GetButtonUp("Jump"))
            {
                if (velocity.y > 0)
                {
                    velocity.y = velocity.y * 0.5f;
                }
            }
        }
        else
        {
            if (Input.GetButtonDown("Jump"))
            {
                velocity.y = wallJumpTakeOffSpeed;
                jumping = true;
                jumpCount = 1;

                // if the player is holding to stick, then move the character away from the wall a little
                if (move.x > MIN_MOVE_DISTANCE)
                    move.x = -wallJumpImpulse;
                else if (move.x < -MIN_MOVE_DISTANCE)
                    move.x = wallJumpImpulse;

                wallStick = false;
            }
        }

        if (grounded && !jumping && jumpCount > 0)
            jumpCount = 0;
    }

    private void WallStick()
    {
        if (!wallStick || velocity.y > MIN_MOVE_DISTANCE)
        {
            gravityModifier = 1;
            return;
        }

        gravityModifier = wallFallSpeedMultiplier;
    }

    private void PickUp()
    {
        inventory.AddItem(pickUpObject);
        pickUpObject = null;
    }
    
    private void UseLock()
    {
        // Close it
        if (lockInRange.isOpen)
        {
            if (inventory.CheckInventoryFull())
            {
                Debug.Log("Max inventory is 3");
                return;
            }
            
            GameObject objectToAdd = lockInRange.Close();

            inventory.AddItem(objectToAdd);
        }
        else
        {
            int tryKey = -1;

            tryKey = lockInRange.Open(inventory.GetItemIds());
            // no valid key is possessed
            if (tryKey < 0)
                return;

            inventory.UseItem(tryKey, lockInRange);
        }
    }

    private void DropItem()
    {
        if (inventory.CheckInventoryEmpty())
        {
            Debug.Log("no items to drop");
            return;
        }

        Vector2 dir = Vector2.zero;

        if (spriteRenderer.flipX)
            dir = Vector2.right;
        else
            dir = Vector2.left;

        // raycast right first
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 1f, ~LayerMask.NameToLayer("Player"));

        Debug.DrawRay(transform.position, dir, Color.yellow);

        if (!hit)
        {
            Vector2 newPos = new Vector2(transform.position.x + 0.75f * dir.x, transform.position.y);
            inventory.DropItem(newPos);
            return;
        }

        dir = -dir;

        hit = Physics2D.Raycast(transform.position, dir, 1f, ~LayerMask.NameToLayer("Player"));

        Debug.DrawRay(transform.position, dir, Color.yellow);

        if (!hit)
        {
            Vector2 newPos = new Vector2(transform.position.x + 0.75f * dir.x, transform.position.y);
            inventory.DropItem(newPos);
            return;
        }

        Debug.Log("Can't drop item here");
    }

    /* Wall Jump Concepting
     * Find left and right contacts of player collider
     * Make sure not grounded
     * Ensure player is holding direction towards wall (negative value for wall to left and positive value for wall to right)
     * "Platforms" collision layer can count as walls
     * Have wallSticking parameter and reset jump count when wallSticking
     * Decrease gravity value when holding wall (slow fall)
     * If press Jump button while still holding towards wall, hop higher on same wall
     * If press Jump button while holding away from wall, jump off wall and gain distance
     * Allow jump while wall sticking
     * If player lets go of direction towards wall, set wallSticking to false and free fall
     * */

    /* Stomp Action
     * 
     * To Stomp, the player must first be airborne
     * The stomp action has a cooldown
     * The stomp action applies force to the character downward
     * */
    private void Stomp()
    {      
        if (stomping || grounded)
        {
            if (grounded)
            {
                StopCoroutine("StompLength");
                StopCoroutine("StompCountdown");
                gravityModifier = 1;
                stomping = false;
            }
            return;
        }
            
        if(Input.GetButtonDown("Vertical") && Input.GetAxis("Vertical") < -MIN_MOVE_DISTANCE)
        {
            velocity.y += stompImpulse;
            gravityModifier = 0;
            stomping = true;
            StartCoroutine("StompLength");
        }
    }

    IEnumerator StompLength()
    {
        float timer = 0;
        
        while(timer < stompDuration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        gravityModifier = 1;
        velocity.y -= stompImpulse;
        StartCoroutine("StompCountdown");
    }

    IEnumerator StompCountdown()
    {
        float timer = 0;

        while (timer < stompCooldown)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        stomping = false;
    }
}
