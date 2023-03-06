using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class Pathfinding_Test : MonoBehaviour
{
    //public Transform t;

    public int2 StartPosition = int2.zero;
    public int2 EndPosition = int2.zero;

    public Steering_Behaviors myAgent;

    // Start is called before the first frame update
    void Start()
    {
        ClassGrid myTest = new ClassGrid(5, 5);
        //myTest.DepthFirstSearch(0, 0, 4, 4);
        //myTest.BestFirstSearch(0, 0, 2, 0);

        //TAREA
        //Objetivo: 0,0 a 2,2
        //myTest.BreadthFirstSearch(0, 0, 2, 2);

        //Objetivo: 2,2 a 1,1
        //myTest.BreadthFirstSearch(2, 2, 1, 1);

        //No hay camino (test)
        //myTest.BreadthFirstSearch(0, 0, -1, -1);

        //myTest.AStarSearch(0, 1, 4, 0);

        List<Node> Pathfinding_result = myTest.AStarSearch(StartPosition.x, StartPosition.y, EndPosition.x, EndPosition.y);
        List<Vector3> WorldPositionPathfinding = new List<Vector3>();

        foreach(Node n in Pathfinding_result)
        {
            WorldPositionPathfinding.Add(myTest.GetWorldPosition(n.x, n.y));
        }

        //myAgent.Función() para asignarle la ruta a seguir al agente de pathfinding
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
