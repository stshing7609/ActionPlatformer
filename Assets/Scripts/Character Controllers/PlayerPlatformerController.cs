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

    bool interacting = false;

    bool stomping;
    float stompDuration = .25f;
    float stompCooldown = 1.5f;

    private Animator animator;
    private BoxCollider2D myCollider;
    private HUDInventory inventory;
    private KeyItems selectedItem;

    // Use this for initialization
    void Awake()
    {
        animator = GetComponent<Animator>();
        myCollider = GetComponent<BoxCollider2D>();
        inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<HUDInventory>();
        jumpForgivenessTime = Time.deltaTime * 4;
        selectedItem = KeyItems.HotChocolate; // do this so that if we reset, the value is something that can't be used to escape the room
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

        //if (Input.GetKeyDown(KeyCode.Q) && grounded && !jumping && jumpCount == 0)
        //{
        //    DropItem();
        //}

        if(Input.GetKeyDown(KeyCode.Q))
            SelectItem();
    }

    protected override void ComputeVelocity()
    {
        move = Vector2.zero;

        move.x = Input.GetAxis("Horizontal");

        CalculateJump();
        WallStick();
        //Stomp();

        flipSprite = spriteRenderer.flipX ? move.x > MIN_MOVE_DISTANCE : move.x < -MIN_MOVE_DISTANCE;
        if (flipSprite)
        {
            Flip();
        }

        animator.SetBool("grounded", grounded);
        animator.SetBool("wallsticking", wallStick);
        animator.SetBool("jumping", jumping);
        animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);
        animator.SetFloat("velocityY", velocity.y);

        if (interacting)
            animator.SetTrigger("interact");

        interacting = false;

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
                if (jumpCount > jumpTakeOffSpeeds.Length)
                    jumpCount = jumpTakeOffSpeeds.Length - 1;
                
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
        // if our inventory is empty, set our selectedItem to what we just picked up
        if(inventory.CheckInventoryEmpty())
        {
            selectedItem = pickUpObject.GetComponent<PickUpObject>().Id;
        }
        
        inventory.AddItem(pickUpObject.GetComponent<PickUpObject>().Id);
        Destroy(pickUpObject);
        pickUpObject = null;
        interacting = true;
    }
    
    private void UseLock()
    {
        interacting = true;
        
        // Close it
        if (lockInRange.isOpen)
        {           
            KeyItems keyToAdd = lockInRange.Close();

            inventory.AddItem(keyToAdd);
        }
        else
        {
            bool[] checks = inventory.UseItem(selectedItem, lockInRange);

            // check if the item can be used
            if (checks[0]) 
            {
                // if it's the last of that item, then change the display
                if(checks[1])
                    SelectItem();
            }
            else
                DialogueCreator.Instance.InitDialogue(lockInRange.cannotOpenDialogueId);

            //int tryKey = -1;

            //tryKey = lockInRange.Open(oldInventory.GetItemIds());
            //// no valid key is possessed
            //if (tryKey < 0)
            //    return;

            //oldInventory.UseItem(tryKey, lockInRange);
        }
    }

    private void SelectItem()
    {
        // don't select any item if the inventory is empty
        if (inventory.CheckInventoryEmpty())
            return;

        selectedItem = inventory.SelectItem();
    }

    //private void DropItem()
    //{
    //    if (oldInventory.CheckInventoryEmpty())
    //    {
    //        Debug.Log("no items to drop");
    //        return;
    //    }

    //    Vector2 dir = Vector2.zero;

    //    if (!spriteRenderer.flipX)
    //        dir = Vector2.right;
    //    else
    //        dir = Vector2.left;

    //    // raycast right first
    //    RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 1f, ~LayerMask.NameToLayer("Player"));

    //    Debug.DrawRay(transform.position, dir, Color.yellow);

    //    if (!hit)
    //    {
    //        Vector2 newPos = new Vector2(transform.position.x + 0.75f * dir.x, transform.position.y);
    //        oldInventory.DropItem(newPos);
    //        return;
    //    }

    //    dir = -dir;

    //    hit = Physics2D.Raycast(transform.position, dir, 1f, ~LayerMask.NameToLayer("Player"));

    //    Debug.DrawRay(transform.position, dir, Color.yellow);

    //    if (!hit)
    //    {
    //        Vector2 newPos = new Vector2(transform.position.x + 0.75f * dir.x, transform.position.y);
    //        oldInventory.DropItem(newPos);
    //        return;
    //    }

    //    Debug.Log("Can't drop item here");
    //}

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
