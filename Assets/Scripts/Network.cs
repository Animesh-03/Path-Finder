using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Network : MonoBehaviour
{
    public static Network Instance;
    public List<List<int>> graph;


    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void MakeGraph()
    {
        graph = new List<List<int>>();
        List<Node> nodes = new List<Node>(GameObject.FindObjectsOfType<Node>());
        Debug.Log(nodes.Count);
        for(int i = 0; i < nodes.Count; i++)
        {
            graph.Add(new List<int>(new int[nodes.Count]));
            foreach(Edge e in nodes[i].GetEdges())
            {
                graph[i][nodes.IndexOf(e.GetEndNode())] = e.GetWeight();
            }
        }

        Debug.Log(graph);


    }

}
