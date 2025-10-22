using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownObj : MonoBehaviour
{
    private Rigidbody2D rb;
    public bool canFall = true; //是否可以下坠

    private Collider2D boxCollider; //物体的碰撞体
    private GameObject fallTrigger; //下落物体

    private Transform startTrans; //物体的初始位置

    private void Awake()
    {
        rb = this.GetComponent<Rigidbody2D>();
        boxCollider = this.GetComponent<Collider2D>();
        startTrans = this.gameObject.transform;
    }

    private void Start()
    {
        if (canFall)
            StartFall();
    }

    //开始下坠
    private void StartFall()
    {
        //设置重力
        rb.gravityScale = 1;
        //先清楚速度
        rb.velocity = Vector2.zero;
        //往下坠落
        rb.velocity = new Vector2(1.5f, -1);

        //TODO:加入旋转

        //在下落位置创建一个碰撞体
        CreateFallTrigger();

        // Invoke("CreateFallTrigger", 1.5f);
    }
    //停止下坠
    private void StopFall()
    {
        //归灵速度
        rb.velocity = Vector2.zero;
        //取出重力
        rb.gravityScale = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //碰撞到地面
        if (collision.gameObject.tag == "Ground")
        {
            Debug.Log("碰到地面");
            StopFall();
        }
        //碰撞到原来位置触发器
        if (collision.gameObject.tag == "FallTrigger")
        {
            ResetState();
        }
    }

    // 创建一个与物体大小相同的碰撞体作为下落触发器
    private void CreateFallTrigger()
    {
        // 创建一个新的GameObject作为触发器
        fallTrigger = new GameObject("FallTrigger");
        //设置Tag为 FallTrigger
        fallTrigger.tag = "FallTrigger";

        // 使它和物体大小相同
        BoxCollider2D triggerCollider = fallTrigger.AddComponent<BoxCollider2D>();
        triggerCollider.isTrigger = true; // 设置为触发器

        // 设置触发器的位置和大小
        triggerCollider.size = boxCollider.bounds.size;
        fallTrigger.transform.position =this.transform.position + new Vector3(0, -boxCollider.bounds.extents.y, 0); // 设置触发器位置在物体下方
    }
    
    //恢复状态
    private void ResetState()
    {
        Debug.Log("恢复状态");


        // //清楚速度
        // rb.velocity = Vector2.zero;
        // //清楚重力
        // rb.gravityScale = 0;
        // //恢复位置
        // this.transform.position = startTrans.position;
        // //角度恢复
        // this.transform.rotation = startTrans.rotation;

    }
   

}
