using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class boxBornPoint : MonoBehaviour
{
    [SerializeField] private GameObject boxPrefab;
    private bool canBorn;
    private void Start()
    {
        canBorn = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!canBorn)
            return;
        if(collision.tag == "mainPlayer")
        {
            StartCoroutine(recoverCanBorn());
        }
    }
    private IEnumerator recoverCanBorn()
    {
        canBorn = false;
        yield return new WaitForSeconds(1f);
        Instantiate(boxPrefab, transform.position, Quaternion.identity);
        canBorn = true;
    }
}
