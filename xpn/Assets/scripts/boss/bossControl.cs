using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class bossControl : MonoBehaviour
{
    [SerializeField] private Animator bodyAnim;
    [SerializeField] private bossHand[] hand;
    [SerializeField] private float handAttackDistance;
    private float attackTimer;
    private float attackCool;
    private int health;
    private int state;
    private int currentHand;
    private void Start()
    {
        attackTimer = 0;
        health = 100;
        attackCool = Random.Range(5f, 8f);
        state = 1;
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
        if (Input.GetKeyDown(KeyCode.H))
        {
            attack2();
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
        if(state == 1)
        {
            bodyAnim.SetBool("isAttack", true);
            currentHand = Random.Range(0, 2);
            hand[currentHand].attack();
            hand[1 - currentHand].move();
        }
        else
        {
            bodyAnim.SetBool("isAttack", true);
            hand[0].attack();
            hand[1].attack();
        }
    }
    private void attack2()
    {
        if (state == 1)
        {
            currentHand = Random.Range(0, 2);
            hand[currentHand].rayAttack();
        }
        else
        {
            hand[0].rayAttack();
            hand[1].rayAttack();
        }
    }
    private void attack3()
    {

    }
    public void attackEnd()
    {
        bodyAnim.SetBool("isAttack", false);
    }
}