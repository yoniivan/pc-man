using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertex : IComparer<Vertex>
{
    public string Name { get; set; }
    public float CordX { get; set; }
    public float CordZ { get; set; }
    public List<Edge> EdgeList { get; set; }
    public bool Visited { get; set; }
    public Vertex LastVertex { get; set; }
    public float Distance { get; set; } = float.MaxValue;

    public Vertex(string name, float cordX, float cordZ)
    {
        Name = name;
        CordX = cordX;
        CordZ = cordZ;
        EdgeList = new List<Edge>();
    }

    public Vertex(string name)
    {
        Name = name;
    }

    public void addEdge(Edge edge)
    {
        EdgeList.Add(edge);
    }


    public int Compare(Vertex start, Vertex end)
    {
        return start.Distance.CompareTo(end.Distance);
    }
}

