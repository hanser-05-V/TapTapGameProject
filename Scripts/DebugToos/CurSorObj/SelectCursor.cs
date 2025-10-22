using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

public class SelectCursor : CurSorObj
{

    private bool isDrawinng; //当前是否正在拖拽

    public GameObject BoxImage; //显示的选择框图片
    private Vector3 startPos; //开始拖拽的位置
    private Vector3 endPos; //结束拖拽的位置

    private Vector3 mousePos; //鼠标位置
    private Vector3 mouseWorldPos; //鼠标世界坐标

    public GameObject Band; //实习布（模拟创口贴）
    

    void Awake()
    {
        BoxImage.GetComponent<SpriteRenderer>().DOFade(0f, 0f); //隐藏选择框图片
    }
    override protected void Update()
    {
        base.Update();
        //得到鼠标操作
        GetMouse();
    }

    private void GetMouse()
    {
        //获得鼠标世界坐标
        mousePos = Input.mousePosition;
        mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);

        //点击开始绘图
        if (Input.GetMouseButtonDown(0))
        {
            startPos = mouseWorldPos;
            startPos.z = 0;
            endPos = mouseWorldPos;
            isDrawinng = true;

        }
        else if (Input.GetMouseButton(0))
        {
            //更新endPos;
            endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            endPos.z = 0;

            //绘制选择框
            DrawBox();

        }
        else if (Input.GetMouseButtonUp(0))
        {
            //进行Debug点数消耗
            GameDataMgr.Instance.currentDebugPoint -= Mathf.Abs(Vector3.Distance(startPos, endPos));
            //结束绘图
            isDrawinng = false;
            //判断能否创造
            if (currentDebugPoint >= 0)
            {
                CreateBand(); //点数足够，创建实习布
            }
            else
            {
                Debug.Log("点数不足,不能创造Band");
            }
        }
        else
        {
            BoxImage.GetComponent<SpriteRenderer>().DOFade(0f, 0f); //隐藏选择框图片
        }
    }

    //创建实习布（模拟创口贴）
    private void CreateBand()
    {
        //TODO:缓存池 优化
        GameObject band = Instantiate(Band);
          //设置选择框图片的位置为起始点
        band.transform.position = new Vector3(startPos.x, startPos.y, 0);
        
        // 计算选择框的尺寸（结束点减去起始点得到宽高）
        var scale = new Vector3(endPos.x, endPos.y, 0) - new Vector3(startPos.x, startPos.y, 0);
        
        // 设置SpriteRenderer的尺寸，使图片拉伸到计算出的选择框大小
        band.GetComponent<SpriteRenderer>().size = new Vector2(scale.x, -scale.y);
    }
    //更改图片大小来实现绘图
    private void DrawBox()
    {
        if (!isDrawinng)
            return;
        //设置选择框图片的位置为起始点
        BoxImage.transform.position = new Vector3(startPos.x, startPos.y, 0);
        
        // 计算选择框的尺寸（结束点减去起始点得到宽高）
        var scale = new Vector3(endPos.x, endPos.y, 0) - new Vector3(startPos.x, startPos.y, 0);
        
        // 设置SpriteRenderer的尺寸，使图片拉伸到计算出的选择框大小
        BoxImage.GetComponent<SpriteRenderer>().size = new Vector2(scale.x, -scale.y);
        
        //0.5秒内将透明度渐变到1（完全显示）
        BoxImage.GetComponent<SpriteRenderer>().DOFade(1f, 0.5f);
    
    }
}
