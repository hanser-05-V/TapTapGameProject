using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Band : MonoBehaviour
{
    private Collider2D bandCollider;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    
    private void Start()
    {
        // 获取 Band 对象的 Collider2D
        bandCollider = GetComponent<Collider2D>();
        //获得组件
        spriteRenderer = GetComponent<SpriteRenderer>();
        //渐渐淡出
        spriteRenderer.DOFade(0, 1.5f).OnComplete(() =>
        {
            //TODO:缓存池 优化
            GameObject.Destroy(gameObject);
            // PoolMgr.Instance.PushObj(gameObject);
        });
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //如果碰到的是污染物
        if (other.gameObject.tag == "Pollution")
        {
         
             // 判断污染物是否完全在 Band 的碰撞体内
            if (IsPollutionInsideBand(other))
            {
          
                //TODO:切换污染到死亡状态
                PoolMgr.Instance.PushObj(other.gameObject);
            }
        }
    }
    
     // 判断污染物是否完全在 Band 的碰撞体内
    private bool IsPollutionInsideBand(Collider2D pollutionCollider)
    {
        // 获取污染物和 Band 的碰撞体边界
        Bounds bandBounds = bandCollider.bounds;
        Bounds pollutionBounds = pollutionCollider.bounds;

        // 检查污染物是否完全被 Band 的边界包围
        if (bandBounds.Contains(pollutionBounds.min) && bandBounds.Contains(pollutionBounds.max))
        {
            return true;
        }

        return false;
    }
}
