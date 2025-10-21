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
                //newWall.sameCollision();
                if (!canTrig)
                return;
                StartCoroutine(recoverTrigger());
                if(tag == "player")
                    newWall.sameCollision();
                sameTriggerEvent?.Invoke(collision.gameObject);
            }
            else
            {
                isColorBlue = (isColorBlue + 1) % 2;
                newWall.differentCollision();
                differentTriggerEvent?.Invoke();
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        wall newWall;
        if (collision.TryGetComponent<wall>(out newWall))
            newWall.collisionExit();
    }
    private IEnumerator recoverTrigger()
    {
        canTrig = false;
        yield return new WaitForSeconds(.5f);
        canTrig = true;
    }
}
