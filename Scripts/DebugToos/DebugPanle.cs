using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DebugPanle : MonoBehaviour
{
    public GameObject PopupGameObject;
    public GameObject CrawObj;
    public GameObject SeleBugGame;
    public GameObject SeleCrawGame;

    private float defultScale;
    private void Awake()
    {
        defultScale = PopupGameObject.transform.localScale.x;
    }
    
    //恢复默认缩放
    public void RecoverScale()
    {
        
        PopupGameObject.transform.localScale = new Vector3(defultScale, defultScale, defultScale);
        CrawObj.transform.localScale = new Vector3(defultScale, defultScale, defultScale);
        SeleBugGame.transform.localScale = new Vector3(defultScale, defultScale, defultScale);
        SeleCrawGame.transform.localScale = new Vector3(defultScale, defultScale, defultScale);
    
    }
}
