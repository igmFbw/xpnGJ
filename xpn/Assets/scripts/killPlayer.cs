using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class killPlayer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "player")
            gloablManager.instance.player.hurt(100);
    }
}
