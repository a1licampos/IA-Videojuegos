using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertState : BaseState
{
    public PatrolAgentFSM _sm;

    private float fTotalTimeTargetHasBeenOnFOV = 0.0f;
    private float fTotalTimeBeforeGoing = 0.0f;

    //Maquina de estados jeraquica, una mini maquina de estados.
    public enum AlertStep
    {
        preparingToGo = 0, going = 1, goingBack = 2, finished = 3
    }

    private AlertStep bCheckingLastKnowPos = AlertStep.finished;

    public AlertState(StateMachine stateMachine) : base("Alert", stateMachine)
    {
        //El ": base("Patrol", stateMachine)" equivale a escribir las siguientes líneas.
        //this.name = "Patrol"
        //this.stateMachine = stateMachine;
    }

    public AlertState(PatrolAgentFSM stateMachine) : base("Alert", stateMachine)
    {
        //El ": base("Patrol", stateMachine)" equivale a escribir las siguientes líneas.
        //this.name = "Patrol"
        //this.stateMachine = stateMachine;
        _sm = stateMachine;
    }

    public override void Enter()
    {     
        fTotalTimeTargetHasBeenOnFOV = 0.0f;
        fTotalTimeBeforeGoing = 0.0f;
        bCheckingLastKnowPos = AlertStep.preparingToGo;

        _sm.ResetAnimations();
    }

    public override void Exit()
    {
        //base.Exit();
    }

    public override void UpdateLogic()
    {
        //base.UpdateLogic();
    }

    public override void UpdatePhysics()
    {
        if(bCheckingLastKnowPos == AlertStep.preparingToGo)
        {
            fTotalTimeBeforeGoing += Time.fixedDeltaTime;
            if(fTotalTimeBeforeGoing >= _sm.FTimeBeforeCheckingTargetsLastKnownPosition)
            {
                //Entonces pasa al AlertStep.going
                bCheckingLastKnowPos = AlertStep.going;
                _sm.navMeshAgent.SetDestination(_sm.v3LastKnowTargetPos);
                _sm.mAnimator.SetBool("forward", true);
            }
        }

        //Mientras no haya llegado a la última posición conocida del infiltrador, 
        if(bCheckingLastKnowPos == AlertStep.going)
        {
            float vDist = (_sm.transform.position - _sm.v3LastKnowTargetPos).magnitude;

            if(vDist <= 1.0f)
            {
                //Entonces ya llegó. Tiene que dar unos vistazos, y luego regresar a su posición de patrullaje.
                bCheckingLastKnowPos = AlertStep.goingBack;
                _sm.ResetAnimations();

                //Regresa a su posicion de patrullaje
                _sm.navMeshAgent.SetDestination(_sm.v3AgentPatrollingPosition);
                _sm.mAnimator.SetBool("forward", true);
            }
        }

       if(bCheckingLastKnowPos == AlertStep.goingBack)
        {
            float vDist = (_sm.transform.position - _sm.v3AgentPatrollingPosition).magnitude;

            if(vDist <= 1.0f)
            {
                //Entonces ya llegó a su posicion de patrullaje y debe de regresar al estado de patrullaje
                bCheckingLastKnowPos = AlertStep.finished;

                _sm.ChangeState(_sm.patrolState);
                return;
            }
        }

        bool CheckFOV = _sm.CheckVFieldOfVision(_sm.FAlertVisionDist, _sm.FAlertVisionAngle, out Vector3 tmp_TargetPosition);

        if(CheckFOV)
        {
            Debug.Log("EY!" + fTotalTimeTargetHasBeenOnFOV);
            fTotalTimeTargetHasBeenOnFOV += Time.fixedDeltaTime;
        }
        

        if(fTotalTimeTargetHasBeenOnFOV > _sm.FTimeToGoFromAlertToAttack)
        {
            Debug.Log("Changing to AttackState");
            _sm.ChangeState(_sm.attackState);

            return;
        }
    }
}
