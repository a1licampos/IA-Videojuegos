using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarAgent : Steering_Behaviors
{
    //Lista donde guardaremos los puntos que nos regrese el método de A*
    public List<Vector3> Route = null;

    //Qué tan cerca tiene que estar el agente del punto objetivo para cambiar al que sigue
    public float fDistanceThreshold;

    //Una instancia a la cual se hace referencia aquí
    Pathfinding_Test _PathfindingReference;

    //Referencia del grid
    ClassGrid _GridReference;

    int iCurrentRoutePoint = 0;

    //Necesitamos cambiar el fondo de la cámara;
    public Camera cam;

    [Header("AGENT")]
    public bool bAgentSelected;
    public MouseGrid mouseGrid;
    public bool bStartAStar;

    [Header("REBOOT")]
    public SelectPlayer selectPlayer;
    public bool bNewPathFinding;

    // Start is called before the first frame update
    void Start()
    {
        //BUSCAMOS las referencias de LOS SCRIPTS A USAR
        _PathfindingReference = GameObject.FindGameObjectWithTag("grid").GetComponent<Pathfinding_Test>();

        mouseGrid = GameObject.Find("Mouse").GetComponent<MouseGrid>();

        cam = GameObject.Find("Main Camera").GetComponent<Camera>();

        //REBOOT
        selectPlayer = GetComponent<SelectPlayer>();
    }

    public List<Node> AStarResult;
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(bAgentSelected && mouseGrid.bStart && mouseGrid.bEnd && !bStartAStar)
            {
                cam.backgroundColor = Color.black;
                bStartAStar = true;
                bNewPathFinding = false;

                _GridReference = _PathfindingReference.myGrid;

                //Guardamos el resultado de nuestro A*
                List<Node> AStarResult = _GridReference.AStarSearch(_PathfindingReference.StartPosition.x,
                                                                    _PathfindingReference.StartPosition.y,
                                                                    _PathfindingReference.EndPosition.x,
                                                                    _PathfindingReference.EndPosition.y);

                if(AStarResult == null)     //El camino es inválido
                {
                    StartCoroutine("NewPathFindingFast");
                }
                else
                {
                    //Usamos esa lista de nodos para sacar las posiciones de mundo a las cuales hacer Seek o Arrive
                    Route = _GridReference.ConvertBacktrackToWorldPos(AStarResult);
                }
            }
            else    //Avisamos que algo falta de hacer o seleccionar
            {
                Debug.LogWarning("Algo falta... ¿Seleccionar Agente, Inicio o Final?... ¡Chécalo!");
            }
        }
    }
    
    private Vector3 v3SteeringForce = Vector3.zero;

    // Update is called once per frame
    void FixedUpdate()
    {

        if(Route.Count != 0)
        {
            //Queremos saber si estamos cerca de nuestra posición objetivo
            //Para ello, calculamos la distancia entra la posición Route[iCurrentRoutePoint] y la actual
            float fDistanceToPoint = (Route[iCurrentRoutePoint] - transform.position).magnitude;
            //Debug.Log("fDistanceToPoint to Point is: " + fDistanceToPoint);

            //Si esta distancia es menor o igual a un umbral, cambiamos al siguiente punto de la lista
            if (fDistanceToPoint < fDistanceThreshold)
            {
                iCurrentRoutePoint++;
                iCurrentRoutePoint = Mathf.Min(iCurrentRoutePoint, Route.Count - 1);
            }

            if(iCurrentRoutePoint == Route.Count - 1)
            {
                bUseArrive = true;                                      //Llego a su objetivo y se quedá en él
                v3SteeringForce = Seek(Route[iCurrentRoutePoint]);      //Hacemos seek al último punto

                if(!bNewPathFinding)                                    //Es momento de un nuevo pathfinding
                {
                    StartCoroutine("NewPathFinding");
                    bNewPathFinding = true;
                }
            }
            else
            {
                //Ahora sí, hay que mover el agente hacia el punto destino.
                v3SteeringForce =  Seek(Route[iCurrentRoutePoint]);
            }
        }

        // Idealmente, usaríamos el ForceMode de Force, para tomar en cuenta la masa del objeto.
        // Aquí ya no usamos el deltaTime porque viene integrado en como funciona AddForce.
        myRigidbody.AddForce(v3SteeringForce, ForceMode.Acceleration);

        //Hacemos un Clamp para que no exceda la velocidad máxima que puede tener el agente
        myRigidbody.velocity = Vector3.ClampMagnitude(myRigidbody.velocity, fMaxSpeed);

        
    }


    public void AgentSelected()
    {
        bAgentSelected = true;
    }

    public void AgentDeselected()
    {
        bAgentSelected = false;
    }

    public void Reboot()
    {
        bStartAStar = false;                    //Ya no se está empezando el A*
        v3SteeringForce = Vector3.zero;         //Reestablecemos la fuerza del agente
        Route.Clear();                          //Borramos la lista que utilizamos para mover el agente
        myRigidbody.velocity = Vector3.zero;    //Quitamos la velocidad del agente del RB
        iCurrentRoutePoint = 0;                 //Reestablecemos el indice
        AgentDeselected();                      //El agente ya no está seleccionado
        cam.backgroundColor = new Color(0.1921569f, 0.3019608f, 0.4745098f);    //Reestablecemos el color de la cámara
    }

    //Se encontró el camino y es momento de reiniciar para un nuevo pathfinding
    //No sé hace instantáneamente para dar un tiempo de asimilar lo ocurrido
    IEnumerator NewPathFinding()
    {
        yield return new WaitForSeconds(4f);        //Tiempo esperado para el Reinicio(Reboot)
        Debug.LogWarning("Nuevo Pathfinding...");   //Avisamos de lo sucedido
        Reboot();                                   //Reiniciamos todo lo usado
        selectPlayer.Reboot();
        mouseGrid.RebootStartEnd();
        _GridReference.ResetGrid();
        _GridReference.RebootGridColor();
    }

    //No se encontró un camino... y es momento de reiniciar para un nuevo pathfinding
    //No sé hace instantáneamente para dar un tiempo de asimilar lo ocurrido
    IEnumerator NewPathFindingFast()
    {
        yield return new WaitForSeconds(2f);
        Debug.LogWarning("No se encontró un camino... Nuevo Pathfinding...");
        Reboot();
        selectPlayer.Reboot();
        mouseGrid.RebootStartEnd();
        _GridReference.ResetGrid();
        _GridReference.RebootGridColor();
    }
}
