using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret_Rotate : MonoBehaviour
{
    public float fBaseTime = 4f;
    public float fTime;
    public float fSpeedTurning;

    public int iAngleZ;

    public bool bAlert;

    void Start()
    {
        fTime = fBaseTime;                      //Asignamos el tiempo para empezar la cuenta regresiva
    }


    //Nuestro Guardia gira con ángulos aleatorios cada x tiempo
    void Update()
    {
        if (!bAlert)                            //Comprobamos que no este en estado alerta
        {
            if (fTime > 0)
            {
                fTime -= Time.deltaTime;            //Vamos restando el tiempo progresivamente
            }
            else
            {
                fTime = fBaseTime;                  //Reiniciamos el tiempo
                iAngleZ = Random.Range(0, 361);     //Escogemos un ángulo aleatorio
            }

            //QUATERNIONES
            //  https://docs.unity3d.com/es/2018.4/Manual/QuaternionAndEulerRotationsInUnity.html
            //Quaternion.RotateTowards
            //  https://docs.unity3d.com/es/2018.4/ScriptReference/Quaternion.RotateTowards.html
            //Girar nuestra transformación un paso más cerca de la del objetivo.
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, iAngleZ), fSpeedTurning * Time.deltaTime);
        }
    }
}
