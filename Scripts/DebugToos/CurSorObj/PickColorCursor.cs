using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickColorCursor : CurSorObj
{
    public int cost = 1;
    public Color pickColor; //拾取到的颜色

    private Color defultColor = Color.white; //默认颜色
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private LayerMask whatIsColorChange; //可以改变COlor的层级

    private void Awake()
    {
       
    }

    protected override void Update()
    {
        base.Update();
        if (Input.GetMouseButtonDown(0))
        {
    
             // 使用2D射线检测
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hitInfo = Physics2D.GetRayIntersection(ray, 1000, whatIsColorChange);
            Debug.Log("鼠标点击");
            if (hitInfo.collider != null)
            {
                GameObject hitObject = hitInfo.transform.gameObject;

                if (hitObject.CompareTag("PickColor"))
                {
                    Debug.Log("处理PickColor物体: " + hitObject.name);
                    //得到脚本数据
                    Mask mask = hitObject.GetComponent<Mask>();
                    if (mask != null)
                    {
                        //判断当前颜色和解锁颜色是否相同
                        if (pickColor == mask.MaskData.unlockColor)
                        {
                            Debug.Log("颜色正确");
                            //删除Mask  //TODO:缓存池优化
                            Destroy(hitObject);
                        }
                        else
                        {
                            Debug.Log("颜色错误");
                            Debug.Log("pickCololr" + pickColor + " unlockColor" + mask.MaskData.unlockColor);
                        }
                    }
                    else
                    {
                        Debug.LogError("未挂载Mask脚本");
                    }
                }
                else
                {
                    Debug.Log("吸取颜色");
                    GameDataMgr.Instance.currentDebugPoint -= cost;
                    StartCoroutine(PickColorAtMousePosition());
                }
            }

            else
            {
                Debug.Log("未检测到有效物体");
            }
            
        }
        
    }

      
    private IEnumerator PickColorAtMousePosition()
    {        
        // 等待一帧结束，确保在渲染完成后读取像素
        yield return new WaitForEndOfFrame();

        // 创建一个纹理来读取屏幕像素
        Texture2D tex = new Texture2D(1, 1, TextureFormat.RGB24, false);
        Vector2 mousePos = Input.mousePosition;
        
        // 读取鼠标位置处的像素
        tex.ReadPixels(new Rect(mousePos.x, mousePos.y, 1, 1), 0, 0);
        tex.Apply();

        // 获取颜色
        pickColor = tex.GetPixel(0, 0);

        //根据RGB 范围来限定颜色  //TODO:颜色范围

        //红色 (R最大)
        if (pickColor.r > pickColor.g && pickColor.r > pickColor.b)
        {
            pickColor = Color.red;
        }
        else if (pickColor.g > pickColor.r && pickColor.g > pickColor.b)
        {
            pickColor = Color.green;
        }
        else if (pickColor.b > pickColor.r && pickColor.b > pickColor.g)
        {
            pickColor = Color.blue;
        }
        else
        {
            pickColor = defultColor;
        }

        //更改拾色器的颜色
        spriteRenderer.color = pickColor;
        // 销毁临时纹理
        Destroy(tex);
    }
}
