using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class button : MonoBehaviour
{
    [SerializeField] private Collider2D door;
    [SerializeField] private Animator anim;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private List<Sprite> sprites;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "mainPlayer"||collision.tag=="box")
        {
            //anim.SetBool("isOpen", true);
            //door.isTrigger = true;
            sr.sprite = sprites[1];
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "mainPlayer" || collision.tag == "box")
        {
            //anim.SetBool("isOpen", false);
            //door.isTrigger = false;
            sr.sprite = sprites[0];
        }
    }
}
