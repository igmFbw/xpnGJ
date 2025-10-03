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
    [SerializeField] private Material leftMaterial;
    [SerializeField] private Material rightMaterial;
    [SerializeField] private MeshRenderer mr;
    [SerializeField] private softTrigger trigger;
    [SerializeField] private softBullet bulletPrefab;
    [SerializeField] private Blob softBlob;
    private int height;
    private int isFaceRight;
    private void Awake()
    {
        trigger.sameTriggerEvent += eatBucket;
    }
    private void Start()
    {
        isFaceRight = 1;
        height = 100;
    }
    private void Update()
    {
        move();
        if(Input.GetKeyDown(KeyCode.E))
            attack();
    }
    private void OnDestroy()
    {
        trigger.sameTriggerEvent -= eatBucket;
    }
    private void attack()
    {
        if(height <= 20)
        {
            return;
        }
        softBullet newBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        newBullet.init(trigger.isColorBlue, isFaceRight);
        height -= 20;
        //transform.localScale = new Vector3(transform.localScale.x - .15f,transform.localScale.y - .15f,1);
        //float newScale = transform.localScale.y - .15f;
        //softBlob.resize(newScale);
    }
    public void eatBucket(GameObject go)
    {
        if (go.tag != "bucket")
            return;
        height += 20;
        if (height > 100)
            height = 100;
        Destroy(go);
        //transform.localScale = new Vector3(transform.localScale.x + .15f, transform.localScale.y + .15f, 1);
        //softBlob.resize(transform.localScale.y + .15f);
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
        }
        else if (Input.GetKey(KeyCode.S))
            speedY = -squatForce;
        rb.velocity = new Vector2(speedX, speedY);
    }
    private void flip(float speedX)
    {
        if (speedX > 0 && isFaceRight == -1)
        {
            isFaceRight = -isFaceRight;
            mr.material = rightMaterial;
        }
        else if (speedX < 0 && isFaceRight == 1)
        {
            isFaceRight = -isFaceRight;
            mr.material = leftMaterial;
        }
    }

    private bool groundDetect()
    {
        return Physics2D.OverlapBox(groundDetectPos.position, groundDetectDistance, 0, groundLayer);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(groundDetectPos.position, groundDetectDistance);
    }
}