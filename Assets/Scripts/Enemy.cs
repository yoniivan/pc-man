using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject m_Player;
    private player scriptPlayer;

    [SerializeField] private CordsGrid m_Grid;

    private List<Vertex> graph;

    [SerializeField] private float m_Speed = 1;
    private Rigidbody rb;
    private Rigidbody playerRigit;

    
    private bool flag = false;
    private bool firstCross = true;
    private bool isPathOne = false;

    private Vector3 v;
    private Vector3 nextPosition;
    private Vector3 prevPosition;

    private List<string> groupByList;
    private List<Vertex> path;
    private List<CrossPrevList> queueOfNodes = new List<CrossPrevList>();

    private void Awake()
    {
        m_Grid = FindObjectOfType<CordsGrid>();
        graph = m_Grid.Generate();
    }

    void Start()
    {
        Vertex tmp = graph.Find(v => v.Name.Equals("crossTags (1)")); // Testing
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!isPathOne)
        {
            if (!moveToCrossWidth())
                moveToNextPoint();
        }
        else
        {
            Vertex point = findLastPoint();
            if(point != null)
            {
                path.Add(point);
                isPathOne = !isPathOne;
            }
        }

        rb.MovePosition(v);
    }

    private Vertex findLastPoint()
    {
        CrossPrevList crossPrevList = queueOfNodes[queueOfNodes.Count - 1];
        Vertex node = graph.Find(i => i.Name.Equals(crossPrevList.Name));
        List<Edge> edgeList = node.EdgeList;
        Vertex nextPoint = null;

        foreach (Edge edge in edgeList)
        {
            var start = edge.StartV;
            var target = edge.EndV;

            bool direction = (start.CordX - target.CordX) == 0 ? false : true; // false -> z, true -> x
            var player = GameObject.Find("Player").transform.position;

            if (direction)
            {
                if (Math.Round(maxMin(start, target, true)[0].CordX, 1)  < Math.Round(player.x, 1) &&
                    Math.Round(maxMin(start, target, true)[1].CordX, 1) > Math.Round(player.x, 1))
                {
                    nextPoint = graph.Find(i => i.Name.Equals(target.Name));
                }
            }
            else
            {
                if (Math.Round(maxMin(start, target, false)[0].CordZ, 1) < Math.Round(player.z, 1) &&
                    Math.Round(maxMin(start, target, false)[1].CordZ, 1) > Math.Round(player.z, 1))
                {
                    nextPoint = graph.Find(i => i.Name.Equals(target.Name));
                }
            }
        }
        return nextPoint;
    }

    public List<Vertex> maxMin(Vertex a, Vertex b, bool direction)
    {
        List<Vertex> list = new List<Vertex>();
        list.Add(a);
        list.Add(b);

        return !direction ? list.OrderBy(i => i.CordZ).ToList() : list.OrderBy(i => i.CordX).ToList();
    }

    public Vertex[] maxMinZ(Vertex a, Vertex b)
    {
        Vertex[] list = new Vertex[2];

        if(a.CordZ > b.CordZ)
        {
            list[0] = a;
            list[1] = b;
        }
        else
        {
            list[0] = b;
            list[1] = a;
        }

        return list;
    }

    private Vertex getEnemyCloseCross()
    {
        var gObj = GameObject.Find("Player");
        if (gObj)
        {
            Vector3 player = gObj.transform.position;
            var distanceList = graph.OrderBy(node => Vector2.Distance(new Vector2(player.x, player.z), new Vector2(node.CordX, node.CordZ))).Take(4);

            List<Vertex> temp = new List<Vertex>();

            foreach(var node in distanceList)
            {
                string name = node.Name;
                var nodeObj = GameObject.Find(name);
                string[] tag = nodeObj.tag.Split('_');

                if((player.z - node.CordZ) > 0.5f && tag.Contains("UP"))
                    continue;
                if((player.z - node.CordZ) < 0f && tag.Contains("DOWN"))
                    continue;

                temp.Add(node);
            }

            return temp.First();
        }
        return null;
    }

    private bool moveToCrossWidth()
    {
        if (queueOfNodes.Count < 2)
            return false;

        groupByList = new List<string>();
        groupByList.Add(path[0].Name);
        groupByList.Add(path[1].Name);
        groupByList.Add(queueOfNodes[queueOfNodes.Count - 1].Name);
        groupByList.Add(queueOfNodes[queueOfNodes.Count - 2].Name);

        var str = groupByList.GroupBy(x => x).Select(x => new { Text = x.Key, Cnt = x.Count() });
        CrossPrevList currentNode;
        CrossPrevList prevNode;

        if(str.Count() == 2)
            flag = true;
        else
            flag = false;

        if (flag)
        {
            currentNode = queueOfNodes[queueOfNodes.Count - 2];
            prevNode = queueOfNodes[queueOfNodes.Count - 1];
        }
        else
        {
            // DEFAULT
            currentNode = queueOfNodes[queueOfNodes.Count - 1]; 
            prevNode = queueOfNodes[queueOfNodes.Count - 2];
        }

        if (path[0].CordX == prevNode.X) // height
        {
            if (currentNode.Z > prevNode.Z) // up
            {
                if (transform.position.z < currentNode.Z)
                {
                    v = moveEnemy(0, 1, false);
                    return true;
                    
                }
            }
            else // down
            {
                if (transform.position.z > currentNode.Z)
                {
                    v = moveEnemy(0, 1, true);
                    return true;
                }
            }
        }
        else // width
        {

            if (currentNode.X < prevNode.X) // left
            {
                if (currentNode.X < transform.position.x)
                {
                    v = moveEnemy(1, 0, true);
                    return true;
                }
            }
            else // right
            {
                if (transform.position.x < currentNode.X)
                {
                    v = moveEnemy(1, 0, false);
                    return true;
                }
            }
        }
        return false;
    }

    private void moveToNextPoint()
    {
        if (firstCross)
        {
            if (nextPosition.z < transform.position.z)
            {
                v = moveEnemy(0, 1, true);
            }
            else
            {
                v = moveEnemy(0, 0, true);
                firstCross = false;
            }
        }
        else
        {
            // calc z or x.
            List<Vertex> t =  path;
            bool direction = (path[0].CordZ - path[1].CordZ) == 0 ? true : false; // X cord -> true, Z cord -> false.
            bool leftRight; // + true / left, - false / right


            if (direction)
                leftRight = path[1].CordX > path[0].CordX ? true : false;
            else
                leftRight = path[1].CordZ > path[0].CordZ ? true : false;


            if (direction && leftRight)
                v = moveEnemy(1, 0, false);
            else if (direction && !leftRight)
                v = moveEnemy(1, 0, true);
            else if (!direction && leftRight)
                v = moveEnemy(0, 1, false);
            else if (!direction && !leftRight)
                v = moveEnemy(0, 1, true);
        }
    }

    private Vector3 moveEnemy(float x, float z, bool direction) // left == smoll -> true, right == yemun -> false || up == false, down == true
    {
        float xCord = x;
        float zCord = z;

        if (x == 0 && z != 0)
        {
            if (direction)
                zCord = m_Speed * Time.deltaTime * (-1);
            else
                zCord = m_Speed * Time.deltaTime;
        }
        if (x != 0 && z == 0)
        {
            if (direction)
                xCord = m_Speed * Time.deltaTime * (-1);
            else
                xCord = m_Speed * Time.deltaTime;
        }

        return new Vector3(transform.position.x + xCord, transform.localPosition.y, transform.position.z + zCord);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("crossTags"))
        {
            bool isValid = CrossPrevList.checkValidCross(queueOfNodes, other.name);

            if (!isValid)
            {
                CrossPrevList cross = new CrossPrevList(other.transform.position.x, other.transform.position.z, other.name);
                queueOfNodes.Add(cross);

                Vertex player = new Vertex(getEnemyCloseCross().Name);
                Vertex enemy = new Vertex(other.name);
                path = dijkstraPath(enemy, player);

                if (path.Count == 1)
                    isPathOne = true;
                else
                    isPathOne = false;
            }

            if (firstCross)
                nextPosition = new Vector3(other.transform.position.x, other.transform.position.y, other.transform.position.z);
        }
    }


    private List<Vertex> dijkstraPath(Vertex enemyV, Vertex playerV)
    {
        Algorithem dijksta = new Algorithem();
        List<Vertex> temp = m_Grid.Generate();
        List<Vertex> list = dijksta.computePaths(temp, enemyV);
        return dijksta.getShortestPath(playerV, list);
    }
}


public class CrossPrevList
{
    public float X { get; set; }
    public float Z { get; set; }
    public string Name { get; set; }

    public CrossPrevList(float x, float z, string name)
    {
        X = x;
        Z = z;
        Name = name;
    }

    // TODO Change to SET !!!
    public static bool checkValidCross(List<CrossPrevList> list, string name)
    {
        if (list.Count == 0)
            return false;
        if(list[list.Count - 1].Name.Equals(name))
            return true;

        return false;
    }

    // TODO remove first X items from list, for memory optimizations.
    public void removeItems(List<CrossPrevList> list, int x) { }


}
