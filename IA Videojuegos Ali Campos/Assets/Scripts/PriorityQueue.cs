using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue
{
    private List<Node> nodes = new List<Node>();

    public List<Node> Nodes
    {
        get { return nodes; }
    }

    public int Count
    {
        get { return nodes.Count; }
    }

    public void Add(Node in_node)
    {
        nodes.Add(in_node);
    }

    //Meter un elemento en cualquier lugar (inicio, medio o final)
    public void Insert(int iPriority, Node in_node)
    {
        //Idealmente, habr�a que medir el tiempo promedio/amortiuzado de la ejecuci�n con los ifs juntos
        //Inserta a in_node en la posici�n de la lista donde haya alg�n elemento con prioridad mayor
        for(int i = 0; i < nodes.Count; i++)
        {
            //Camibar el f_Cost por g_Cost est�mos rompiendo los otros algoritmos, pero A* es m�s importante
            if(nodes[i].f_Cost > in_node.f_Cost)
            {
                nodes.Insert(i, in_node);
                return;
            }
            else if (nodes[i].f_Cost == in_node.f_Cost &&
                     nodes[i].h_Cost > in_node.h_Cost)
            {
                //Este es el caso en que tienen el mismo f_cost pero el node a insertar tiene menor h_cost
                //https://youtu.be/i0x5fj4PqP4
                nodes.Insert(i, in_node);
                return;
            }
        }
        //Si nunca encontr� a alguien con mayor costo que �l, entonces in_node es el de mayor costo
        //y debe ir hasta atr�s de la lista de prioridad
        nodes.Add(in_node);
    }

    public void Remove(Node in_node)
    {
        nodes.Remove(in_node);
    }


    //Desencolar, atender a la primera petici�n de la cola
    public Node Dequeue()
    {
        Node out_node = nodes[0];
        nodes.RemoveAt(0);

        return out_node;
    }

    public Node GetAt(int i)
    {
        return nodes[i];
    }

    public bool Contains(Node in_node)
    {
        return nodes.Contains(in_node);
    }

}
