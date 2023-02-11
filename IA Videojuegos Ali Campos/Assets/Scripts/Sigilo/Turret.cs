using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public Transform tPlayer;
    public Transform tHead;

    [Range(0f, 360f)]
    public float fVisionAngle = 30f;
    public float fVisionDistance = 10f;

    public bool bDetected = false;


    void Start()
    {
        
    }

    Vector2 v2PlayerVector = Vector2.zero;
    void Update()
    {
        bDetected = false;

        v2PlayerVector = tPlayer.position - tHead.position;

        if (Vector3.Angle(v2PlayerVector.normalized, tHead.right) < fVisionAngle * 0.5)
        {
            if (v2PlayerVector.magnitude < fVisionDistance)
            {
                bDetected = true;
            }
        }
    }

    private Vector2 v2P1, v2P2;
    private void OnDrawGizmos()
    {
        if (fVisionAngle <= 0f) return;

        float fHalfVisionAngle = fVisionAngle * 0.5f;

        v2P1 = PointForAngle(fHalfVisionAngle, fVisionDistance);
        v2P2 = PointForAngle(-fHalfVisionAngle, fVisionDistance);

        Gizmos.color = bDetected ? Color.green :  Color.red;
        Gizmos.DrawLine(tHead.position, (Vector2)tHead.position + v2P1);
        Gizmos.DrawLine(tHead.position, (Vector2)tHead.position + v2P2);

        Gizmos.DrawRay(tHead.position, tHead.right * 4f);
    }

    Vector3 PointForAngle(float fAngle, float fDistance)
    {
        return tHead.TransformDirection
            (
                new Vector2
                (
                    Mathf.Cos(fAngle * Mathf.Deg2Rad),
                    Mathf.Sin(fAngle * Mathf.Deg2Rad)
                )
            ) * fDistance;
    }
}
