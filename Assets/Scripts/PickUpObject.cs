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
    public SpriteRenderer rend;
    public float dampTime = 0.15f;

    private Vector3 startPosition;
    public int id;
    private bool isHeld = false;
    private Transform playerTransform;
    private PlayerPlatformerController player;
    private Vector3 velocity = Vector3.zero;

    public bool IsHeld { get => isHeld; }
    public int Id { get => id; }

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        player = playerTransform.GetComponent<PlayerPlatformerController>();
        anim.enabled = false;
        switch(id)
        {
            case 0:
                rend.color = new Color(0, 1, 0.2533984f, 1);
                break;
            case 1:
                rend.color = new Color(0, 0.3429475f, 1, 1);
                break;
            case 2:
                rend.color = new Color(1, 0.8929985f, 0, 1);
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isHeld)
            Follow();
    }

    public void Init(int id)
    {
        this.id = id;

        Start();
    }

    public void PickUp(Transform newParent, Vector2 newPos)
    {
        gameObject.SetActive(true);
        isHeld = true;
        myCollider.enabled = false;
        transform.localScale = new Vector2(.25f, .25f);
        transform.SetParent(newParent);
        anim.enabled = true;
        interactionSensor.SetActive(false);
        rend.color = new Color(rend.color.r, rend.color.g, rend.color.b, 1);

        if(player.spriteRenderer.flipX)
            transform.localPosition = new Vector2(-newPos.x, newPos.y);
        else
            transform.localPosition = newPos;
    }

    public void Use(LockObject lockObject)
    {
        isHeld = false;
        transform.SetParent(lockObject.transform);
        transform.localPosition = Vector2.zero;
        transform.localScale = new Vector2(.33f, .24f);
        rend.color = new Color(rend.color.r, rend.color.g, rend.color.b, .4f);
        lockObject.myObject = this;
    }

    public void DestroyIt()
    {
        Destroy(gameObject);
    }

    public void UpdatePosition(Vector2 newPos)
    {
        if (player.spriteRenderer.flipX)
            transform.localPosition = new Vector2(-newPos.x, newPos.y);
        else
            transform.localPosition = newPos;
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
