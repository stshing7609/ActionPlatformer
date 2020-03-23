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
    public Animator anim;
    public GameObject interactionSensor;
    public SpriteRenderer rend;
    public float dampTime = 0.15f;

    [SerializeField] KeyItems id;
    private bool isHeld = false;
    private Transform playerTransform;
    private PlayerPlatformerController player;
    private Vector3 velocity = Vector3.zero;
    private Vector3 startScale;
    private GameObject sensor;

    public bool IsHeld { get => isHeld; }
    public KeyItems Id { get => id; }

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        player = playerTransform.GetComponent<PlayerPlatformerController>();
        startScale = transform.localScale;
        sensor = transform.Find("InteractionSensor").gameObject;
    }

    // Update is called once per frame
    //void Update()
    //{
    //    if (isHeld)
    //        Follow();
    //}

    void ResetValues()
    {
        gameObject.SetActive(true);
        transform.localScale = startScale;
        rend.color = new Color(rend.color.r, rend.color.g, rend.color.b, 1);
        anim.enabled = false;
        transform.parent = null;
        isHeld = false;
        interactionSensor.SetActive(true);
    }

    public void Init(Vector2 pos)
    {
        transform.position = pos;

        ResetValues();
    }

    public void PickUp(Transform newParent, Vector2 newPos)
    {
        gameObject.SetActive(true);
        isHeld = true;
        transform.SetParent(newParent);
        transform.localScale = new Vector2(.33f, .33f);
        anim.enabled = true;
        interactionSensor.SetActive(false);
        rend.color = new Color(rend.color.r, rend.color.g, rend.color.b, 1);

        if(player.spriteRenderer.flipX)
            transform.localPosition = new Vector2(-newPos.x, newPos.y);
        else
            transform.localPosition = newPos;
    }

    public void Use(KeyItems newId, LockObject lockObject)
    {
        id = newId;

        Sprite newSprite;

        if (KeyObjectDictionary.keyItems.TryGetValue(id, out newSprite))
        {
            rend.sprite = newSprite;
            anim.enabled = true;
        }
        else
        {
            Debug.LogError("could not find image in dictionary");
            return;
        }

        isHeld = false;
        transform.SetParent(lockObject.transform);
        transform.localPosition = Vector2.zero;
        transform.localScale = new Vector2(1, 1);
        rend.color = new Color(rend.color.r, rend.color.g, rend.color.b, .6f);
        anim.enabled = true;
        interactionSensor.SetActive(false);
        lockObject.myObject = gameObject;
    }

    //public void Use(LockObject lockObject)
    //{
    //    isHeld = false;
    //    transform.SetParent(lockObject.transform);
    //    transform.localPosition = Vector2.zero;
    //    transform.localScale = new Vector2(1, 1);
    //    rend.color = new Color(rend.color.r, rend.color.g, rend.color.b, .6f);
    //    lockObject.myObject = gameObject;
    //}

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
        }
    }
}
