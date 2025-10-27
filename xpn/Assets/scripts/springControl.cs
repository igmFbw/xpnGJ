using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class springControl : MonoBehaviour
{
    [SerializeField] private float power;
    [SerializeField] private Animator anim;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "mainPlayer")
        {
            gloablManager.instance.player.GetComponent<Rigidbody2D>().AddForce(new Vector2(0,power), ForceMode2D.Impulse);
            anim.SetBool("isSpring", false);
        }
    }
}
