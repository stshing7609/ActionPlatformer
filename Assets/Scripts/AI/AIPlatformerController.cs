using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AIPlatformerController : UnitController
    /* This is a UnitController that uses the information in the GameObject's 'States'
     * attribute to manipulate the GameObject.
     * This is so that the MLAgent can simply update State information in order to provide
     * inputs.
     */
{
    public float maxSpeed = 3;
    public float[] jumpTakeOffSpeeds = new float[] { 4.7f, 4 };
    public float wallJumpTakeOffSpeed = 4;
    public float wallJumpImpulse = 2;
    public float wallFallSpeedMultiplier = 0.3f;

    Vector2 move;
    float jumpForgivenessTime;
    int jumpCount = 0;

    private Animator animator;
    private BoxCollider2D myCollider;
    private States buttons;

    // Use this for initialization
    void Awake()
    {
        animator = GetComponent<Animator>();
        myCollider = GetComponent<BoxCollider2D>();
        buttons = GetComponent<States>();
        jumpForgivenessTime = Time.deltaTime * 4;
    }

    protected override void ComputeVelocity()
    {
        move = Vector2.zero;

        move.x = buttons.horizontal_tilt;

        CalculateJump();
        CalculateWallStick();

        bool flipSprite = spriteRenderer.flipX ? move.x < -MIN_MOVE_DISTANCE : move.x > MIN_MOVE_DISTANCE;
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
            if (buttons.jump_button_pressed && (grounded || (jumping && jumpCount < 2)) && jumpCount < 2)
            {
                velocity.y = jumpTakeOffSpeeds[jumpCount];
                jumping = true;
                jumpCount++;
            }
            else if (!buttons.jump_button_pressed && jumping) // This can only be true the frame after a jump button is released.
            {
                jumping = false;
                jumpCount = 0;
                if (velocity.y > 0)
                {
                    velocity.y = velocity.y * 0.5f;
                }
            }
        }
        else
        {
            if (buttons.jump_button_pressed)
            {
                velocity.y = wallJumpTakeOffSpeed;
                jumping = true;
                jumpCount = 1;

                // If the player is holding to stick, then move the character away from the wall a little
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

    private void CalculateWallStick()
    {
        if (!wallStick || velocity.y > MIN_MOVE_DISTANCE)
        {
            gravityModifier = 1;
            return;
        }

        gravityModifier = wallFallSpeedMultiplier;
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
}
