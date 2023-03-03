using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding_Test : MonoBehaviour
{
    public Transform t;
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

        myTest.AStarSearch(0, 0, 4, 4);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
