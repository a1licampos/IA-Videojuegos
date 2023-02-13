using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge
{
    public Node A;
    public Node B;
    public float fCost;
}

//Se puede manejar como un tipo de dato estandar, directamente en memoria
public class Node
{
    public int x;
    public int y;

    //public List<Edge> Neighbors;    
    public Node Parent;
    public float fTerrainCost; 

    //Este es para a* y djikstra
    public float g_Cost;            //Que costo tiene llegar al nodo
}

public class Graph
{
    public List<Node> Nodes;
}

public class ClassGrid 
{
    public int iHeight = 10;
    public int iWidth = 10;

    public Node[,] Nodes;

    public ClassGrid(int in_Height, int in_Width)
    {
        iHeight = in_Height;
        iWidth = in_Width;

        Nodes = new Node[iHeight, iWidth];

        for(int y = 0; y < iHeight; y++)
        {
            for (int x = 0; x < iWidth; x++)
            {
                Nodes[y, x].x = x;
                Nodes[y, x].y = y;
                Nodes[y, x].Parent = null;
                Nodes[y, x].g_Cost = int.MaxValue;
                Nodes[y, x].fTerrainCost = 1;

                //Nodes[y, x].Neighbors = new List<Edge>();
                //Nodes[y, x].Neighbors.Add();
            }
        }
    }

    //Quiero encontrar un camino de start a end.
    public List<Node> DepthFirstSearch(int in_startX, int in_startY, int in_endX, int in_endY)
    {

        Node StartNode = GetNode(in_startY, in_startX);
        Node EndNode = GetNode(in_endY, in_endX);

        if(StartNode == null || EndNode == null)
        {
            Debug.LogError("Invalid coordinates in DeepthFirstSearch");
            return null;
        }

        Stack<Node> OpenList = new Stack<Node>();
        List<Node> ClosedList = new List<Node>();

        OpenList.Push(StartNode);

        while(OpenList.Count > 0)
        {
            //Mientras haya nodos en la lista abierta, vamos a buscar un camino

            //Obtenemos el primer nodo de la lista abierta
            Node currentNode = OpenList.Pop();
            Debug.Log("Current Node is: " + currentNode.x + ", " + currentNode.y);

            if(currentNode == EndNode)
            {
                //Encontramos un camino.
                Debug.Log("Camino encontrado");

                //Necesitamos construir ese camino. Para eso hacemos backtracking
                return Backtrack(currentNode);
            }

            ClosedList.Add(currentNode);

            //Vamos a visitar a todos sus vecinos
            List<Node> currentNeighbors = GetNighborts(currentNode);
            foreach(Node neighbors in currentNeighbors)
            {
                if (ClosedList.Contains(neighbors))
                    continue;

                //Si no lo contiene, entonces lo agregamos a la lista
                neighbors.Parent = currentNode;
                OpenList.Push(neighbors);
            }
        }

        Debug.LogError("No path found between start and end.");

        return null;
    }

    public Node GetNode(int x, int y)
    {
        //Checamos si las coordenadas dentro de nuestra cuadricula se repitre
        if (x < iWidth && x >= 0 && y < iHeight && y >= 0)
        {
            return Nodes[y, x];
        }
        else
        {
            Debug.LogError("Invalid coordinates in DeepthFirstSearch");
            return null;
        }
    }

    public List<Node> GetNighborts(Node in_currentNode)
    {
        List<Node> out_Neighbors = new List<Node>();

        //Visitamos al nodo de arriba
        int x = in_currentNode.x;
        int y = in_currentNode.y;

        //Arriba
        if (GetNode(y + 1, x) != null)
        {
            out_Neighbors.Add(Nodes[y + 1, x]);
        }

        //Izquierda
        if (GetNode(y, x - 1) != null)
        {
            out_Neighbors.Add(Nodes[y, x - 1]);
        }

        //Derecha
        if (GetNode(y, x + 1) != null)
        {
            out_Neighbors.Add(Nodes[y, x + 1]);
        }

        //Abajo
        if (GetNode(y - 1, x) != null)
        {
            out_Neighbors.Add(Nodes[y - 1, x]);
        }

        return out_Neighbors;
    }


    public List<Node> Backtrack(Node in_node)
    {
        List<Node> out_Path = new List<Node>();
        Node current = in_node;

        while(current.Parent != null)
        {
            out_Path.Add(current);
            current = current.Parent;
        }

        out_Path.Add(current);
        out_Path.Reverse();

        return out_Path;
    }
}


