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

    public Vector3 v3AgentPosition = Vector3.zero;

    [Header("STATES")]
    public States_Turret sStates_Turrent;

    Vector2 v2PlayerVector = Vector2.zero;


    void Update()
    {
        try
        {
            tPlayer = GameObject.FindGameObjectWithTag("James Bond").GetComponent<Transform>();     //Buscamos la posicion del agente

            bDetected = false;

            v2PlayerVector = tPlayer.position - tHead.position;                                     //Punta menos cola

            if (Vector3.Angle(v2PlayerVector.normalized, tHead.right) < fVisionAngle * 0.5)         //Comprobamos que el agente no este dentro del campo de vision
            {
                if (v2PlayerVector.magnitude < fVisionDistance)
                {
                    bDetected = true;                                                               //Entro en el campo de vision
                    v3AgentPosition = tPlayer.position;
                }
            }

        }catch
        {
            Debug.Log("Se destruyó el agente James Bond...");
        }
    }

    private Vector2 v2P1, v2P2;
    private float fHalfVisionAngle;
    private void OnDrawGizmos()
    {
        if (fVisionAngle <= 0f) return;

        //Divimos el cono en dos para trabajar mejor
        fHalfVisionAngle = fVisionAngle * 0.5f;

        v2P1 = PointForAngle(fHalfVisionAngle, fVisionDistance);    //La mitad del cono
        v2P2 = PointForAngle(-fHalfVisionAngle, fVisionDistance);   //La mitad del otro cono

        //Estado normal al estado de ataque
        if (!sStates_Turrent.bAttack)
            Gizmos.color = sStates_Turrent.bAlert ? Color.yellow : Color.white;
        else
            Gizmos.color = Color.red;   //Estado de ataque

        //Dibujamos el cono de vision completo
        Gizmos.DrawLine(tHead.position, (Vector2)tHead.position + v2P1);
        Gizmos.DrawLine(tHead.position, (Vector2)tHead.position + v2P2);

        Gizmos.DrawRay(tHead.position, tHead.right * 4f);
    }

    Vector3 PointForAngle(float fAngle, float fDistance)           //Creamos el cono a la distancia establecida
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
