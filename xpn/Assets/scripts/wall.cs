using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class wall : MonoBehaviour
{
    public int isColorBlue;
    [SerializeField] private List<Color> colorList;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Collider2D wallCollider;
    private void Start()
    {
        sr.color = colorList[isColorBlue];
    }
    public void differentCollision()
    {
        isColorBlue = (isColorBlue + 1) % 2;
        sr.color = colorList[isColorBlue];
    }
    public void sameCollision()
    {
        wallCollider.isTrigger = true;
    }
    public void collisionExit()
    {
        wallCollider.isTrigger = false;
    }
}
