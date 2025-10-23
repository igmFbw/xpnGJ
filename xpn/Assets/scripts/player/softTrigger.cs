using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class softTrigger : MonoBehaviour
{
    public int isColorBlue = 0;
    public Action differentTriggerEvent;
    public Action<GameObject> sameTriggerEvent;
    private bool canTrig;
    private void Start()
    {
        canTrig = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        wall newWall;
        if (collision.TryGetComponent<wall>(out newWall))
        {
            if (newWall.isColorBlue == isColorBlue)
            {
                if(tag == "player")
                    newWall.sameCollision();
                sameTriggerEvent?.Invoke(collision.gameObject);
            }
            else
            {
                if (!canTrig)
                    return;
                StartCoroutine(recoverTrigger());
                isColorBlue = (isColorBlue + 1) % 2;
                newWall.differentCollision();
                differentTriggerEvent?.Invoke();
            }
        }
        else if(collision.tag == "enemy")
        {
            enemyControl enemy = collision.GetComponent<enemyControl>();
            if(isColorBlue == enemy.color)
            {
                if (tag == "player")
                {
                    enemy.sameTrigger();
                    sameTriggerEvent?.Invoke(collision.gameObject);
                }
                //enemy.sameTrigger();
                //sameTriggerEvent?.Invoke(collision.gameObject);
            }
            else if(gameObject.tag == "bullet")
                enemy.die();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        wall newWall;
        if (collision.TryGetComponent<wall>(out newWall))
            newWall.collisionExit();
        else if (collision.tag == "enemy")
        {
            enemyControl enemy = collision.GetComponent<enemyControl>();
            enemy.recoverRb();
        }
    }
    private IEnumerator recoverTrigger()
    {
        canTrig = false;
        yield return new WaitForSeconds(1.5f);
        canTrig = true;
    }
}
