using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//虫子移动策略工厂
public static class CrawStrategyFactory
{
    //后续处理特殊情况使用
    public static ICrawMoveStrategy GetMoveStrategy(E_CrawMoveDir moveDir, Craw craw)
    {
        switch (moveDir)
        {
            case E_CrawMoveDir.Right:

                return new CrawRightMove();

            case E_CrawMoveDir.Left:
                return new CrawLeftMove();

            case E_CrawMoveDir.Up:
                return new CrawUpMove();

            case E_CrawMoveDir.Down:
                return new CrawDownMove();

            case E_CrawMoveDir.TopRight:
                return new CrawTopRightMove();

            case E_CrawMoveDir.TopLeft:
                return new CrawTopLeftMove();

            case E_CrawMoveDir.DownLeft:
                return new CrawDownLeftMove();
            case E_CrawMoveDir.DownRight:
                return new CrawDownRightMove();
        }
        return null;
    }


    //通用移动逻辑
    public static ICrawMoveStrategy GetGeneralMoveStrategy(E_CrawMoveDir moveDir, E_CrawMoveType moveType, Craw craw)
    {
        SetCrawRotation(moveDir, moveType, craw);
        return new CrawGenericMove();
    }

    //设置移动角度
    public static void SetCrawRotation(E_CrawMoveDir moveDir, E_CrawMoveType moveType, Craw craw)
    {
        float angle = 0;
        if (moveType == E_CrawMoveType.Straight)
        {
            angle = GetStriaghtRotation(moveDir, craw);
        }
        else if (moveType == E_CrawMoveType.Romdam)
        {
            angle = GetRomdamRotation(moveDir, craw);
        }

        //设置旋转
        craw.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    //直线型角度
    public static float GetStriaghtRotation(E_CrawMoveDir moveDir, Craw craw)
    {
        switch (moveDir)
        {
            case E_CrawMoveDir.Right:     return 0f;

            case E_CrawMoveDir.Left:      return 180f;
             
            case E_CrawMoveDir.Up:        return 90f;
          
            case E_CrawMoveDir.Down:      return 270f;
              
            case E_CrawMoveDir.TopRight:  return 45f;
             
            case E_CrawMoveDir.TopLeft:   return 135f;
            
            case E_CrawMoveDir.DownLeft:   return 225f;
          
            case E_CrawMoveDir.DownRight:  return 315f;
               
            default: return 0;
        }
    }
    //随机性角度
    public static float GetRomdamRotation(E_CrawMoveDir moveDir, Craw craw)
    {
        switch (moveDir)
        {
            case E_CrawMoveDir.Right:     return Random.Range(-90f,90f);

            case E_CrawMoveDir.Left:      return Random.Range(90f, 270f);
             
            case E_CrawMoveDir.Up:        return Random.Range(0f, 180f);
          
            case E_CrawMoveDir.Down:      return Random.Range(180, 360f);
              
            case E_CrawMoveDir.TopRight:  return Random.Range(0f, 90f);
             
            case E_CrawMoveDir.TopLeft:   return Random.Range(90f,180f);
            
            case E_CrawMoveDir.DownLeft:   return Random.Range(180f, 270f);
          
            case E_CrawMoveDir.DownRight:  return Random.Range(270f, 360f);
              
            default: return 0;
        }
    }
}
