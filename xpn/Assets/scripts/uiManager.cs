using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class uiManager : MonoBehaviour
{
    public static uiManager instance;
    [SerializeField] private List<healthBar> healthImage;
    private void Awake()
    {
        instance = this;
    }
    public void updateHealth(int health)
    {
        int x = health / 20;
        x = x > 0 ? x : 0;
        for(int i=0;i<x;i++)
            healthImage[i].setActive(true);
        for(int i=x;i<5;i++)
            healthImage[i].setActive(false);
    }
}
