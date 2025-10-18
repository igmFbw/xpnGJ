using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class bossHand : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private int color;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask kegLayer;
    [SerializeField] private float handAttackDistance;
    [SerializeField] private LineRenderer lr;
    [SerializeField] private Material rayMaterial;
    public Transform attackPos;
    public void attack()
    {
        anim.SetBool("isAttack", true);
    }
    public void rayAttack()
    {
        anim.SetBool("isRay", true);
    }
    public void rayEnd()
    {
        anim.SetBool("isRay", false);
    }
    public void attackEnd()
    {
        anim.SetBool("isAttack", false);
    }
    public void move()
    {
        anim.SetBool("isMove", true);
    }
    public void moveEnd()
    {
        anim.SetBool("isMove", false);
    }
    public void attackKey()
    {
        Collider2D playerCo = Physics2D.OverlapCircle(attackPos.position, handAttackDistance, playerLayer);
        if (playerCo != null)
            gloablManager.instance.player.hurt(100);
        Collider2D kegCo = Physics2D.OverlapCircle(attackPos.position, handAttackDistance, kegLayer);
        if (kegCo != null)
            gloablManager.instance.boss.hurt(10);
    }
    public void rayKey()
    {

    }
}