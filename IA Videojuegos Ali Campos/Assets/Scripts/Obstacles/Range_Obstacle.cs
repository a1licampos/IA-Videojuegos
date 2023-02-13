using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Range_Obstacle : MonoBehaviour
{
    public float fRadius;

    void Update()
    {
        GetComponent<SphereCollider>().radius = fRadius;    //Estamos actualizando constatemente el radio del collider de la esfera
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;                          
        Gizmos.DrawWireSphere(transform.position, fRadius); //Mostramos el rango al cual el agente empieza a esquivarlos
    }
}
