using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge
{
    // Start is called before the first frame update
    private float weight;
    private Vertex startV;
    private Vertex endV;

    public Edge(float weight, Vertex startV, Vertex endV)
    {
        this.weight = weight;
        this.startV = startV;
        this.endV = endV;
    }

    public float getWeight()
    {
        return weight;
    }

    public void setWeight(float weight)
    {
        this.weight = weight;
    }



    public Vertex getStartV()
    {
        return startV;
    }

    public void setStartV(Vertex startV)
    {
        this.startV = startV;
    }

    public Vertex getEndV()
    {
        return endV;
    }

    public void setEndV(Vertex endV)
    {
        this.endV = endV;
    }
}
