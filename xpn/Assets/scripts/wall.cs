using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class wall : MonoBehaviour
{
    public int isColorBlue;
    [SerializeField] private List<Sprite> colorList;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Collider2D wallCollider;
    [SerializeField] private Rigidbody2D rb;
    private void Start()
    {
        sr.sprite = colorList[isColorBlue];
    }
    public void differentCollision()
    {
        isColorBlue = (isColorBlue + 1) % 2;
        sr.sprite = colorList[isColorBlue];
    }
    public void sameCollision()
    {
        wallCollider.isTrigger = true;
        if (rb != null)
            rb.bodyType = RigidbodyType2D.Static;
            //rb.gravityScale = 0;
    }
    public void collisionExit()
    {
        wallCollider.isTrigger = false;
        if (rb != null)
            //rb.gravityScale = 1;
            rb.bodyType = RigidbodyType2D.Dynamic;
    }
}
