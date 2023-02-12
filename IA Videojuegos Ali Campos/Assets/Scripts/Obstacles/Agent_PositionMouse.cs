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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Posicion Objetivo
        if (Input.GetMouseButtonDown(0))
        {
            v3MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    private void FixedUpdate()
    {
        v3MousePosition.z = 0.0f;

        if (bArrive)
        {
            v3SteeringForce = Arrive(v3MousePosition, fSpeed);

            rb.AddForce(v3SteeringForce, ForceMode.Acceleration);

            rb.velocity = Vector3.ClampMagnitude(rb.velocity, fSpeedMax * Time.fixedDeltaTime);
        }
        else
        {
            v3SteeringForce = Flee(tObstaclePosition, fSpeed);

            rb.AddForce(v3SteeringForce, ForceMode.Acceleration);

            rb.velocity = Vector3.ClampMagnitude(rb.velocity, fSpeedMax * Time.fixedDeltaTime);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Obstacle"))
        {
            tObstaclePosition = other.gameObject.GetComponent<Transform>().position;
            bArrive = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            bArrive = true;
        }
    }

    private void OnDrawGizmos()
    {
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
