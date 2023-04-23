using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToClick : MonoBehaviour
{
    // Basado en el script mostrado en:
    // https://docs.unity3d.com/Manual/nav-MoveToClickPoint.html
    // A�adir la mascara de colision para que solo cheque contra el piso y no contra todas las 
    // capas.

    UnityEngine.AI.NavMeshAgent _agent = null;
    LayerMask floorMask;

    public Animator mAnimatorPlayer;

    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        floorMask = LayerMask.GetMask("Floor");
        mAnimatorPlayer = GetComponentInChildren<Animator>();
    }

            RaycastHit hit;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100.0f, floorMask))
            {
                // Le decimos que vaya al punto en el piso que choc? con el rayo de la c?mara.
                _agent.destination = hit.point;
                
            }
        }


        if (_agent.velocity.magnitude == 0.0f)
        {
            mAnimatorPlayer.SetBool("Moving", false);
        }
        else
        {
            mAnimatorPlayer.SetBool("Moving", true);
        }
    }

    private void OnDrawGizmos()
    {
        if (_agent != null && _agent.destination != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(_agent.destination, 0.2f);
        }
    }
}
