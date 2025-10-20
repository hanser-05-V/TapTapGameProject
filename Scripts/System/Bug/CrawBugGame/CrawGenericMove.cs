using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawGenericMove : ICrawMoveStrategy
{
    public override void Move(Craw craw, E_CrawMoveType moveType)
    {
        craw.transform.Translate(Vector3.right * craw.crawData.moveSpeed * Time.deltaTime, Space.Self);
    }
}
