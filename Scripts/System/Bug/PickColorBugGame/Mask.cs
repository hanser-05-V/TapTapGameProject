using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Mask : MonoBehaviour
{
    [ShowInInspector]
    public MaskData MaskData{ get; private set; }

    public void SetMaskData(MaskData data)
    {
        MaskData = data;
    }
}
