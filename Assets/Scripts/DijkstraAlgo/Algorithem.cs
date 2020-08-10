using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Algorithem
{

    public List<Vertex> computePaths(List<Vertex> graph, Vertex sourceVertex)
    {
        List<Vertex> list = graph;
        Stack<Vertex> prioQueue = new Stack<Vertex>();
        foreach (Vertex v in list)
        {
            if (v.Name != sourceVertex.Name)
            {
                prioQueue.Push(v);
            }
        }
        sourceVertex = graph.Find(v => v.Name.Equals(sourceVertex.Name));
        sourceVertex.Distance = 0;

        prioQueue.Push(sourceVertex);

        while (prioQueue.Count != 0)
        {
            Vertex actualVertex = prioQueue.Pop();

            foreach (Edge edge in actualVertex.EdgeList)
            {
                string vName = edge.EndV.Name;
                Vertex v = null;
                foreach(Vertex node in list)
                {
                    if(vName == node.Name)
                    {
                        v = node;
                        break;
                    }
                }

                float newDistance = actualVertex.Distance + edge.Weight;

                if (newDistance < v.Distance)
                {
                    v.Distance = newDistance;
                    v.LastVertex = actualVertex;
                    prioQueue.Push(v);
                }
            }
        }
        return list;
    }

    public List<Vertex> getShortestPath(Vertex targetVertex, List<Vertex> list)
    {
        List<Vertex> shortestPathToTarget = new List<Vertex>();
        Vertex v = list.Find(i => i.Name.Equals(targetVertex.Name));

        for (Vertex vertex = v; vertex != null; vertex = vertex.LastVertex)
        {
            shortestPathToTarget.Add(vertex);
        }

        shortestPathToTarget.Reverse();
        return shortestPathToTarget;
    }
}
