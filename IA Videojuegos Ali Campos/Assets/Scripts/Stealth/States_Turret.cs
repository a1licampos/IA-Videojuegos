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

    void Start()
    {
        fTime_Alert = fTimeBase_Alert;                      //Reiniciamos el tiempo al cual le restaremos el time.deltatime
        bAlert = false;                                     //Estamos de alerta en falso
        bAttack = false;                                    //Estamos de ataque en falso
        fSeenTime = 0f;                                     //Reiniciamos el tiempo que se ha visto al agente
        fVisionAngle_AlertBefore = sTurret.fVisionAngle;    //Guardamos el angulo de vision base
    }


    void Update()
    {
        try
        {
            rbAgent = GameObject.FindGameObjectWithTag("James Bond").GetComponent<Rigidbody>();     //Obtenemos el Rigibody del agente
        }
        catch
        {
            Debug.Log("Se destruyó el agente James...");
        }

        if (sTurret.bDetected)                          //El agente entro al campo de vision
        {
            sTurret_Rotate.bAlert = true;               //Detenemos el estado normal del guardia

            bAlert = true;                              //Activamos el estado de alerta

            sTurret.fVisionAngle = fVisionAngle_Alert;  //Aumentamos el angulo de vision

            fSeenTime += Time.deltaTime;                //Sumamos a la variable el tiempo que se ha visto al agente
        }
        else if(!bAlert && !bAttack)                    //Si no esta en estado de alerta ni de ataque
        {
            sTurret_Rotate.bAlert = false;              //Vuele al estado normal
            fSeenTime = 0;                              //El tiempo visto al agente
            fTime_Alert = fTimeBase_Alert;              //Reiniciamos el tiempo al cual le restaremos el time.deltatime
            sTurret.fVisionAngle = fVisionAngle_AlertBefore;    //Reestrablecemos el campo de vision
        }

        if(bAlert)                                      //Si entra en estado de alerta
        {                                               //Esperamos el tiempo necesario para que vaya al ultimo lugar donde vio al agente
            if (fTime_Alert > 0)
            {
                fTime_Alert -= Time.deltaTime;
            }
            else
            {
                bArrive = true;
            }
        }



        if (fSeenTime > fTotalSeenTime)                 //Si el tiempo total visto al agente alcanza al necesario
        {
            bAttack = true;                             //Pasamos al modo de ataque
        }

        if (bAttack)                                    //Una vez en el modo de ataque
        {                                               //Comienza la cuenta regresiva de 5 seg en la cual 
            if (fTime_Attack > 0)                       //Perseguira al agente
            {
                fTime_Attack -= Time.deltaTime;
            }
            else                                        //En caso contrario, reiniciara todo para el estado normal del vigilante
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
            bArrive && !bAttack)                                    //En estado de alerta se activa un Arrive hasta la ultima posicion del agente
        {
            //Vamos a la ultima posicion del agente (James Bond)
            v3SteeringForce = Arrive(sTurret.v3AgentPosition, fSpeed);

            rb.AddForce(v3SteeringForce, ForceMode.Acceleration);

            rb.velocity = Vector3.ClampMagnitude(rb.velocity, fSpeedMax * Time.fixedDeltaTime);

        }
        else if (!bAttack)                                          //Si el estado de ataque es falso y ya llego al punto
        {
            bArrive = false;

            if ( (fTime_Alert < 0f || fTime_Alert == fTimeBase_Alert) && !bAttack)
            {
                bAlert = false;                                     //Desactiva todo y vuelve al centro y al estado normal
            }
        }

        if (bAttack)                                                //Entramos al estado de ataque
        {
            try
            {
                bArrive = false;                                    //Desactivamos el Arrive

                v3SteeringForce = Pursuit(rbAgent);                 //Hacemos un persuit al agente

                rb.AddForce(v3SteeringForce, ForceMode.Acceleration);

                rb.velocity = Vector3.ClampMagnitude(rb.velocity, fSpeedMax * Time.fixedDeltaTime);
            }
            catch
            {
                Debug.Log("James Bond fue destruido...");
            }

        }
        
        
        if (!bAttack && !bAlert && !bArrive)                        //Si ningun estado esta activo, el vigilante vuelve al centro (0,0,0)
        {
            ComeBack();
        }

    }

    private void ComeBack()
    {
        //Vuelve al centro con un Arrive
        v3SteeringForce = Arrive(new Vector3(0, 0, 0), fSpeed);

        rb.AddForce(v3SteeringForce, ForceMode.Acceleration);

        rb.velocity = Vector3.ClampMagnitude(rb.velocity, fSpeedMax * Time.fixedDeltaTime);
    }

    private void OnDrawGizmos()
    {
        //Dibujamos el punto donde hara el Arrive
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
        if(other.gameObject.CompareTag("James Bond"))       //Si hace trigger con el agente lo destruira
        {
            sTouch_Agent.bTouched = true;
            bAttack = false;                                //Volvemos al estado normal
            bAlert = false;
            fTime_Attack = fTimeBase_Attack;
        }
    }
}
