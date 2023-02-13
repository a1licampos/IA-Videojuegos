using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent_PositionMouse : Steering_Behaviors
{
    [Header("MOUSE")]
    public Vector3 v3MousePosition = Vector3.zero;

    [Header("MOVEMENT")]
    private Vector3 v3SteeringForce = Vector3.zero;

    public Vector3 tObstaclePosition;

    public bool bArrive;

    public float fSpeedMax;
    public float fSpeed;

    public Rigidbody rb;


    void Update()
    {
        //Posicion Objetivo a traves del click del mouse
        if (Input.GetMouseButtonDown(0))
        {
            v3MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    private void FixedUpdate()
    {
        v3MousePosition.z = 0.0f;   //Actulizamos el z del click porque estamos en 2D

        if (bArrive)                //Hacemos un Arrive a la posicion del click
        {
            v3SteeringForce = Arrive(v3MousePosition, fSpeed);

            rb.AddForce(v3SteeringForce, ForceMode.Acceleration);

            rb.velocity = Vector3.ClampMagnitude(rb.velocity, fSpeedMax * Time.fixedDeltaTime);
        }
        else                        //Hacemos un Flee al obstaculo
        {
            v3SteeringForce = Flee(tObstaclePosition, fSpeed);

            rb.AddForce(v3SteeringForce, ForceMode.Acceleration);

            rb.velocity = Vector3.ClampMagnitude(rb.velocity, fSpeedMax * Time.fixedDeltaTime);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Obstacle"))     //Si hacemos trigger con un obstaculo
        {
            tObstaclePosition = other.gameObject.GetComponent<Transform>().position;    //Obtemos su posicion
            bArrive = false;                            //Desactivamos el Arrive al objetivo
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle"))    //Cuando salimos de su rango
        {
            bArrive = true;                             //Volvemos a hacer el arrive al click
        }
    }

    private void OnDrawGizmos()
    {
        //Dibujamos la posicion objetivo
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(v3MousePosition, 1f);

        //Dibujamos una línea de la velocidad
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + myRigidbody.velocity);

        //Ahora dibujamos una línea de la fuerza
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position + v3SteeringForce);
    }
}
