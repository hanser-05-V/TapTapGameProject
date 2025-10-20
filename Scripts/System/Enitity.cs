using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary> 游戏实体类 包含创建对象的公共属性/行为
/// 
/// </summary>
public class Enitity : MonoBehaviour
{
    // 状态机
    protected FSM fSM;
    [LabelText("角色状态机"), SerializeField] //TODO：测试使用
    public Animator Animator { get; private set; }
    
    protected virtual void Awake()
    {
        fSM = new FSM();
        //获得组件
        Animator = this.GetComponent<Animator>();
    }
    protected virtual void Start()
    {
        
    }
    protected virtual void Update()
    {
        //让状态机更新状态运行
        fSM.CurrentState.OnUpdate();
    }
}
