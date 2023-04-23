using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public float fRotationY = 50f;
    void Update()
    {
        transform.Rotate(0, fRotationY * Time.deltaTime, 0);
    }
}
