using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPool : MonoBehaviour
{
    public float speed = 10f;

    private void Update()
    {
        this.transform.position += Vector3.forward * Time.deltaTime * speed;

    }
   
   
}
