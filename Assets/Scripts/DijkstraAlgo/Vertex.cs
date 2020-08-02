using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertex : IComparer<Vertex>
{
    private string name;
    private float cordX, cordZ;
    private List<Edge> edgeList;
    private bool visited;
    private Vertex lastVertex;
    private float distance = float.MaxValue;

    public Vertex(string name, float cordX, float cordZ)
    {
        this.name = name;
        this.cordX = cordX;
        this.cordZ = cordZ;
        this.edgeList = new List<Edge>();
    }
    
    public Vertex(string name)
    {
        this.name = name;
    }

    public void setLastVertex(Vertex lastVertex)
    {
        this.lastVertex = lastVertex;
    }

    public Vertex getLastVertex()
    {
        return lastVertex;
    }

    public void addEdge(Edge edge)
    {
        this.edgeList.Add(edge);
    }

    public string getName()
    {
        return this.name;
    }

    public void setName(string name)
    {
        this.name = name;
    }

    public List<Edge> getEdgeList()
    {
        return edgeList;
    }

    public void setEdgeList(List<Edge> edgeList)
    {
        this.edgeList = edgeList;
    }

    public void setDistance(float distance)
    {
        this.distance = distance;
    }

    public float getDistance()
    {
        return distance;
    }

    public float getCordX()
    {
        return cordX;
    }

    public float getCordZ()
    {
        return cordZ;
    }

    public int Compare(Vertex start, Vertex end)
    {
        return start.distance.CompareTo(end.distance);
    }
}

