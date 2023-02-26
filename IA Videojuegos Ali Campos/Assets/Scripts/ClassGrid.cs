using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

    //Este es para a* y djikstra
    public float g_Cost;            //Que costo tiene llegar al nodo
    public float fTerrainCost;
    public bool bWalkable;

    public Node(int in_x, int in_y)
    {
        this.x = in_x;
        this.y = in_y;
        this.Parent = null;
        this.g_Cost = int.MaxValue;
        this.fTerrainCost = 1;
        this.bWalkable = true;
    }

    public override string ToString()
    {
        return y.ToString() + ", " + x.ToString();
    }
}

public class Graph
{
    public List<Node> Nodes;
}

public class ClassGrid 
{
    public int iHeight = 10;
    public int iWidth = 10;

    //Dibujar el grid
    private float fTileSize;
    private Vector3 v3OriginPosition;

    public Node[,] Nodes;
    public TextMesh[,] debugTextArray;

    public bool bShowDebug = true;

    public GameObject debugGO = null;

    //Constructor
    public ClassGrid(int in_Height, int in_Width, float in_fTileSize = 10.0f, Vector3 in_v3OriginPosition = default)
    {
        iHeight = in_Height;
        iWidth = in_Width;

        InitGrid();

        //Con esta variable vamos a crear objetos de tipos textMesh
        this.fTileSize = in_fTileSize;
        this.v3OriginPosition = in_v3OriginPosition;

        if(bShowDebug)
        {
            debugGO = new GameObject("GridDebugParent");

            debugTextArray = new TextMesh[iHeight, iWidth];

            for (int y = 0; y < iHeight; y++)
            {
                for (int x = 0; x < iWidth; x++)
                {
                    debugTextArray[y, x] = CreateWorldText(Nodes[x, y].ToString(),
                                                           debugGO.transform,
                                                           GetWorldPosition(x, y) + new Vector3(fTileSize * 0.5f, fTileSize * 0.5f),
                                                           30,
                                                           Color.white,
                                                           TextAnchor.MiddleCenter);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
                }
            }

            Debug.DrawLine(GetWorldPosition(0, iHeight), GetWorldPosition(iWidth, iHeight), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(iWidth, 0), GetWorldPosition(iWidth, iHeight), Color.white, 100f);
        }
    }

    public void InitGrid()
    {
        Nodes = new Node[iHeight, iWidth];

        for (int y = 0; y < iHeight; y++)
        {
            for (int x = 0; x < iWidth; x++)
            {
                Nodes[y, x] = new Node(x, y);
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

            //Checamos si llegamos al destino
            if(currentNode == EndNode)
            {
                //Encontramos un camino.
                Debug.Log("Camino encontrado");

                //Necesitamos construir ese camino. Para eso hacemos backtracking
                List<Node> path = Backtrack(currentNode);
                EnumeratePath(path);

                return path;
            }

            //Otra posible solución, con caminos pequeños
            if(ClosedList.Contains(currentNode))
            {
                continue;
            }

            ClosedList.Add(currentNode);

            //Vamos a visitar a todos sus vecinos
            List<Node> currentNeighbors = GetNeighbors(currentNode);


            //Meterlos a la pila en el orden inverso para que al sacarlos nos den el orden "normal"
            for (int x = currentNeighbors.Count - 1; x >= 0; x--)
            {
                //Solo queremos nodos que no estén en la lista cerrada (la cerrada contiene nodos ya visitados)
                if(currentNeighbors[x].bWalkable 
                  /* && !OpenList.Contains(currentNeighbors[x])*/       //Otra posible solución, para caminos más grandes
                   && !ClosedList.Contains(currentNeighbors[x]))
                {
                    currentNeighbors[x].Parent = currentNode;
                    OpenList.Push(currentNeighbors[x]);
                }
            }
        }

        Debug.LogError("No path found between start and end.");

        return null;
    }

    public List<Node> BreadthFirstSearch(int in_startX, int in_startY, int in_endX, int in_endY)
    {

        Node StartNode = GetNode(in_startY, in_startX);
        Node EndNode = GetNode(in_endY, in_endX);

        if (StartNode == null || EndNode == null)
        {
            Debug.LogError("Invalid coordinates in BreadthFirstSearch");
            return null;
        }

        Queue<Node> OpenList = new Queue<Node>();
        List<Node> ClosedList = new List<Node>();

        OpenList.Enqueue(StartNode);

        //Prioridad
        //int iP = 0; 

        while (OpenList.Count > 0)
        {
            //Mientras haya nodos en la lista abierta, vamos a buscar un camino
            //Obtenemos el primer nodo de la lista abierta
            Node currentNode = OpenList.Dequeue();
            Debug.Log("Current Node is: " + currentNode.x + ", " + currentNode.y);

            //Checamos si llegamos al destino
            if (currentNode == EndNode)
            {
                //Encontramos un camino.
                Debug.Log("Camino encontrado");

                //Necesitamos construir ese camino. Para eso hacemos backtracking
                List<Node> path = Backtrack(currentNode);
                EnumeratePath(path);

                return path;
            }

            //Otra posible solución, con caminos pequeños
            if (ClosedList.Contains(currentNode))
            {
                continue;
            }

            ClosedList.Add(currentNode);

            //Vamos a visitar los vecinos de la derecha y arriba
            List<Node> currentNeighbors = GetNeighbors(currentNode);

            foreach (Node neighbor in currentNeighbors)
            {
                if (ClosedList.Contains(neighbor))
                    continue;

                //Si no lo contiene, entonces lo agregamos a la lista Abierta
                neighbor.Parent = currentNode;

                //Lo mandamos a llamar para cada vecino
                //OpenList.Insert(iP, neighbor);
                OpenList.Enqueue(neighbor);

                //Ajustamos la prioridad, para que cada nuevo que entre sea añada al último
                //iP++;
            }

            string RemainingNodes = "Nodes in open list are: ";
            
            //for(int i = 0; i < OpenList.Count; i++)
            //    RemainingNodes += "(" + OpenList.GetAt(i).x + ", " + OpenList.GetAt(i).y + ") // ";

            foreach (Node n in OpenList)
                RemainingNodes += "(" + n.x + ", " + n.y + ") // ";

            Debug.Log(RemainingNodes);

        }

        Debug.LogError("No path found between start and end.");

        return null;
    }

    public List<Node> BestFirstSearch(int in_startX, int in_startY, int in_endX, int in_endY)
    {

        Node StartNode = GetNode(in_startY, in_startX);
        Node EndNode = GetNode(in_endY, in_endX);

        if (StartNode == null || EndNode == null)
        {
            Debug.LogError("Invalid coordinates in BestFirstSearch");
            return null;
        }

        PriorityQueue OpenList = new PriorityQueue();
        List<Node> ClosedList = new List<Node>();

        OpenList.Add(StartNode);

        while (OpenList.Count > 0)
        {
            //Mientras haya nodos en la lista abierta, vamos a buscar un camino
            //Obtenemos el primer nodo de la lista abierta
            Node currentNode = OpenList.Dequeue();
            Debug.Log("Current Node is: " + currentNode.x + ", " + currentNode.y);

            //Checamos si llegamos al destino
            if (currentNode == EndNode)
            {
                //Encontramos un camino.
                Debug.Log("Camino encontrado");

                //Necesitamos construir ese camino. Para eso hacemos backtracking
                List<Node> path = Backtrack(currentNode);
                EnumeratePath(path);

                return path;
            }

            //Checamos si ya está en la lista cerrada
            if (ClosedList.Contains(currentNode))
            {
                continue;
            }

            ClosedList.Add(currentNode);

            //Vamos a visitar a todos sus vecinos
            List<Node> currentNeighbors = GetNeighbors(currentNode);

            foreach (Node neighbor in currentNeighbors)
            {
                if (ClosedList.Contains(neighbor))
                    continue;

                //Si no lo contiene, entonces lo agregamos a la lista Abierta
                neighbor.Parent = currentNode;

                int dist = GetDistance(neighbor, EndNode);
                Debug.Log("dist between: " + neighbor.x + ", " + neighbor.y + " and goal is: " + dist);

                neighbor.g_Cost = dist;

                //Lo mandamos a llamar para cada vecino
                OpenList.Insert(dist, neighbor);
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

        return null;
        
    }

    public List<Node> GetNeighbors(Node in_currentNode)
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

    //Euclidiana (hasta el momento)
    public int GetDistance(Node in_a, Node in_b)
    {
        int x_diff = (in_a.x - in_b.x);
        int y_diff = (in_b.y - in_a.y);
        return (int) Mathf.Sqrt(Mathf.Pow(x_diff, 2) + Mathf.Pow(y_diff, 2));
    }

    public static TextMesh CreateWorldText(string in_text, Transform in_parent = null,
                                           Vector3 in_localPosition = default,
                                           int in_iFontSize = 32, Color in_color = default,
                                           TextAnchor in_textAnchor = TextAnchor.UpperLeft, TextAlignment in_textAligment = TextAlignment.Left)
    {
        if (in_color == null) in_color = Color.white;

        GameObject goMyObject = new GameObject(in_text, typeof(TextMesh));

        goMyObject.transform.parent = in_parent;
        goMyObject.transform.localPosition = in_localPosition;

        TextMesh myTM = goMyObject.GetComponent<TextMesh>();
        myTM.text = in_text;
        myTM.anchor = in_textAnchor;
        myTM.alignment = in_textAligment;
        myTM.fontSize = in_iFontSize;
        myTM.color = in_color;

        return myTM;
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        //Nos regresa la posición en mundo del tile/cuadro especificado por X y Y.
        //Por eso lo multiplicamos por el fTileSize
        //(dado que tienen lo mismo de alto y ancho cada cuadro)
        //y finalmente sumamos la posición de origen del grid
        return new Vector3(x, y) * fTileSize + v3OriginPosition;
    }

    //Enumera un camino en el orden que tienen y lo muestra en los debugTextArray
    public void EnumeratePath(List<Node> in_path)
    {
        int iCounter = 0;
        foreach(Node n in in_path)
        {
            iCounter++;
            debugTextArray[n.y, n.x].text = n.ToString()
                                            + Environment.NewLine + "step: "
                                            + iCounter.ToString();
        }
    }
}


