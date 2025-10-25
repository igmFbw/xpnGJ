using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class kegControl : MonoBehaviour
{
    [SerializeField] private wall wall;
    private void Awake()
    {
        wall.isColorBlue = Random.Range(0, 2);
    }
}
