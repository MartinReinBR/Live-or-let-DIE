using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Pathfinding : MonoBehaviour
{
    private static GameObject[] groundBlocks;

    public GameObject player;

    private class Node
    {
        public Vector3 position { get; }
        public float f { get; set; }
        public float g { get; set; }
        public float h { get; set; }
        public Node parent { get; }

        public Node(Vector3 position, Node parent = null)
        {
            this.position = position;
            this.f = 0;
            this.g = 0;
            this.h = 0;
            this.parent = parent;
        }
    }

    class NodeEqualityComparer : IEqualityComparer<Node>
    {
        public bool Equals(Node a, Node b)
        {
            return Vector3.Distance(a.position, b.position) < 0.01f;
        }
        public int GetHashCode(Node node)
        {
            return node.position.GetHashCode();
        }
    }

    void Start()
    {
        UpdateBlocks();
    }


    public static bool IsGround(Vector3 position)
    {
        foreach(GameObject go in groundBlocks)
        {
            if(Vector3.Distance(go.transform.position, position) < 0.01f)
            {
                return true;
            }
        }
        return false;
    }

    public static void UpdateBlocks()
    {
        groundBlocks = GameObject.FindGameObjectsWithTag("Ground");
    }

    public List<Vector3> AStar(Vector3 origin, Vector3 target)
    {
        //flatten
        origin.y = 0;
        target.y = 0;

        NodeEqualityComparer nodeEq = new NodeEqualityComparer();
        HashSet<Node> open = new HashSet<Node>(nodeEq);
        HashSet<Node> closed = new HashSet<Node>(nodeEq);

        open.Add(new Node(origin));

        while(open.Count != 0)
        {
            Node q = GetSmallestF(open);
            open.Remove(q);

            List<Node> neighbours = GenerateNeighbours(q);
            foreach (Node node in neighbours)
            {
                if (Vector3.Distance(node.position, target) < 0.1f)
                {
                    // stop search
                    return TracePath(node);
                }
                node.g = q.g + 1;
                node.h = Vector3.Distance(node.position, target);
                node.f = node.g + node.h;

                if(CheckList(open, node))
                {
                    // skip this
                    continue;
                }
                if(CheckList(closed, node))
                {
                    continue;
                }
                else
                {
                    open.Add(node);
                }

            }

            closed.Add(q);
        }
        return null;

    }

    List<Node> GenerateNeighbours(Node original)
    {
        List<Node> neighbours = new List<Node>();

        Vector3 pos = original.position + new Vector3(1, 0, 0);
        if (IsGround(pos))
            neighbours.Add(new Node(pos, original));
        pos = original.position + new Vector3(0, 0, 1);
        if (IsGround(pos))
            neighbours.Add(new Node(pos, original));
        pos = original.position + new Vector3(-1, 0, 0);
        if (IsGround(pos))
            neighbours.Add(new Node(pos, original));
        pos = original.position + new Vector3(0, 0, -1);
        if (IsGround(pos))
            neighbours.Add(new Node(pos, original));

        return neighbours;
    }

    bool CheckList(HashSet<Node> set, Node node)
    {
        foreach (Node openNode in set)
        {
            if (openNode.position == node.position && openNode.f < node.f)
                return true;
        }
        return false;
    }

    Node GetSmallestF(HashSet<Node> set)
    {
        Node smallestNode = null;
        float smallestF = float.MaxValue;
        foreach(Node node in set)
        {
            if(node.f < smallestF)
            {
                smallestNode = node;
                smallestF = node.f;
            }
        }
        return smallestNode;
    }

    List<Vector3> TracePath(Node destination)
    {
        List<Vector3> path = new List<Vector3>();
        path.Add(destination.position);
        Node n = destination.parent;
        while (n != null)
        {
            path.Add(n.position);
            n = n.parent;
        }

        path.Reverse(0, path.Count);

        return path;
    }
}
