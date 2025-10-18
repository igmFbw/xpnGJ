using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class bossControl : MonoBehaviour
{
    [SerializeField] private Animator bodyAnim;
    [SerializeField] private bossHand[] hand;
    [SerializeField] private float handAttackDistance;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask kegLayer;
    private float attackTimer;
    private float attackCool;
    private int health;
    private int state;
    private int currentHand;
    private void Start()
    {
        attackTimer = 0;
        health = 100;
        attackCool = Random.Range(4f, 8f);
        state = 1;
    }
    private void Awake()
    {
        hand[0].attackAction += handAttack;
        hand[1].attackAction += handAttack;
    }
    private void OnDestroy()
    {
        hand[0].attackAction -= handAttack;
        hand[1].attackAction -= handAttack;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(hand[0].attackPos.position, handAttackDistance);
        Gizmos.DrawWireSphere(hand[1].attackPos.position, handAttackDistance);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            attack1();
        }
        attack();
    }
    public void hurt(int damage)
    {
        health -= damage;
        if (health <= 60)
            state = 2;
        else if (health <= 20)
            state = 3;
        else if (health <= 0)
            die();
    }
    private void die()
    {

    }
    private void attack()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackCool)
        {
            switch (state)
            {
                case 1:
                    int x1 = Random.Range(1, 101);
                    if (x1 <= 50)
                        attack1();
                    else 
                        attack2();
                    attackCool = Random.Range(4f, 8f);
                    break;
                case 2:
                    int x2 = Random.Range(1, 101);
                    if (x2 <= 35)
                        attack1();
                    else
                        attack2();
                    attackCool = Random.Range(4f, 8f);
                    break;
                case 3:
                    attack3();
                    break;
            }
            attackTimer = 0;
        }
    }
    private void attack1()
    {
        bodyAnim.SetBool("isAttack", true);
        currentHand = Random.Range(0, 2);
        hand[currentHand].attack();
        hand[1- currentHand].move();
    }
    private void attack2()
    {

    }
    private void attack3()
    {

    }
    public void attackEnd()
    {
        bodyAnim.SetBool("isAttack", false);
    }
    private void handAttack()
    {
        Collider2D playerCo = Physics2D.OverlapCircle(hand[currentHand].attackPos.position, handAttackDistance, playerLayer);
        if (playerCo != null)
            gloablManager.instance.player.hurt(100);
        Collider2D kegCo = Physics2D.OverlapCircle(hand[currentHand].attackPos.position, handAttackDistance, kegLayer);
        if (kegCo != null)
            hurt(10);
    }
}