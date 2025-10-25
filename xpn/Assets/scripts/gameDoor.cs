using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class gameDoor : MonoBehaviour
{
    [SerializeField] private Animator anim;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "mainPlayer")
            anim.SetBool("isOpen", true);
    }
    public void win()
    {
        SceneManager.LoadScene(gloablManager.instance.gameIndex + 1);
    }
}
