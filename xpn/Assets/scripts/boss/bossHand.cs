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
    [SerializeField] private Transform rayPos;
    [SerializeField] private bossRayDetect rayDetectPos;
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip hitClip;
    [SerializeField] private AudioClip rayClip;
    public Action<int> hurtAction;
    public Transform attackPos;
    private void Awake()
    {
        rayDetectPos.rayDetectAction += rayDetect;
    }
    private void OnDestroy()
    {
        rayDetectPos.rayDetectAction -= rayDetect;
    }
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
        audioSource.clip = hitClip;
        audioSource.Play();
        if (gloablManager.instance.player.trigger.isColorBlue == color)
            return;
        Collider2D playerCo = Physics2D.OverlapCircle(attackPos.position, handAttackDistance, playerLayer);
        if (playerCo != null)
            gloablManager.instance.player.hurt(100);
        Collider2D kegCo = Physics2D.OverlapCircle(attackPos.position, handAttackDistance, kegLayer);
        if (kegCo != null)
        {
            Debug.Log("1");
            if (kegCo.GetComponent<wall>().isColorBlue != color)
            {
                gloablManager.instance.boss.hurt(10);
                Destroy(kegCo.transform.gameObject);
                GameObject newEffect = Instantiate(explosionEffect, kegCo.transform.position, Quaternion.identity);
                Destroy(newEffect, .2f);
            }
        }
    }
    public void rayKey()
    {
        lr.positionCount = 2;
        lr.SetPosition(0,rayPos.position);
        rayDetectPos.gameObject.SetActive(true);
        audioSource.clip = rayClip;
        audioSource.Play();
    }
    public void clearRayPoint()
    {
        lr.positionCount = 0;
        rayDetectPos.gameObject.SetActive(false);
    }
    public void rayDetect()
    {
        Vector2 dir = (rayDetectPos.transform.position - rayPos.position).normalized;
        var go = Physics2D.RaycastAll(rayPos.position, dir, Mathf.Infinity);
        if (go == null)
        {
            lr.SetPosition(1, rayDetectPos.transform.position);
            return;
        }
        System.Array.Sort(go, (a, b) => a.distance.CompareTo(b.distance));
        int n = go.Length;
        for (int i = 0; i < n; i++)
        {
            if (go[i].transform.tag != "wall")
            {
                if (go[i].transform.tag == "powerkeg")
                {
                    if (go[i].transform.GetComponent<wall>().isColorBlue != color)
                    {
                        Destroy(go[i].transform.gameObject);
                        GameObject newEffect = Instantiate(explosionEffect, go[i].transform.position, Quaternion.identity);
                        Destroy(newEffect, .2f);
                    }
                }
                if (go[i].transform.tag == "player")
                    if (gloablManager.instance.player.trigger.isColorBlue!=color)
                        gloablManager.instance.player.hurt(20);
                lr.SetPosition(1, go[i].point);
                break;
            }
            if (go[i].transform.GetComponent<wall>().isColorBlue != color)
            {
                lr.SetPosition(1, go[i].point);
                break;
            }
        }
    }
}