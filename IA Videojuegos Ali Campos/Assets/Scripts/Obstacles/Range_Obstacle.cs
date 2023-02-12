using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Range_Obstacle : MonoBehaviour
{
    public float fRadius;

    // Update is called once per frame
    void Update()
    {
        GetComponent<SphereCollider>().radius = fRadius;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, fRadius);
    }
}
