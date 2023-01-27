using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Position_Circle : MonoBehaviour
{
    public float CircleDistance;
    public Vector2 CircleCenter;

    public float CircleRadius;
    public Vector2 Displacement;

    public float Angle_Change;
    public float WanderAngle;

    public Vector3 MoveVector;
    public float Speed;

    public Vector3 WanderForce;

    public float RotationSpeed;


    // Update is called once per frame
    void FixedUpdate()
    {
        CircleCenter = new Vector2(transform.forward.x, transform.forward.y);//, transform.forward.z);
        CircleCenter.Normalize();
        CircleCenter *= CircleDistance;

        Displacement = new Vector3(0, 1, 0);    //Cambie el z = 1 a y = 1
        Displacement *= CircleRadius;

        WanderAngle += (Random.value * Angle_Change) - (Angle_Change * 0.5f);

        SetAngle(ref Displacement, WanderAngle);

        WanderForce = CircleCenter + Displacement;

        MoveVector = (transform.forward * Speed) + WanderForce;
        MoveVector.Normalize();
        MoveVector *= Speed;

        MoveVector.z = 0;

        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(MoveVector), RotationSpeed * Time.deltaTime);
        transform.position += MoveVector * Time.deltaTime;
    }

    Vector2 Wander()
    {
        CircleCenter = new Vector2(transform.forward.x, transform.forward.y);//, transform.forward.z);
        CircleCenter.Normalize();
        CircleCenter *= CircleDistance;

        Displacement = new Vector3(0, 1, 0);    //Cambie el z = 1 a y = 1
        Displacement *= CircleRadius;

        WanderAngle += (Random.value * Angle_Change) - (Angle_Change * 0.5f);

        SetAngle(ref Displacement, WanderAngle);

        return WanderForce = CircleCenter + Displacement;
    }

    void SetAngle(ref Vector2 vector, float angle)
    {
        float len = vector.magnitude;

        vector.x = Mathf.Cos(angle) * len;

        vector.y = Mathf.Sin(angle) * len;
    }
}
