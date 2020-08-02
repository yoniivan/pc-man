using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Algorithem
{

    public List<Vertex> computePaths(List<Vertex> graph, Vertex sourceVertex)
    {
        List<Vertex> list = graph;
        List<Vertex> prioQueue = new List<Vertex>();
        foreach (Vertex v in list)
        {
            if (v.getName() != sourceVertex.getName())
            {
                prioQueue.Add(v);
            }
        }
        sourceVertex = graph.Find(v => v.getName().Equals(sourceVertex.getName()));
        sourceVertex.setDistance(0);

        prioQueue.Add(sourceVertex);

        while (prioQueue.Count != 0)
        {
            bubbleSort(prioQueue);
            Vertex actualVertex = prioQueue[0];
            string x = actualVertex.getName();
            prioQueue.Remove(actualVertex);

            int len = actualVertex.getEdgeList().Count;
            foreach (Edge edge in actualVertex.getEdgeList())
            {
                string vName = edge.getEndV().getName();
                Vertex v = null;
                foreach(Vertex node in list)
                {
                    if(vName == node.getName())
                    {
                        v = node;
                        break;
                    }
                }

                float newDistance = actualVertex.getDistance() + edge.getWeight();

                if (newDistance < v.getDistance())
                {
                    v.setDistance(newDistance);
                    v.setLastVertex(actualVertex);
                    prioQueue.Add(v);
                }
            }
        }
        return list;
    }

    public List<Vertex> getShortestPath(Vertex targetVertex, List<Vertex> list)
    {
        List<Vertex> shortestPathToTarget = new List<Vertex>();

        Vertex v = list.Find(i => i.getName().Equals(targetVertex.getName()));

        for (Vertex vertex = v; vertex != null; vertex = vertex.getLastVertex())
        {
            shortestPathToTarget.Add(vertex);
        }

        shortestPathToTarget.Reverse();
        return shortestPathToTarget;
    }

    private void bubbleSort(List<Vertex> list) // Flag = true -> x, Flag = false -> z
    {
        List<Vertex> tempList = list;
        Vertex temp;

        for (int i = 0; i <= tempList.Count - 2; i++)
        {
            for (int j = 0; j <= tempList.Count - 2; j++)
            {
                {
                    if (tempList[j].getDistance() > tempList[j + 1].getDistance())
                    {
                        temp = tempList[j + 1];
                        tempList[j + 1] = tempList[j];
                        tempList[j] = temp;
                    }
                }
            }
        }
    }

}
