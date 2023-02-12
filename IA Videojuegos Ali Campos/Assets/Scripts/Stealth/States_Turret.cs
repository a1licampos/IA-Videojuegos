using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class States_Turret : Steering_Behaviors
{
    [Header("BASICS")]
    public Turret sTurret;
    public Turret_Rotate sTurret_Rotate;


    [Header("ARRIVE")]
    public Rigidbody rb;

    public Vector3 v3SteeringForce = Vector3.zero;

    public bool bArrive;

    public float fSpeedMax;
    public float fSpeed;


    [Header("ALERT")]
    public float fVisionAngle_Alert;
    public float fVisionAngle_AlertBefore;

    public float fTimeBase_Alert;
    public float fTime_Alert;    
    public float fTotalSeenTime;
    public float fSeenTime;

    public bool bAlert;


    [Header("ATTACK")]
    public bool bAttack;

    public float fTimeBase_Attack;
    public float fTime_Attack;

    public Rigidbody rbAgent;

    public Touch_Agent sTouch_Agent;

    // Start is called before the first frame update
    void Start()
    {
        fTime_Alert = fTimeBase_Alert;
        bAlert = false;
        bAttack = false;
        fSeenTime = 0f;
        fVisionAngle_AlertBefore = sTurret.fVisionAngle;
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            rbAgent = GameObject.FindGameObjectWithTag("James Bond").GetComponent<Rigidbody>();
        }
        catch
        {
            Debug.Log("Se destruyó el agente James...");
        }

        if (sTurret.bDetected)
        {
            sTurret_Rotate.bAlert = true;

            bAlert = true;

            sTurret.fVisionAngle = fVisionAngle_Alert;

            fSeenTime += Time.deltaTime;
        }
        else if(!bAlert && !bAttack)
        {
            sTurret_Rotate.bAlert = false;
            fSeenTime = 0;
            fTime_Alert = fTimeBase_Alert;
            sTurret.fVisionAngle = fVisionAngle_AlertBefore;
        }

        if(bAlert)
        {
            if (fTime_Alert > 0)
            {
                fTime_Alert -= Time.deltaTime;
            }
            else
            {
                bArrive = true;
            }
        }



        if (fSeenTime > fTotalSeenTime)
        {
            bAttack = true;
        }

        if (bAttack)
        {
            if (fTime_Attack > 0)
            {
                fTime_Attack -= Time.deltaTime;
            }
            else
            {
                bAttack = false;
                bAlert = false;
                bArrive = false;
                fTime_Attack = fTimeBase_Attack;
            }
        }

    }

    private void FixedUpdate()
    {
        if (
            //Sacamos un decimal de la posicion para detenerlo
            (Mathf.Round(transform.position.x * 10) / 10 !=
            Mathf.Round(sTurret.v3AgentPosition.x * 10) / 10)

            &&

            //Sacamos un decimal de la posicion para detenerlo
            (Mathf.Round(transform.position.y * 10) / 10 !=
            Mathf.Round(sTurret.v3AgentPosition.y * 10) / 10)

            &&
            bArrive && !bAttack)
        {
            //Vamos a la ultima posicion del agente (James Bond)
            v3SteeringForce = Arrive(sTurret.v3AgentPosition, fSpeed);

            rb.AddForce(v3SteeringForce, ForceMode.Acceleration);

            rb.velocity = Vector3.ClampMagnitude(rb.velocity, fSpeedMax * Time.fixedDeltaTime);

        }
        else if (!bAttack)
        {
            bArrive = false;

            if ( (fTime_Alert < 0f || fTime_Alert == fTimeBase_Alert) && !bAttack)
            {
                bAlert = false;

                //ComeBack();
            }
        }

        if (bAttack)
        {
            try
            {
                bArrive = false;

                v3SteeringForce = Pursuit(rbAgent);

                rb.AddForce(v3SteeringForce, ForceMode.Acceleration);

                rb.velocity = Vector3.ClampMagnitude(rb.velocity, fSpeedMax * Time.fixedDeltaTime);
            }
            catch
            {
                Debug.Log("James Bond fue destruido...");
            }

        }
        
        
        if (!bAttack && !bAlert && !bArrive)
        {
            ComeBack();
        }

    }

    private void ComeBack()
    {
        //Vuelve al centro
        v3SteeringForce = Arrive(new Vector3(0, 0, 0), fSpeed);

        rb.AddForce(v3SteeringForce, ForceMode.Acceleration);

        rb.velocity = Vector3.ClampMagnitude(rb.velocity, fSpeedMax * Time.fixedDeltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(sTurret.v3AgentPosition, fArriveRadius);

        //Dibujamos una línea de la velocidad
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + myRigidbody.velocity);

        //Ahora dibujamos una línea de la fuerza
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position + v3SteeringForce);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("James Bond"))
        {
            sTouch_Agent.bTouched = true;
            bAttack = false;
            bAlert = false;
            fTime_Attack = fTimeBase_Attack;
        }
    }
}
