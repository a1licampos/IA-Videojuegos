using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving : Steering_Behaviors
{
    [Header("MOVING")]
    public MousePoint_Creator sMousePoint_Creator;

    public Vector3 v3SteeringForce;

    public Rigidbody rb;

    public int iWayPoint;

    public float fSpeedMax;
    public float fSpeed;

    public bool bOne = true;        //Nos ayuda a pasar del waypoint 0 al 1
    public bool bNone = false;      //Nos ayuda a quedarnos en el centro (0,0,0) cuando no hay waypoints

    private void Update()
    {
        if(sMousePoint_Creator.lWayPoints.Count == 2 && bOne)       //Esta comprobacion nos ayuda a mover al agente al waypoint 1 cuando solo teníamos el waypoint 0
        {
            bOne = false;                                           //Solo se hace una vez esta comprobacion
            iWayPoint++;
        }

        if(sMousePoint_Creator.lWayPoints.Count == 1)               //Si solo tenemos el waypoint 0 no queremos que siga en el centro (0,0,0)
        {
            bOne = true;
            bNone = false;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (sMousePoint_Creator.lWayPoints.Count > 0)               //Si tenemos minimo un waypoint hacemos un Arrive hasta EL
        {
            v3SteeringForce = Arrive(sMousePoint_Creator.lWayPoints[iWayPoint].transform.position, fSpeed);

            rb.AddForce(v3SteeringForce, ForceMode.Acceleration);

            rb.velocity = Vector3.ClampMagnitude(rb.velocity, fSpeedMax * Time.fixedDeltaTime);
        }

        if (bNone)                                                  //Si NO tenemos un waypoint hacemos Arrive hasta el centro
        {
            v3SteeringForce = Arrive(new Vector3(0, 0, 0), fSpeed);

            rb.AddForce(v3SteeringForce, ForceMode.Acceleration);

            rb.velocity = Vector3.ClampMagnitude(rb.velocity, fSpeedMax * Time.fixedDeltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)                     //Cuando hacemos collision con el waypoint sumamos para el siguiente o regresamos al principio
    {
        if (iWayPoint < sMousePoint_Creator.lWayPoints.Count - 1)   //Comprobamos que no vamos a exceder el indice de la lista
        {
            if (other.gameObject == sMousePoint_Creator.lWayPoints[iWayPoint].gameObject)   //Si el objetos con el que colisionamos es el waypoint con el indice objetivo
                iWayPoint++;                                                                //Vamos al siguiente waypoint
        }
        else
        {
            if (other.gameObject == sMousePoint_Creator.lWayPoints[sMousePoint_Creator.lWayPoints.Count - 1].gameObject)    //Si llegamos al utlimo waypoint
                iWayPoint *= 0;                                                                                             //Volvemos al principio
        }
    }
}
