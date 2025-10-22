using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//吸拽工具
public class DrapCursor : CurSorObj
{
    [SerializeField]
    public int costDebugNum; //消耗值

    private bool canDrop;
    private Animator animator; 

    protected override void Update()
    {
        base.Update();
        //长按进入吸取状态
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        
    }
}
