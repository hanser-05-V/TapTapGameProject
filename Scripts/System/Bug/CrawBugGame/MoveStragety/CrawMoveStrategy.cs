using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//虫子移动策略
public abstract class ICrawMoveStrategy 
{
    public abstract void Move(Craw craw,E_CrawMoveType moveType);
}
