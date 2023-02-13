using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePoint_Creator : MonoBehaviour
{
    [Header("MOUSE")]
    public Vector3 v3MousePosition = Vector3.zero;

    [Header("WAYPOINTS")]
    public GameObject gbWayPoints;
    public List<GameObject> lWayPoints = new List<GameObject>();


    void Update()
    {
        //Obtener la posicion donde crearemos el waypoint
        if (Input.GetMouseButtonDown(1))
        {
            v3MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);              //Obtenemos la posicion donde dimos lclick

            v3MousePosition.z = 0.0f;                                                           //Cambiamos la posicion en Z a 0 porque estamos en 2D

            lWayPoints.Add(Instantiate(gbWayPoints, v3MousePosition, Quaternion.identity));     //Instanciamos el waypoint y lo aregamos a la lista
        }
    }

    /*
     * LÓGICA INICIAL
     * Hacer un arrive hasta los puntos
     * cuando hace collider enter aumentar el indice de la lista del arrive
     * listo
     * al crear un way point se hace push
     * al quitarlo se hace pop ¿Se recorrera el array?
     */
}
