using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class CrawCursor :CurSorObj
{
    

    [LabelText("动画状态机"), SerializeField]
    private Animator animator;

    [LabelText("是否能消灭")]
    public bool canAttack = false;

    [LabelText("充能阈值时间")]
    private float thresholdTime = 1f;
    private float mouseClickTime;

    private Vector2 costStatPos;
    private Coroutine coroutine;
    protected void Awake()
    {
        animator = this.GetComponent<Animator>();
    }
    protected override void Update()
    {
        base.Update();
        //左键按下开始充能
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("开始充能");
            //记录时间
            mouseClickTime = Time.time;
            //开始充能
            animator.SetBool("AttackPre", true);
            animator.SetBool("Idle", false);

            canAttack = false;
        }
        else if (Input.GetMouseButton(0)) //左键按住充能
        {
            if (Time.time - mouseClickTime > thresholdTime && mouseClickTime >0)
            {
                animator.SetBool("AttackPre", false);
                //播放充能完成动画
                animator.SetBool("Attack", true);
                canAttack = true;
                //开始消耗debug点数
                Debug.Log("开始消耗debug点数");
                coroutine = StartCoroutine(CostDebugPoint());

                mouseClickTime = -1; //确保只进入一次
                
            }
        }
        else if (Input.GetMouseButtonUp(0))//左键松开
        {
            canAttack = false;

            //回到初始状态
            animator.SetBool("Attack", false);
            animator.SetBool("AttackPre", false);
            animator.SetBool("Idle", true);
            mouseClickTime = 0;

            //停止消耗协程
            StopCoroutine(coroutine);
        }
    }

    
    private IEnumerator CostDebugPoint()
    {

        if (currentDebugPoint <= 0)
        {
            Debug.Log("点数不够");
            yield break;
        }
        //记录开始位置
 
        costStatPos = this.gameObject.transform.position;
        yield return new WaitForSeconds(0.5f);//每隔0.5秒消耗一次debug点数
        float distance = Vector2.Distance(costStatPos, this.gameObject.transform.position);

        //消耗debug点数
        currentDebugPoint -= Mathf.Abs(distance);

        GameDataMgr.Instance.currentDebugPoint = currentDebugPoint;
        //继续检测
      
        yield return CostDebugPoint();
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        //碰撞着是 Layer是爬虫
        if (other.gameObject.layer == LayerMask.NameToLayer("Craw"))
        {
            if (canAttack)
            {
                Craw craw = other.GetComponent<Craw>();
                //切换到死亡状态
                craw.ChangeState(E_CrawStateType.Died);
            }
            else
            {
                Debug.Log("还未充能完成");
            }
        }

    }
    

}
