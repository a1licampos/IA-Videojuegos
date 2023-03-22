using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectPlayer : MonoBehaviour
{
    public SpriteRenderer spriteAgent;
    public AStarAgent aStarAgent;

    [Header("SELECTED")]
    public bool bSelected;
    //Sprites del agente
    public Sprite spriteBase;
    public Sprite spriteSelected;

    private void Start()
    {
        spriteAgent = GetComponent<SpriteRenderer>();
        aStarAgent = GameObject.Find("AStarAgent2D").GetComponent<AStarAgent>();
    }

    private void OnMouseDown()
    {
        if(!bSelected)                              //Si al hacer click en el agente no est� seleccionado
        {
            bSelected = true;                       //Se selecciona
            //spriteAgent.color = Color.gray;
            spriteAgent.sprite = spriteSelected;    //Se cambia su sprite o color
            aStarAgent.AgentSelected();             //Se avisa a la A* que el agente est� seleccionado
        }
        else                                        //De lo contrario quiere decir que est� selccionado
        {                                           //Por lo tanto lo deseleccionamos y reestablecemos sus valores
            bSelected = false;
            RebootColor();
            aStarAgent.AgentDeselected();
        }
    }

    //Colocamos el color o el sprite del agente sin seleccionar
    public void RebootColor()
    {
        //spriteAgent.color = Color.white;
        spriteAgent.sprite = spriteBase;
    }

    //Reiniciamos todos los valores
    public void Reboot()
    {
        RebootColor();
        bSelected = false;
    }
}
