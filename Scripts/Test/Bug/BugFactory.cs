using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class BugFactory : MonoBehaviour
{



    [LabelText("bug 对象")] public GameObject bugPrefab;
    [LabelText("bug 数量")] public int bugCount = 10;
    [LabelText("水平方向生成的范围")] public float width = 10f;
    [LabelText("垂直方向生成的范围")] public float height = 10f;
    [LabelText("生成的 bug 间隔")] public float interval = 1f;
    void Start()
    {
    }
    public void CreateBug()
    {
        StartCoroutine(CreateBugCoroutine());
    }
    private IEnumerator CreateBugCoroutine()
    {
      
        for (int i = 0; i < bugCount; i++)
        {
            //随机生成位置
            float x = Random.Range(-width / 2, width / 2);
            float y = Random.Range(-height / 2, height / 2);
            Vector3 position = new Vector3(x, y, 0);
            //生成 bug
            GameObject bug = Instantiate(bugPrefab, position, Quaternion.identity);
            bug.transform.DOScale(1, 0.5f).From(0);

            yield return new WaitForSeconds(interval);
        }
        
    }
}
