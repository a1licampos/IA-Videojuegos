using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseGrid : MonoBehaviour
{
    public AStarAgent aStarAgent;

    [Header("MOUSE")]
    public Vector3 v3MousePosition = Vector3.zero;
    public Vector3 v3Prueba = Vector3.zero;

    [Header("START")]
    public GameObject visualStart;
    public bool bStart;
    public Vector2 v2Start = Vector2.zero;

    [Header("END")]
    public GameObject visualEnd;
    public bool bEnd;
    public Vector2 v2End = Vector2.zero;

    [Header("REFERENCIAS")]
    //Una instancia que se referencia aquí
    Pathfinding_Test _PathfindingReference;
    ClassGrid _GridReference;
    public int a;

    [Header("BASE")]
    public Vector2 v2OutCamera = new Vector2(1000, 1000);

    // Start is called before the first frame update
    void Start()
    {
        _PathfindingReference = GameObject.FindGameObjectWithTag("grid").GetComponent<Pathfinding_Test>();

        aStarAgent = GameObject.Find("AStarAgent2D").GetComponent<AStarAgent>();

        _GridReference = _PathfindingReference.myGrid;

        //Obtenemos el visual reference Start para ver el NODO INICIAL ESCOGIDO
        visualStart = GameObject.Find("Visual_StartCircle");
        //Lo sacamos de la vista de la cámara
        visualStart.transform.position = v2OutCamera;

        //Obtenemos el visual reference End para ver el NODO FINAL ECOGIDO
        visualEnd = GameObject.Find("Visual_EndCircle");
        //Lo sacamos de la vista de la cámara
        visualEnd.transform.position = v2OutCamera;
    }

    // Update is called once per frame
    void Update()
    {
        //Posicion Objetivo a traves del click DERECHO del mouse
        if (Input.GetMouseButtonDown(1))
        {
            //Obtenemos la posición del mouse
            v3MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //Establecemos en 0 su eje z ya que estamos en un entorno 2D
            v3MousePosition.z = 0f;

            //Obtenemos el vector tentativo/prueba
            v3Prueba = _GridReference.GetTilePosition(Mathf.FloorToInt(v3MousePosition.x), v3MousePosition.y);

            if (!bStart)    //Aún no se ha elegido el punto de partida
            {
                //Asignamos la posición de inicio
                visualStart.transform.position = v3MousePosition;

                //Asignamos la posición para usarse en A*
                _PathfindingReference.StartPosition.x = (int)v3Prueba.x;
                _PathfindingReference.StartPosition.y = (int)v3Prueba.y;

                v2Start = new Vector2((int)v3Prueba.x, (int)v3Prueba.y);
                bStart = true;

                //Asegurarnos que no tienen la misma posición
                if (v2Start == v2End)
                {
                    bEnd = false;   //De tener la misma posición cambiamos el lugar con Final y lo ponemos en falso
                    visualEnd.transform.position = v2OutCamera;
                }
            }
            else if (!bEnd) 
            {
                //Asignamos la posición de final
                visualEnd.transform.position = v3MousePosition;

                //Asignamos la posición para usarse en A*
                _PathfindingReference.EndPosition.x = (int)v3Prueba.x;
                _PathfindingReference.EndPosition.y = (int)v3Prueba.y;

                    v2End = new Vector2((int)v3Prueba.x, (int)v3Prueba.y);
                    bEnd = true;

                //Asegurarnos que no tienen la misma posición
                if (v2End == v2Start)
                {
                    bStart = false;     //De tener la misma posición cambiamos el lugar con Inicio y lo ponemos en falso
                    visualStart.transform.position = v2OutCamera;
                }
            }


        }

        //Evitamos que el reinicio ocurra cuando está haciendo el pathfinding
        if (Input.GetKeyDown(KeyCode.R) && !aStarAgent.bStartAStar)
        {
            RebootStartEnd();
        }

    }

    public void RebootStartEnd()
    {
        bStart = false;
        visualStart.transform.position = v2OutCamera;
        bEnd = false;
        visualEnd.transform.position = v2OutCamera;
    }
}
