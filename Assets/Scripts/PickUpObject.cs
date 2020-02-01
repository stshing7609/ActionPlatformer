using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* The KEY
 * pick up
 * use
 * number - what key is this (represented as a number)
 * sprite
 * player
 * follow
 * */

/* The LOCK
 * isLocked
 * key used (what key is being held)
 * keys required
 * sprite
 * pick up
 * */

public class PickUpObject : MonoBehaviour
{
    public Collider2D myCollider;
    public Animator anim;
    public GameObject interactionSensor;
    public float dampTime = 0.15f;

    private Vector3 startPosition;
    private int keyCode;
    private bool isHeld = false;
    private bool used = false;
    private Transform playerTransform;
    private PlayerPlatformerController player;
    private Vector3 velocity = Vector3.zero;

    public bool IsHeld { get => isHeld; }
    public bool Used { get => used; }
    public int KeyCode { get => keyCode; }

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        player = playerTransform.GetComponent<PlayerPlatformerController>();
        anim.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isHeld)
            Follow();
    }

    public void PickUp(Transform newParent)
    {
        gameObject.SetActive(true);
        isHeld = true;
        myCollider.enabled = false;
        transform.localScale = new Vector2(.25f, .25f);
        transform.SetParent(newParent);
        anim.enabled = true;
        interactionSensor.SetActive(false);

        if(player.spriteRenderer.flipX)
            transform.localPosition = new Vector2(-0.6f, 0.135f);
        else
            transform.localPosition = new Vector2(0.6f, 0.135f);
    }

    public void Use()
    {
        used = true;
        isHeld = false;
        transform.localScale = Vector2.one;
        gameObject.SetActive(false);
        anim.enabled = false;
    }

    void Follow()
    {
        if (player)
        {
            int multiplier = 1;
            bool flip = player.flipSprite;
            if (flip)
                multiplier = -1;

            transform.localPosition = new Vector2(transform.localPosition.x * multiplier, transform.localPosition.y);
            //transform.position = playerTransform.position + new Vector3(0.6f * multiplier, 0.375f, 0);
            //Vector3 delta = playerTransform.position - new Vector3(0.5f, 0.5f, playerTransform.position.z);

            //Vector3 destination = playerTransform.position + delta;

            //transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
        }
    }
}
