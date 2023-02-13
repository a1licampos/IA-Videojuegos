using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touch_Agent : MonoBehaviour
{

    public GameObject goAgent;
    public GameObject goAgente_Prefab;

    public bool bTouched;
    public bool bCreate;

    public float fTimeBase_Destroy;
    public float fTime_Destroy;


    void Start()
    {
        fTime_Destroy = fTimeBase_Destroy;   //Asignamos el tiempo base que pasara para volver a crear al agente 
    }

    // Update is called once per frame
    void Update()
    {
        if (bTouched)                       //Si el vigilante ataca al agente
        {
            Destroy(goAgent);                //Destruimos al agente

            bTouched = false;
            bCreate = true;
        }

        if (fTime_Destroy > 0 && !bTouched && bCreate)      //Esperamos el tiempo necesario para volver a crear al agente
        {
            fTime_Destroy -= Time.deltaTime;
        }
        else if (bCreate)           
        {
            fTime_Destroy = fTimeBase_Destroy;              //Reiniciamos valores

            bCreate = false;

            goAgent = Instantiate(goAgente_Prefab, new Vector3(0, 3.5f, 0), Quaternion.identity);     //Creamos al agente y lo asignamos al gameObject gAgent
        }


    }
}
