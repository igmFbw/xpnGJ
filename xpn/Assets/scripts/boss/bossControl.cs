using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class bossControl : MonoBehaviour
{
    [SerializeField] private Animator bodyAnim;
    [SerializeField] private bossHand[] hand;
    [SerializeField] private float handAttackDistance;
    [SerializeField] private CanvasGroup finaCg;
    [SerializeField] private Image finaImage;
    [SerializeField] private List<Color> finaColor;
    [SerializeField] private AudioSource audioPlayer;
    private float attackTimer;
    private float attackCool;
    private int health;
    private int state;
    private int currentHand;
    private bool isFina;
    private int finaIndex;
    private void Awake()
    {
        hand[0].hurtAction += hurt;
        hand[1].hurtAction += hurt;
    }
    private void OnDestroy()
    {
        hand[0].hurtAction -= hurt;
        hand[1].hurtAction -= hurt;
    }
    private void Start()
    {
        attackTimer = 0;
        health = 100;
        attackCool = Random.Range(5f, 8f);
        state = 1;
        isFina = false;
        finaIndex = 0;
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
        if (Input.GetKeyDown(KeyCode.G))
        {
            state = 3;
        }
        if(state != 3)
            attack();
        else
        {
            if (isFina)
                return;
            StartCoroutine(attack3());
            finaCg.gameObject.SetActive(true);
            finaCg.DOFade(1, 1);
            isFina = true;
        }
    }
    public void hurt(int damage)
    {
        health -= damage;
        if (health <= 60)
            state = 2;
        else if (health <= 20)
        {
            attackTimer = 1.5f;
            state = 3;
            finaCg.gameObject.SetActive(true);
            finaCg.DOFade(1, 1);
        }
        else if (health <= 0)
            die();
    }
    private void die()
    {
        audioPlayer.Play();
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
    private IEnumerator attack3()
    {
        yield return new WaitForSeconds(1);
        while(true)
        {
            finaImage.DOColor(finaColor[finaIndex], 1.5f);
            hurt(2);
            yield return new WaitForSeconds(2.5f);
            if (gloablManager.instance.player.trigger.isColorBlue != finaIndex)
                gloablManager.instance.player.hurt(100);
            finaIndex = (finaIndex + 1) % 2;
        }
    }
    public void attackEnd()
    {
        bodyAnim.SetBool("isAttack", false);
    }
}