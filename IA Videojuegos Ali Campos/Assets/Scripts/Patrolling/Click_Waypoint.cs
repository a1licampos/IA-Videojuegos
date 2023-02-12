using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Click_Waypoint : MonoBehaviour
{
    public MousePoint_Creator sMousePoint_Creator;
    public TextMeshProUGUI txtNumber;
    public int i;

    void Update()
    {
        sMousePoint_Creator = GameObject.FindGameObjectWithTag("Creator").GetComponent<MousePoint_Creator>();

        i = sMousePoint_Creator.lWayPoints.IndexOf(gameObject);

        txtNumber.text = i.ToString();
    }

    private void OnMouseDown()
    {
        sMousePoint_Creator.lWayPoints.Remove(gameObject);
        Destroy(gameObject);
    }
}
