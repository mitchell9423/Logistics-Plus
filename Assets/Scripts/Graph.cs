using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph
{
    private List<int> values;
    private int totalVertices;
    private LinkedList<int>[] adjacencyList;
    public LinkedList<int>[] AdjacencyList { get; }
    public int Size { get { return totalVertices; } }
    public Graph(List<int> _values)
    {
        values = _values;
        int n = _values.Count;
        totalVertices = n;
        adjacencyList = new LinkedList<int>[n];

        for (int i = 0; i < n; i++)
        {
            adjacencyList[i] = new LinkedList<int>();
        }

        AddEdges();
    }

    public void AddEdges()
    {
        for (int i = 0; i < totalVertices; i++)
        {
            for (int j = 0; j < totalVertices; j++)
            {
                if (j != i) adjacencyList[i].AddLast(j);
            }
        }
    }

    public string PrintAdjacencyList()
    {
        string nodeString = "";
        for (int i = 0; i < adjacencyList.Length; i++)
        {
            if (adjacencyList[i].Count > 0)
            {
                nodeString += $"[Node Value: {values[i]} with Neighbors";
                foreach (var item in adjacencyList[i])
                {
                    nodeString += $" -> {values[item]}";
                }
                nodeString += $" ]\n";
            }
        }

        return nodeString;
    }
}
