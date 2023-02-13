using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Click_Waypoint : MonoBehaviour
{
    public MousePoint_Creator sMousePoint_Creator;
    public Moving sMoving;

    public TextMeshProUGUI txtNumber;

    public int i;


    void Update()
    {
        if (sMousePoint_Creator == null)    //Buscamos asignar los scripts
            sMousePoint_Creator = GameObject.FindGameObjectWithTag("Creator").GetComponent<MousePoint_Creator>();   

        if(sMoving == null)                 //Buscamos asignar los scripts
            sMoving = GameObject.Find("Agent").GetComponent<Moving>();

        i = sMousePoint_Creator.lWayPoints.IndexOf(gameObject);         //Guardamos el indice que le corresponde al waypoint
           
        txtNumber.text = i.ToString();                                  //Imprimimos el indice en el canvas del objeto
    }

    private void OnMouseDown()
    {
        sMousePoint_Creator.lWayPoints.Remove(gameObject);              //Buscamos y quitamos el objeto de la lista

        if (sMoving.iWayPoint > 0)      //Si el indice del waypoint actual (el que indica al agente a donde ir)
            sMoving.iWayPoint--;        //Le restamos 1 para se diriga al anterior
        else                            //Sino quiere decir que el waypoint está en 0, es decir, que solo hay un waypoint
            sMoving.bNone = true;       //Variable que nos ayuda a mover el agente al centro (0,0,0)

        Destroy(gameObject);            //Destruimos el waypoint
    }
}
