using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class kegBorn : MonoBehaviour
{
    [SerializeField] private List<Transform> bornPos;
    [SerializeField] private GameObject kegPrefab;
    private float bornTimer;
    private float bornCool;
    private void Start()
    {
        bornTimer = 0;
        bornCool = 5;
    }
    private void Update()
    {
        bornTimer += Time.deltaTime;
        if (bornTimer >= bornCool)
        {
            bornTimer = 0;
            Instantiate(kegPrefab, bornPos[Random.Range(0,6)].position,Quaternion.identity);
        }
    }
}