using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class gloablManager : MonoBehaviour
{
    public static gloablManager instance;
    public player player;
    public bossControl boss;
    private void Awake()
    {
        instance = this;
    }
    private void OnDestroy()
    {
        instance = null;
    }
}
