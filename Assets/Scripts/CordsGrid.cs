using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CordsGrid : MonoBehaviour
{
    GameObject CrossGridArr;
    [SerializeField]
    private float m_MinimumDistance;
    [SerializeField]
    private GameObject Enemy;
    private List<GameObject> objList;

    // Start is called before the first frame update
    void Start()
    {

    }

    public float[] getSingleObjCords(string name)
    {
        float[] cords = new float[2];
        foreach(GameObject obj in objList)
        {
            if (obj.name.Equals(name))
            {
                float z = obj.transform.localPosition.z;
                float x = obj.transform.localPosition.x;
                cords[0] = x;
                cords[1] = z;
            }
        }
        return cords;
    }

    public List<Vertex> Generate()
    {
        List<Vertex> graph = new List<Vertex>();
        objList = GenerateCrossList();

        foreach (GameObject i in objList)
        {
            Vertex v = generateEgesForVertex(i);
            graph.Add(v);
        }

        return graph;
    }

    public Vertex generateEgesForVertex(GameObject g)
    {
        GameObject singleObj = g;
        List<GameObject> neighborList = new List<GameObject>();

        Vertex node = new Vertex(g.name, g.transform.position.x, g.transform.position.z);

        var getXpos = getPos(singleObj, objList, true);
        var getZpos = getPos(singleObj, objList, false);

        List<GameObject> xPos = getXpos.OrderBy(x => x.transform.position.x).ToList();
        List<GameObject> zPos = getZpos.OrderBy(z => z.transform.position.z).ToList();

        
        addToNeighbor(neighborList, checkAmoutOfNodes(singleObj, xPos, true));
        addToNeighbor(neighborList, checkAmoutOfNodes(singleObj, zPos, false));

        neighborList = checkValidEdge(neighborList, g);

        for(int i = 0; i < neighborList.Count; i++)
        {
            float weight = calcWeight(g, neighborList[i]);
            if(weight == -1)
            {
                continue;
            }
            Vertex target = new Vertex(neighborList[i].name, neighborList[i].transform.position.x, neighborList[i].transform.position.z);
            Edge edge = new Edge(weight, node, target);
            node.addEdge(edge);
        }

        return node;
    }

    public int find(List<GameObject> list, GameObject obj)
    {
        return list.FindIndex(i => i.name.Equals(obj.name));
    }

    public List<GameObject> copyList(List<GameObject> list)
    {
        List<GameObject> newList = new List<GameObject>();
        foreach (GameObject v in list)
            newList.Add(v);

        return newList;
    }

    private List<GameObject> checkValidEdge(List<GameObject> edges, GameObject start)
    {
        List<GameObject> list = copyList(edges);
        string[] startWalls = start.tag.Split('_');
        foreach (GameObject edge in edges)
        {
            string[] endWalls = edge.tag.Split('_');
            string[] walls = combineArrays(startWalls, endWalls);

            bool direction = (edge.transform.position.x - start.transform.position.x) == 0 ? true : false; // false == xAxis, true zAxis.
            bool posNeg;
            if (direction)
            {
                posNeg = (start.transform.position.z - edge.transform.position.z) > 0 ? true : false; // true -> left == smoll, false -> right == yemin. 
            }
            else
            {
                posNeg = (start.transform.position.x - edge.transform.position.x) > 0 ? true : false;
            }

            if ( startWalls.Contains("RIGHT") && endWalls.Contains("LEFT") && !posNeg && !direction )
            {
                int index = list.FindIndex(i => i.name.Equals(edge.name));
                list.RemoveAt(index);
                continue;
            }

            if (startWalls.Contains("RIGHT") && endWalls.Contains("LEFT") && posNeg && !direction)
            {
                int index = list.FindIndex(i => i.name.Equals(edge.name));
                list.RemoveAt(index);
                continue;
            }

            if (startWalls.Contains("LEFT") && endWalls.Contains("RIGHT") && posNeg && !direction)
            {
                int index = list.FindIndex(i => i.name.Equals(edge.name));
                list.RemoveAt(index);
                continue;
            }


            if (startWalls.Contains("LEFT") && endWalls.Contains("RIGHT") && !posNeg && !direction)
            {
                int index = list.FindIndex(i => i.name.Equals(edge.name));
                list.RemoveAt(index);
                continue;
            }

            if (startWalls.Contains("UP") && endWalls.Contains("DOWN") && !posNeg && direction)
            {
                int index = list.FindIndex(i => i.name.Equals(edge.name));
                list.RemoveAt(index);
                continue;
            }

            if (startWalls.Contains("DOWN") && endWalls.Contains("UP") && posNeg && direction)
            {
                int index = list.FindIndex(i => i.name.Equals(edge.name));
                list.RemoveAt(index);
                continue;
            }
        }
        return list;
    }

    private string[] combineArrays(string[] start, string[] end)
    {
        string[] walls = new string[start.Length + end.Length];
        int cnt = 0;
        for(int i = 0; i < start.Length; i++)
        {
            walls[cnt++] = start[i];
        }

        for(int i = 0; i < end.Length; i++)
        {
            walls[cnt++] = end[i];
            
        }
        return walls;
    }

    private float calcWeight(GameObject first, GameObject end)
    {
        float weight = 0;
        if(first.transform.localPosition.x == end.transform.localPosition.x) // Zaxis
        {
            if(end.transform.localPosition.z > first.transform.localPosition.z) // Cheks witch is bigger.
            {
                weight = end.transform.localPosition.z - first.transform.localPosition.z;
            }
            else
            {
                weight = first.transform.localPosition.z - end.transform.localPosition.z;
            }
        }
        else // Xaxis
        {
            if(end.transform.localPosition.x > first.transform.localPosition.x)
            {
                weight = end.transform.localPosition.x - first.transform.localPosition.x;
            }
            else
            {
                weight = first.transform.localPosition.x - end.transform.localPosition.x;
            }
        }


        return weight;
    }

    // Adds objects from to a list.
    private void addToNeighbor(List<GameObject> neighbor, List<GameObject> list)
    {
        for(int i = 0; i < list.Count; i++)
        {
            neighbor.Add(list[i]);
        }
    }

    // Returns all neighbors from Xaxis.
    private List<GameObject> checkAmoutOfNodes(GameObject singleObj, List<GameObject> list, bool flag) // flag = true -> x, flag = false -> z.
    {
        if (list.Count() == 1)
        {
            return list;
        }
        List<GameObject> objList = new List<GameObject>();
        
        for (int i = 0; i < list.Count; i++)
        {
            if (flag)
            {
                if(singleObj.transform.localPosition.x == list[i].transform.localPosition.x && singleObj.transform.localPosition.z == list[i].transform.localPosition.z)
                {
                    if(i > 0 && i < list.Count - 1) // not last and not first
                    {
                        objList.Add(list[i - 1]);
                        objList.Add(list[i + 1]);
                        return objList;
                    }
                    else if(i == 0)
                    {
                        objList.Add(list[i + 1]);
                        return objList;
                    }
                    else if(i == list.Count - 1)
                    {
                        objList.Add(list[i - 1]);
                        return objList;
                    }
                }

            }
            else
            {
                if (singleObj.transform.localPosition.z == list[i].transform.localPosition.z && singleObj.transform.localPosition.x == list[i].transform.localPosition.x)
                {
                    if (i > 0 && i < list.Count - 1) // not last and not first
                    {
                        objList.Add(list[i - 1]);
                        objList.Add(list[i + 1]);
                        return objList;
                    }
                    else if (i == 0)
                    {
                        objList.Add(list[i + 1]);
                        return objList;
                    }
                    else if (i == list.Count - 1)
                    {
                        objList.Add(list[i - 1]);
                        return objList;
                    }
                }

            }
            
        }
        return null;
    }

    // Returns all Objects on Xaxis.
    private List<GameObject> getPos(GameObject obj, List<GameObject> list, bool flag) //flag = true -> x, flag = false -> z.
    {
        List<GameObject> pos = new List<GameObject>();
        pos.Add(obj);

        for(int i = 0; i < list.Count; i++)
        {
            if (flag)
            {
                if (obj.transform.localPosition.z == list[i].transform.localPosition.z && obj.transform.localPosition.x != list[i].transform.localPosition.x)
                {
                    pos.Add(list[i]);
                }
            }
            else
            {
                if (obj.transform.localPosition.z != list[i].transform.localPosition.z && obj.transform.localPosition.x == list[i].transform.localPosition.x)
                {
                    pos.Add(list[i]);
                }
            }

        }
        return pos;
    }

    // Returns all crosses from IU.
    private List<GameObject> GenerateCrossList()
    {
        List<GameObject> xAxis = new List<GameObject>();
        CrossGridArr = this.gameObject;
        int count = CrossGridArr.transform.childCount;
        for (int i = 0; i < count; i++)
        {
            GameObject g = CrossGridArr.transform.GetChild(i).gameObject;
            xAxis.Add(g);
        }
        return xAxis;
    }




    // Update is called once per frame
    void Update()
    {
        
    }
}

