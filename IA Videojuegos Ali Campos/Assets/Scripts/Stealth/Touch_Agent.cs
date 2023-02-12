using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touch_Agent : MonoBehaviour
{

    public GameObject gAgent;
    public GameObject gAgente_Prefab;

    public bool bTouched;
    public bool bCreate;

    public float fTimeBase_Destroy;
    public float fTime_Destroy;


    void Start()
    {
        fTime_Destroy = fTimeBase_Destroy;    
    }

    // Update is called once per frame
    void Update()
    {
        if (bTouched)
        {
            Destroy(gAgent);

            bTouched = false;
            bCreate = true;
        }

        if (fTime_Destroy > 0 && !bTouched && bCreate)
        {
            fTime_Destroy -= Time.deltaTime;
        }
        else if (bCreate)
        {
            fTime_Destroy = fTimeBase_Destroy;
            //bTouched = true;
            //gAgent.SetActive(true);
            //gAgent.transform.position = new Vector3(0, 3.5f, 0);

            bCreate = false;

            gAgent = Instantiate(gAgente_Prefab, new Vector3(0, 3.5f, 0), Quaternion.identity);
        }


    }
}
