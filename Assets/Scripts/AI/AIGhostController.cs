using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AIGhostController : UnitController
/* This is a UnitController that uses the information in the GameObject's 'States'
 * attribute to manipulate the GameObject.
 * This is so that the MLAgent can simply update State information in order to provide
 * inputs.
 */
{
    public float maxSpeed = 1;
    Vector2 move;

    private Animator animator;
    private BoxCollider2D myCollider;
    private States buttons;
    public bool active = true;

    // Use this for initialization
    void Awake()
    {
        animator = GetComponent<Animator>();
        myCollider = GetComponent<BoxCollider2D>();
        buttons = GetComponent<States>();
    }

    protected override void ComputeVelocity()
    {
        move = Vector2.zero;

        move.x = buttons.horizontal_tilt / 2;
        move.y = buttons.vertical_tilt;

        bool flipSprite = spriteRenderer.flipX ? move.x < -MIN_MOVE_DISTANCE : move.x > MIN_MOVE_DISTANCE;
        if (flipSprite)
        {
            Flip();
        }

        animator.SetBool("grounded", grounded);
        animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);

        float boost = 0f;
        if (buttons.action_button_pressed)
        {
            boost = 1f;
        }
        targetVelocity = move * maxSpeed * (1 + boost);
        velocity.y = targetVelocity.y;
        velocity.x = targetVelocity.x;
    }
}
