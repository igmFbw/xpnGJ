using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class player : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float squatForce;
    [SerializeField] private Vector2 groundDetectDistance;
    [SerializeField] private Transform groundDetectPos;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private LayerMask kegLayer;
    [SerializeField] private Material[] leftMaterial;
    [SerializeField] private Material[] rightMaterial;
    [SerializeField] private MeshRenderer mr;
    [SerializeField] private softTrigger trigger;
    [SerializeField] private softBullet bulletPrefab;
    [SerializeField] private Blob softBlob;
    [SerializeField] private dieEffect[] dieEffects;
    [SerializeField] private playerSoundControl audioControl;
    [SerializeField] private float attackCool;
    private bool idleAnim;
    private float idleTimer;
    private int height;
    private int isFaceRight;
    private float cuJumpForce;
    private float cuSpeed;
    private float attackTimer;
    private void Awake()
    {
        trigger.sameTriggerEvent += sameTrigger;
        trigger.differentTriggerEvent += changeColor;
    }
    private void Start()
    {
        attackTimer = 0;
        idleAnim = true;
        idleTimer = 0;
        isFaceRight = 1;
        height = 100;
        cuJumpForce = jumpForce;
        cuSpeed = speed;
    }
    private void Update()
    {
        move();
        attackTimer += Time.deltaTime;
        if(Input.GetKeyDown(KeyCode.E))
            attack();
        if(Input.GetKeyDown(KeyCode.J))
        {
            float newScale = transform.localScale.y + .15f;
            softBlob.resize(newScale);
        }
        idleTimer += Time.deltaTime;
        if(idleTimer >= 1&&idleAnim)
        {
            idleTimer = 0;
            rb.velocity = new Vector2(rb.velocity.x, -20);
        }
    }
    private void OnDestroy()
    {
        trigger.sameTriggerEvent -= sameTrigger;
        trigger.differentTriggerEvent -= changeColor;
    }
    private void attack()
    {
        if (attackTimer < attackCool)
            return;
        attackTimer = 0;
        if(height <= 20)
        {
            return;
        }
        softBullet newBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        newBullet.init(trigger.isColorBlue, isFaceRight);
        height -= 20;
        float newScale = transform.localScale.y - .15f;
        softBlob.resize(newScale);
        calculateAttribute();
    }
    public void sameTrigger(GameObject go)
    {
        if (go.tag == "bucket")
        {
            height += go.GetComponent<softBucket>().bucketNum;
            if (height > 100)
                height = 100;
            audioControl.playEatBucketClip();
            calculateAttribute();
            Destroy(go);
            softBlob.resize(transform.localScale.y + .15f);
        }
        else if(go.tag == "wall")
            go.GetComponent<Collider2D>().isTrigger = true;
    }
    private void move()
    {
        float speedX = Input.GetAxisRaw("Horizontal") * speed;
        flip(speedX);
        float speedY = rb.velocity.y;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (groundDetect() && rb.velocity.y < .1f)
                speedY = jumpForce;
            idleAnim = false;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            speedY = -squatForce;
            idleAnim = false;
        }
        else
            idleAnim = true;
        rb.velocity = new Vector2(speedX, speedY);
    }
    private void flip(float speedX)
    {
        if (speedX > 0 && isFaceRight == -1)
        {
            isFaceRight = -isFaceRight;
            mr.material = rightMaterial[trigger.isColorBlue];
        }
        else if (speedX < 0 && isFaceRight == 1)
        {
            isFaceRight = -isFaceRight;
            mr.material = leftMaterial[trigger.isColorBlue];
        }
    }
    private bool groundDetect()
    {
        bool flag1 = Physics2D.OverlapBox(groundDetectPos.position, groundDetectDistance, 0, groundLayer);
        bool flag2 = Physics2D.OverlapBox(groundDetectPos.position, groundDetectDistance, 0, wallLayer);
        bool flag3 = Physics2D.OverlapBox(groundDetectPos.position, groundDetectDistance, 0, kegLayer);
        return flag1 || flag2 || flag3;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(groundDetectPos.position, groundDetectDistance);
    }
    private void changeColor()
    {
        if(isFaceRight == 1)
            mr.material = rightMaterial[trigger.isColorBlue];
        else
            mr.material = leftMaterial[trigger.isColorBlue];
    }
    private void die()
    {
        Instantiate(dieEffects[trigger.isColorBlue],transform.position,Quaternion.identity);
        Destroy(gameObject);
    }
    public void hurt(int damage)
    {
        height -= damage;
        if (height <= 0)
        {
            die();
            return;
        }
        calculateAttribute();
    }
    private void calculateAttribute()
    {
        float k = height / 2.5f;
        cuJumpForce = k * jumpForce;
        cuSpeed = k * speed;
    }
}