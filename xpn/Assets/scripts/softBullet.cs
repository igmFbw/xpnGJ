using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;
public class softBullet : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private softTrigger trigger;
    [SerializeField] private float speed;
    private int facing;
    private void Awake()
    {
        trigger.differentTriggerEvent += destroySelf;
        trigger.sameTriggerEvent += resetVelocity;
    }
    private void OnDestroy()
    {
        trigger.differentTriggerEvent -= destroySelf;
        trigger.sameTriggerEvent -= resetVelocity;
    }
    private void destroySelf()
    {
        Destroy(gameObject, .8f);
    }
    private void Start()
    {
        Destroy(gameObject, 3);
    }
    public void init(int color,int facing)
    {
        trigger.isColorBlue = color;
        this.facing = facing;
        rb.velocity = new Vector2(facing * speed, 0);
    }
    public void resetVelocity(GameObject go)
    {
        rb.velocity = new Vector2(facing * speed, 0);
        Debug.Log(rb.velocity);
    }
}