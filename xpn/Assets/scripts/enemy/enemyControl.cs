using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class enemyControl : MonoBehaviour
{
    private int facing;
    [SerializeField] private Collider2D col;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform leftPoint;
    [SerializeField] private Transform rightPoint;
    [SerializeField] private float speed;
    [SerializeField] private float followSpeed;
    [SerializeField] private Transform detectPos;
    [SerializeField] private Vector2 detectDistance;
    [SerializeField] private LayerMask playerLayer;
    public int color;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Animator anim;
    [SerializeField] private List<Color> colorList;
    private Vector3 leftPos;
    private Vector3 rightPos;
    private void Start()
    {
        facing = 1;
        sr.color = colorList[color];
        leftPos = leftPoint.position;
        rightPos = rightPoint.position;
    }
    private void Update()
    {
        if (transform.position.x >= rightPos.x && facing == 1)
            flip();
        else if (transform.position.x <= leftPos.x && facing == -1)
            flip();
        if(detectPlayer()&&gloablManager.instance.player.trigger.isColorBlue != color)
            rb.velocity = new Vector2(followSpeed * facing, rb.velocity.y);
        else
            rb.velocity = new Vector2(speed * facing, rb.velocity.y);
    }
    public void die()
    {
        Destroy(gameObject,.5f);
        anim.SetBool("isDie", true);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(detectPos.position, detectDistance);
    }
    private void flip()
    {
        facing *= -1;
        transform.Rotate(0, 180, 0);
    }
    private bool detectPlayer()
    {
        return Physics2D.OverlapBox(detectPos.position, detectDistance, 0, playerLayer);
    }
    public void sameTrigger()
    {
        rb.gravityScale = 0;
        col.isTrigger = true;
    }
    public void recoverRb()
    {
        rb.gravityScale = 1;
        col.isTrigger = false;
    }
    public void changeColor()
    {
        color = (color + 1) % 2;
        sr.color = colorList[color];
    }
    public void playAttack()
    {
        anim.SetBool("isAttack", true);
    }
    public void attack()
    {

    }
    public void attackEnd()
    {
        anim.SetBool("isAttack", false);
    }
}