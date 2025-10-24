using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UIElements.Experimental;
public class softBullet : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private softTrigger trigger;
    [SerializeField] private float speed;
    [SerializeField] private Blob softBody;
    [SerializeField] private MeshRenderer mr;
    [SerializeField] private Material[] materials;
    private void Awake()
    {
        trigger.differentTriggerEvent += changeColor;
        trigger.sameTriggerEvent += destroySelf;
        mr.sortingOrder = -1;
    }
    private void OnDestroy()
    {
        trigger.differentTriggerEvent -= changeColor;
        trigger.sameTriggerEvent -= destroySelf;
    }
    private void destroySelf(GameObject go)
    {
        //rb.velocity = new Vector2(facing * speed, 0);
        Destroy(gameObject, .5f);
    }
    private void Start()
    {
        Destroy(gameObject, 3);
    }
    public void init(int color,Vector3 facing)
    {
        StartCoroutine(fire(color,facing));
        /*trigger.isColorBlue = color;
        this.facing = facing;
        rb.velocity = new Vector2(facing * speed, 0);*/
    }
    private IEnumerator fire(int color, Vector3 facing)
    {
        yield return new WaitForSeconds(.1f);
        trigger.isColorBlue = color;
        rb.velocity = facing * speed;
        mr.material = materials[color];
    }
    private void changeColor()
    {
        mr.material = materials[trigger.isColorBlue];
        Destroy(gameObject, .5f);
    }
}