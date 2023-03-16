using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseGrid : MonoBehaviour
{
    [Header("MOUSE")]
    public Vector3 v3MousePosition = Vector3.zero;
    public Vector3 v3Prueba = Vector3.zero;

    [Header("PRUEBA")]
    //Una instancia que se referencia aquí
    Pathfinding_Test _PathfindingReference;
    ClassGrid _GridReference;
    public int a;

    // Start is called before the first frame update
    void Start()
    {
        _PathfindingReference = GameObject.FindGameObjectWithTag("grid").GetComponent<Pathfinding_Test>();

        _GridReference = _PathfindingReference.myGrid;
    }

    // Update is called once per frame
    void Update()
    {
        //Posicion Objetivo a traves del click del mouse
        if (Input.GetMouseButtonDown(0))
        {
            v3MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        Debug.LogWarning((v3MousePosition.x) + "<-------");
        
        Debug.LogWarning(_GridReference.GetTilePosition(v3MousePosition.x, v3MousePosition.y));
        v3Prueba = _GridReference.GetTilePosition(Mathf.FloorToInt(v3MousePosition.x), v3MousePosition.y);

        Debug.LogWarning((int)v3Prueba.x);
        _PathfindingReference.StartPosition.x = (int)v3Prueba.x;
        _PathfindingReference.StartPosition.y = (int)v3Prueba.y;
        //_GridReference.AStarSearch((int)v3Prueba.x, (int)v3Prueba.y,
        //                           4,4);

    }
}
