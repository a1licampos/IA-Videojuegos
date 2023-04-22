using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
{
    public PatrolAgentFSM _sm;
    private float fCurrentChaseTime;
    private bool bGoingBackToPatrolPos = false;
    public AttackState(StateMachine stateMachine) : base("Attack", stateMachine)
    {
        //El ": base("Patrol", stateMachine)" equivale a escribir las siguientes líneas.
        //this.name = "Patrol"
        //this.stateMachine = stateMachine;
    }

    public AttackState(PatrolAgentFSM stateMachine) : base("Attack", stateMachine)
    {
        //El ": base("Patrol", stateMachine)" equivale a escribir las siguientes líneas.
        //this.name = "Patrol"
        //this.stateMachine = stateMachine;
        _sm = stateMachine;
    }

    public override void Enter()
    {
        //Podemos mandar a llamar cualquier función
        //base.Enter();

        //Por ejemplo, aquí podrían poner el trigger de su animtor a "SetTrigger("OnPatrol")"
        fCurrentChaseTime = 0.0f;
        bGoingBackToPatrolPos = false;
        _sm.navMeshAgent.destination = _sm.v3TargetTransform.position;
    }

    private void CheckPersecutionTime()
    {
        if(fCurrentChaseTime >= _sm.FMaxChasingTime)
        {
            //Ahora tenemos que hacer que vuelva a su posicion inicial
            _sm.navMeshAgent.destination = _sm.v3AgentPatrollingPosition;
            bGoingBackToPatrolPos = true;
        }
    }

    private void GoBackToPatrolPosition()
    {
        //IDEA: Incluso podria ser regresar primero a Alert pero con su AlertStep en goingBack
        //Se ejecuta hasta que llegue a la posicion, y una vez lo hace, vuelve al estado de patrullaje
        float fDist = (_sm.transform.position - _sm.v3AgentPatrollingPosition).magnitude;

        if(fDist <= 1.0f)
        {
            _sm.navMeshAgent.destination = _sm.transform.position;
            _sm.ChangeState(_sm.patrolState);
            return;
        }
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
        //base.UpdatePhysics();

        if(!bGoingBackToPatrolPos)
        {
            fCurrentChaseTime += Time.fixedDeltaTime;

            //Lo ideal no es hacerlo cada cuado, 10 veces por seg por ejemplo
            _sm.navMeshAgent.destination = _sm.v3TargetTransform.position;

            CheckPersecutionTime();
        }
        else
        {
            GoBackToPatrolPosition();
        }
    }
}

