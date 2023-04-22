using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolAgentFSM : StateMachine
{
    [HideInInspector]
    public PatrolState patrolState;
    [HideInInspector]
    public AlertState alertState;
    [HideInInspector]
    public AttackState attackState;

    private LayerMask WallLayerMask;


    //Todas las variables que son necesarios para modificarse en el editor
    //public

    public UnityEngine.AI.NavMeshAgent navMeshAgent;

    //Posicion donde el agente va a estar parado mientras esta en el esztado de Patrol,
    //y a la cual debe regresar en el estado de Alert para volver al estado de Patrol.
    public Vector3 v3AgentPatrollingPosition;

    //Cono de vision
    //Estas variables pueden cambiar mucho seg�n el modo de juego (2d, 3d, etc.)
    //Variables que necesitamos para definir el cono de visi�n.
    [Range(0.01f, 1000.0f)]
    public float fVisionDist = 10.0f;
    [Range(0.0f, 360.0f)]
    public float fVisionAngle = 90.0f;

    //Hacia donde esta viendo el agente Patrullero
    public Transform v3TargetTransform;
    public Vector3 v3LastKnowTargetPos;


    [Range(0, 359.0f)]
    public float fRotateAngle = 45f;
    public float FRotateAngle
    {
        get { return fRotateAngle; }
    }


    [Range(0.1f, 120.0f)]
    public float fTimeToRotate = 2.0f;
    public float FTimeToRotate
    {
        get { return fTimeToRotate; }
    }


    [Range(0.1f, 1000f)]
    public float fAlertVisionDist = 10.0f;
    public float FAlertVisionDist
    {
        get { return fAlertVisionDist; }
    }


    [Range(0.0f, 359.0f)]
    public float fAlertVisionAngle = 20.0f;
    public float FAlertVisionAngle
    {
        get { return fAlertVisionAngle; }
    }

    [Range(0.0f, 120.0f)]
    public float fTimeToGoFromAlertToAttack = 2.0f;
    public float FTimeToGoFromAlertToAttack
    {
        get { return fTimeToGoFromAlertToAttack; }
    }

    [Range(0.0f, 120.0f)]
    public float fTimeBeforeCheckingTargetsLastKnownPosition = 2.0f;
    public float FTimeBeforeCheckingTargetsLastKnownPosition
    {
        get { return fTimeBeforeCheckingTargetsLastKnownPosition; }
    }

    //Que tanto tiempo va a perseguir el agente patrullero al agente inflitrador durante el estado de Ataque
    [Range(1.0f, 25.0f)]
    public float fMaxChasingTime = 5.0f;
    public float FMaxChasingTime
    {
        get { return fMaxChasingTime; }
    }

    //Animator
    public Animator mAnimator;

    //Ligths
    public Light mLinter;

    public bool CheckVFieldOfVision(float in_fVisionDist, float in_fVisionAngle, out Vector3 v3TargetPos)
    {
        v3TargetPos = Vector3.zero;
        //La comprobaci�n de 2 chequeos, uno similar al chequep del area de un circulo.
        //y otro que es respecto al �nfulo de ese circulo

        //OJO: Cual de estas dos comprobaciones deberia realizarse primero en terminos de
        //desempeño (performance)
        Vector3 v3AgentToTarget = (v3TargetTransform.position - transform.position);

        //Profiling o benchmarking
        float fAgentToTargetDist = v3AgentToTarget.magnitude;
        if (fAgentToTargetDist > in_fVisionDist)
        {
            //Nos salimos porque no esta en el rango de vision
            return false;
        }

        if(Vector3.Angle(v3AgentToTarget, transform.forward) > in_fVisionAngle * 0.5)
        {
            //Nos salimos porque no esta dentro del �ngulo que define al cono.
            return false;
        }

        if (Physics.Raycast(transform.position, v3AgentToTarget.normalized, v3AgentToTarget.magnitude, WallLayerMask))
            return false;

        v3TargetPos = v3TargetTransform.position;
        return true;
    }

    private Vector3 PointForAngle(float fAngle, float fDistance)
    {
        float fAngleRads = Mathf.Rad2Deg * fAngle;
        return transform.TransformDirection(new Vector2(Mathf.Cos(fAngleRads), Mathf.Sin(fAngleRads)) * fVisionDist);
    }



    private void Awake()
    {
        patrolState = new PatrolState(this);
        alertState = new AlertState(this);
        attackState = new AttackState(this);

        WallLayerMask = LayerMask.GetMask("Wall");
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        mAnimator = GetComponent<Animator>();
        mLinter = GetComponentInChildren<Light>();


        //Esto es para que la posición en la que el patrullero inicie en la escena
        //sea su posición a la que deba regresar para volver al estado Patrol
        //Si ustedes no desearan este comportamiento, favor de cambiarla esta linea;
        v3AgentPatrollingPosition = transform.position;

        mLinter.range = fVisionDist + 2;
        mLinter.spotAngle = fVisionAngle + 10;
    }

    protected override BaseState GetInitialState()
    {
        //Segun definimos para este agente, el primer estado debe ser el de patrullar
        return patrolState;
    }

    public void ResetAnimations()
    {
        mAnimator.SetBool("forward", false);
        mAnimator.SetBool("backwards", false);
        mAnimator.SetBool("attack", false);
        mAnimator.SetBool("standing", false);
    }
}
