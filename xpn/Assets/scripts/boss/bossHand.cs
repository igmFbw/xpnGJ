using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class bossHand : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private int color;
    public Transform attackPos;
    public Action attackAction;
    public void attack()
    {
        anim.SetBool("isAttack", true);
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
        attackAction?.Invoke();
    }
}