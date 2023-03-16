using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarAgent : Steering_Behaviors
{
    //Lista donde guardaremos los puntos que nos regrese el método de A*
    public List<Vector3> Route;

    //Qué tan cerca tiene que estar el agente del punto objetivo para cambiar al que sigue
    public float fDistanceThreshold;

    //Una instancia que se referencia aquí
    Pathfinding_Test _PathfindingReference;

    ClassGrid _GridReference;

    int iCurrentRoutePoint = 0;

    // Start is called before the first frame update
    void Start()
    {
        _PathfindingReference = GameObject.FindGameObjectWithTag("grid").GetComponent<Pathfinding_Test>();

        _GridReference = _PathfindingReference.myGrid;

        //Guardamos el resultado de nuestro A*
        List < Node > AStarResult = _GridReference.AStarSearch(_PathfindingReference.StartPosition.x,
                                                            _PathfindingReference.StartPosition.y,
                                                            _PathfindingReference.EndPosition.x,
                                                            _PathfindingReference.EndPosition.y);

        //Usamos esa lista de nodos para sacar las posiciones de mundo a las cuales hacer Seek o Arrive
        Route = _GridReference.ConvertBacktrackToWorldPos(AStarResult);
    }

    public List<Node> AStarResult;
    private void Update()
    {
        ////Guardamos el resultado de nuestro A*
        // AStarResult = _GridReference.AStarSearch(_PathfindingReference.StartPosition.x,
        //                                                    _PathfindingReference.StartPosition.y,
        //                                                    _PathfindingReference.EndPosition.x,
        //                                                    _PathfindingReference.EndPosition.y);

        ////Usamos esa lista de nodos para sacar las posiciones de mundo a las cuales hacer Seek o Arrive
        //Route = _GridReference.ConvertBacktrackToWorldPos(AStarResult);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 v3SteeringForce = Vector3.zero;

        if(Route != null)
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
                bUseArrive = true;
                v3SteeringForce = Seek(Route[iCurrentRoutePoint]);
            }
            else
            {
                //Ahora sí, hay ue mover el agente hacia el punto destino.
                v3SteeringForce =  Seek(Route[iCurrentRoutePoint]);
            }
        }

        // Idealmente, usaríamos el ForceMode de Force, para tomar en cuenta la masa del objeto.
        // Aquí ya no usamos el deltaTime porque viene integrado en como funciona AddForce.
        myRigidbody.AddForce(v3SteeringForce, ForceMode.Acceleration);

        //Hacemos un Clamp para que no exceda la velocidad máxima que puede tener el agente
        myRigidbody.velocity = Vector3.ClampMagnitude(myRigidbody.velocity, fMaxSpeed);

        
    }
}
