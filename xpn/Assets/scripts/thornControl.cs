using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class thornControl : MonoBehaviour
{
    [SerializeField] private int color;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Collider2D col;
    [SerializeField] private List<Sprite> sprites;
    private void Start()
    {
        sr.sprite = sprites[color];
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "mainPlayer")
        {
            if (gloablManager.instance.player.trigger.isColorBlue == color)
                col.isTrigger = true;
            else
                gloablManager.instance.player.hurt(100);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "mainPlayer")
            if (gloablManager.instance.player.trigger.isColorBlue == color)
                col.isTrigger = true;
    }
}
