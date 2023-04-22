using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    //Referencia al estado actual de la m�quina.
    BaseState currentState;

    //Funci�n get para que otros scrips puedan saber en �qu� estado se encuentra la m�quina de estados
    public BaseState CurrentState
    {
        get { return currentState; }
    }
    
     
    // Start is called before the first frame update
    public void Start()
    {
        currentState = GetInitialState();
        if (currentState != null)
            currentState.Enter();
    }

    /* Como las m�quinas de estados que vamos a usar deben heradar de esta clase y hacer un
     * override de esta funci�n para iniciar en el estado que deseen.
     * 
     * OJO: Esta funci�n es protected para que solo esta clase y sus hijas puedas accederla.
     */
    protected virtual BaseState GetInitialState()
    {
        //por defecto regresa null
        return null;
    }

    public void Update()
    {
        //Si hay un estado actual (es decir, no es nulo)
        if (currentState != null)
            //Entonces, que actualice la l�gica de juego, inputs y otras cosas necesarias del juego
            currentState.UpdateLogic();
    }

    public void FixedUpdate()
    {
        if (currentState != null)
            currentState.UpdatePhysics();
    }

    public void ChangeState(BaseState newState)
    {
        //Primero, que el estado actual haga la limpieza que requiera.
        currentState.Exit();
        //Despu�s, asignamos el nuevo estado como el estado actual de la m�quina.
        currentState = newState;
        //Finalmente, que el nuevo estado haga las inicializaciones que requiera en su enter.
        currentState.Enter();
    }

    private void OnGUI()
    {
        string text = currentState != null ? currentState.name : "No current State asigned";
        GUILayout.Label($"<size=40>{text}</size>");
    }
}
