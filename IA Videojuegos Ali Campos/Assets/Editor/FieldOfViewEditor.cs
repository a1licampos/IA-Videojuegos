using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PatrolAgentFSM))]
public class FieldOfViewEditor : Editor
{
    private void OnSceneGUI()
    {
        PatrolAgentFSM FOVref = (PatrolAgentFSM)target;
        Vector3 FOVPos = FOVref.transform.position;

        //Por defecto inicializamos estas variables a los valores del estado inicial patrullaje
        float fTmpAngle = FOVref.fVisionAngle;
        float fTmpDist = FOVref.fVisionDist;

        //Si estamos en el de Alerta, sobrescribimos los valores por los del estado de alerta.
        //OJO AQUÍ CON EL OPERADOR DE '?' que nos está haciendo el chequo ante el null.
        if(FOVref.CurrentState?.name == "Alert")
        {
            fTmpAngle = FOVref.fAlertVisionAngle;
            fTmpDist = FOVref.fAlertVisionDist;
        }

        Handles.color = Color.white;
        Handles.DrawWireArc(FOVPos, Vector3.up, FOVref.transform.forward, fTmpAngle ,fTmpDist, 3.0f);

        Vector3 leftLine = PointForAngle(FOVref.transform, -fTmpAngle * 0.5f, fTmpDist);
        Vector3 rightLine = PointForAngle(FOVref.transform, fTmpAngle * 0.5f, fTmpDist);

        Handles.color = Color.green;
        Handles.DrawLine(FOVPos, FOVPos + leftLine);
        Handles.DrawLine(FOVPos, FOVPos + rightLine);

        Handles.color = Color.yellow;
        Handles.DrawLine(FOVPos, FOVPos + FOVref.transform.forward * fTmpDist);

    }

    private Vector3 PointForAngle(Transform ObjTransform, float fAngle, float fDistance)
    {
        float fEulerY = ObjTransform.transform.eulerAngles.y;

        float fAngleRads = Mathf.Deg2Rad * (fEulerY + fAngle);
        return (new Vector3(Mathf.Sin(fAngleRads), 0.0f, Mathf.Cos(fAngleRads)) * fDistance);
    }
}
