using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : BaseState
{
    public PatrolAgentFSM _sm;
    private WaitForSeconds RotateIntervalWait;
    private Coroutine RotateCoroutineRef;

   public PatrolState(StateMachine stateMachine) : base("Patrol", stateMachine)
    {
        //El constructor solo es de BaseStateMachine
        //El ": base("Patrol", stateMachine)" equivale a escribir las siguientes líneas.
            //this.name = "Patrol"
            //this.stateMachine = stateMachine;
    }

    //Sin embargo, al tener el constructor con la StateMachine, no estamos forzando al 
    //desarrollador a usar la PatrolAgentFSM que diseñamos para trabajar en conjunto.
    //Pero podemos hacer una versión del constructor que sí lo enforce.
    public PatrolState(PatrolAgentFSM stateMachine) : base("Patrol", stateMachine)
    {
        _sm = stateMachine;
    }

    //Función para rotar al personaje cada n tiempo
    private IEnumerator Rotate()
    {
        RotateIntervalWait = new WaitForSeconds(_sm.FTimeToRotate);

        while (true)
        {
            //Esperamos n segundos antes de ejecutar el codigo siguinete
            yield return RotateIntervalWait;

            //Rotamos a nuestro agente patrullero especto al eje de "arriba"
            _sm.transform.Rotate(_sm.transform.up, _sm.FRotateAngle);

        }

    }

    public override void Enter()
    {
        //Por ejemplo, aquí podrían poner el trigger de su animtor a "SetTrigger("OnPatrol")"
        _sm.ResetAnimations();
        _sm.mAnimator.SetBool("standing", true);    // Comienza la animacion de buscar objetivo
        RotateCoroutineRef = _sm.StartCoroutine(this.Rotate());
    }

    public override void Exit()
    {
        //base.Exit();
        _sm.StopCoroutine(Rotate());
    }

    public override void UpdateLogic()
    {
        //base.UpdateLogic();

        //ESTA PARTE NO ESTÁ FUNCIONANDO AL 100.        13 ABRIL 2023
        //Vector3 FakeInfiltratorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //FakeInfiltratorPosition.z = 0.0f; //Si no hacemos esto, tendrá la z de la cámara.
        //_sm.v3TargetPosition = FakeInfiltratorPosition;

    }

    public override void UpdatePhysics()
    {
        //base.UpdatePhysics();

        bool bCheckVision = _sm.CheckVFieldOfVision(_sm.fVisionDist, _sm.fVisionAngle, out Vector3 tmp_TargetPosition);

        if (bCheckVision)
        {
            _sm.v3LastKnowTargetPos = tmp_TargetPosition;
            _sm.mAnimator.SetBool("standing", false);   // Termina la animacion de buscar objetivo
            _sm.ChangeState(_sm.alertState);

            return; //Para asegurarnos que salga de esta función, pues ya se disparó un cambio de estado.
        }
    }
}
