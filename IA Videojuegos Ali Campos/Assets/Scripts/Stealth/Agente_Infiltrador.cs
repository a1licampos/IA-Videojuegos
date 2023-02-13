using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agente_Infiltrador : Steering_Behaviors
{
    [Header("MOUSE")]
    public  Vector3 v3MousePosition = Vector3.zero;
    private Vector3 v3SteeringForce = Vector3.zero;

    public Rigidbody rb;

    public float fSpeedMax;
    public float fSpeed;

    public bool bStop;

    void Update()
    {
        //Moviendo a James Bond "Agent Infiltrate" hacia el click del mouse
        if (Input.GetMouseButtonDown(0))
        {
            v3MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            bStop = false;
        }

    }


    //OBTENER DOS DECIMALES
    //https://answers.unity.com/questions/50391/how-to-round-a-float-to-2-dp.html
    private void FixedUpdate()
    {
        v3MousePosition.z = 0.0f;       //Actualizamos la Z del click porque estamos en 2D

        if (
            //Sacamos dos decimales de la posicion para detenerlo
            (Mathf.Round(transform.position.x * 100) / 100 !=
            Mathf.Round(v3MousePosition.x * 100) / 100)

            &&

            //Sacamos dos decimales de la posicion para detenerlo
            (Mathf.Round(transform.position.y * 100) / 100 !=
            Mathf.Round(v3MousePosition.y * 100) / 100)

            && 
            bStop)                      //Cuando hacemos un click, se realiza un Arrive hasta la posicion del mismo
        {
            v3SteeringForce = Arrive(v3MousePosition, fSpeed);

            rb.AddForce(v3SteeringForce, ForceMode.Acceleration);

            rb.velocity = Vector3.ClampMagnitude(rb.velocity, fSpeedMax * Time.fixedDeltaTime);

        }
        else                            //Una vez que llegamos aproximadamente a la misma posicion
            bStop = true;               //Detenemos el Arrive

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
