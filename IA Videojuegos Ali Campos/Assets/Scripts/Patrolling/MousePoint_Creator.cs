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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Posicion donde crearemos el waypoint
        if (Input.GetMouseButtonDown(1))
        {
            v3MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            v3MousePosition.z = 0.0f;

            lWayPoints.Add(Instantiate(gbWayPoints, v3MousePosition, Quaternion.identity));
        }
    }

    /*
     * Hacer un arrive hasta los puntos
     * cuando hace collider enter aumentar el indice de la lista del arrive
     * listo
     * al crear un way point se hace push
     * al quitarlo se hace pop ¿Se recorrera el array?
     */
}
