using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
public class bossRayDetect : MonoBehaviour
{
    public Action rayDetectAction;
    [SerializeField] private int facing;
    private void OnEnable()
    {
        transform.position = new Vector2(Camera.main.transform.position.x + (facing * 12f), transform.position.y);
        transform.DOMoveX(Camera.main.transform.position.x - (facing * 12f), 1f);
    }
    private void Update()
    {
        rayDetectAction?.Invoke();
    }
}