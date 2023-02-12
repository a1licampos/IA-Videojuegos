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

    public bool one = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        if(sMousePoint_Creator.lWayPoints.Count == 2 && one)
        {
            one = false;
            iWayPoint++;
        }

        if(sMousePoint_Creator.lWayPoints.Count == 1)
        {
            one = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (sMousePoint_Creator.lWayPoints.Count > 0)
        {
            v3SteeringForce = Arrive(sMousePoint_Creator.lWayPoints[iWayPoint].transform.position, fSpeed);

            rb.AddForce(v3SteeringForce, ForceMode.Acceleration);

            rb.velocity = Vector3.ClampMagnitude(rb.velocity, fSpeedMax * Time.fixedDeltaTime);
        }
        //else
        //{
        //    v3SteeringForce = Arrive(new Vector3(0,0,0), fSpeed);

        //    rb.AddForce(v3SteeringForce, ForceMode.Acceleration);

        //    rb.velocity = Vector3.ClampMagnitude(rb.velocity, fSpeedMax * Time.fixedDeltaTime);
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        //if(other.gameObject == sMousePoint_Creator.lWayPoints[iWayPoint + 1])
        //{
        //    if (sMousePoint_Creator.lWayPoints.Count > 0)
        //    {
        if (iWayPoint < sMousePoint_Creator.lWayPoints.Count - 1)
        {
            if (other.gameObject == sMousePoint_Creator.lWayPoints[iWayPoint + 1].gameObject)
                iWayPoint++;
        }
        else
        {
            if (other.gameObject == sMousePoint_Creator.lWayPoints[sMousePoint_Creator.lWayPoints.Count - 1].gameObject)
                iWayPoint *= 0;
        }

        //    }
            
        //}
    }
}
