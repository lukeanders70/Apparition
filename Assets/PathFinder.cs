using CollectionTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    private static List<IntVector2> cardinalDirections = new List<IntVector2> { new IntVector2(1, 0), new IntVector2(-1, 0), new IntVector2(0, 1), new IntVector2(0, -1) };
    private static List<IntVector2> angledDirections = new List<IntVector2> { new IntVector2(1, 1), new IntVector2(-1, 1), new IntVector2(1, -1), new IntVector2(-1, -1) };

    public BoxCollider2D colliderComp;

    public GameObject room;
    public RoomGrid roomGrid;

    private Dictionary<IntVector2, GraphNode> graph;
    private HashSet<GraphNode> visited;
    private PriorityQueue<PathNode> fringe;
    private PathNode considered;
    private List<IntVector2> lastPath;
    void Start()
    {
        room = GetComponentInParent<RoomController>().gameObject;
        roomGrid = GetComponentInParent<RoomController>().roomGrid;
        if (roomGrid == null)
            Debug.LogError("could not find enemy room grid for Enemy");
        if (colliderComp == null)
            Debug.LogError("could not find collider for Enemy");
        if (room == null)
            Debug.LogError("could not find room for Enemy");

        graph = buildGraph();
    }


    private static float HeuristicDistance(IntVector2 index, IntVector2 desiredIndex, float distanceSoFar)
    {
        var xDis = Mathf.Abs(index.x - desiredIndex.x);
        var yDis = Mathf.Abs(index.y - desiredIndex.y);
        // first term is if we only travel on diagonal (optimal), second term is remaining that we have to travel in a straight line
        return (Mathf.Min(xDis, yDis) * (float) Math.Sqrt(2)) + (Mathf.Abs(xDis - yDis)) + distanceSoFar;
    }

    public List<IntVector2> Pathfind(GameObject room, GameObject o, Vector2 desiredPosition)
    {
        var endingNode = findClosestGraphNode(desiredPosition - (Vector2)room.transform.position);
        if (endingNode == null)
        {
            Debug.LogError("desired ending position not in graph");
            return new List<IntVector2>();
        }

        var startingNode = findClosestGraphNode(o.transform.localPosition);
        if (startingNode == null)
        {
            Debug.LogError("desired starting position not in graph");
            return new List<IntVector2>();
        }

        visited = new HashSet<GraphNode>();
        fringe = new PriorityQueue<PathNode>(new PathNodeComparer());
        var fringeSet = new HashSet<GraphNode>();

        fringe.Enqueue(new PathNode(startingNode, 0, float.PositiveInfinity));
        fringeSet.Add(startingNode);

        while (fringe.Count() > 0)
        {
            do
            {
                considered = fringe.Dequeue();
            } while (visited.Contains(considered.graphNode));

            visited.Add(considered.graphNode);

            if (considered != null)
            {
                if (considered.graphNode.Equals(endingNode))
                {
                    lastPath = considered.TraceBackPath();
                    return lastPath;
                }
                else
                {
                    foreach((float edgeDist, GraphNode node) edge in considered.graphNode.neighboors)
                    {
                        if(!visited.Contains(edge.node) && !fringeSet.Contains(edge.node))
                        {
                            fringe.Enqueue(new PathNode(
                                edge.node,
                                considered.distance + edge.edgeDist,
                                HeuristicDistance(edge.node.index, endingNode.index, considered.distance + edge.edgeDist),
                                considered
                            ));
                            fringeSet.Add(edge.node);
                        }
                    }
                }
            }
        }
        lastPath = new List<IntVector2>();
        return lastPath;
    }

    public static K MinPosition<T, K>(Dictionary<K, T> l, Func<T, float> mapper)
    {
        var minValue = float.PositiveInfinity;
        K minKey = default(K);

        foreach (KeyValuePair<K, T> pair in l)
        {
            var value = mapper(pair.Value);
            if (value < minValue)
            {
                minValue = value;
                minKey = pair.Key;
            }
        }
        return minKey;
    }

    public IEnumerator MoveAlongSpline(Rigidbody2D rb, Vector2 roomOffset, List<IntVector2> spline, float speed, Action callback)
    {
        foreach (IntVector2 gridPosition in spline)
        {
            var desiredPosition = (RoomGrid.GetLocationFromCell(gridPosition) + roomOffset - colliderComp.offset);
            Vector2 initialDirection = (desiredPosition - rb.position).normalized;

            while (Vector2.Distance(rb.position, desiredPosition) > 0.05)
            {
                Vector3 dir = (desiredPosition - rb.position).normalized;
                if (Vector2.Angle(initialDirection, dir) > 90) // we've gone past the point
                {
                    break;
                }
                rb.velocity = (dir * speed);
                yield return null;
            }
            rb.position = desiredPosition;
        }
        rb.velocity = Vector3.zero;
        if (callback != null)
        {
            callback();
        }
        yield break;
    }

    private GraphNode findClosestGraphNode(Vector2 position)
    {
        var index = roomGrid.GetCellFromLocation(position);
        if (graph.ContainsKey(index)){
            return graph[index];
        }
        foreach(IntVector2 dir in cardinalDirections)
        {
            var testIndex = IntVector2.Add(index, dir);
            if (graph.ContainsKey(testIndex))
            {
                return graph[testIndex];
            }
        }
        foreach (IntVector2 dir in angledDirections)
        {
            var testIndex = IntVector2.Add(index, dir);
            if (graph.ContainsKey(testIndex))
            {
                return graph[testIndex];
            }
        }
        return null;
    }

    private Dictionary<IntVector2, GraphNode> buildGraph()
    {
        var nodes = new Dictionary<IntVector2, GraphNode>();
        for (int xIndex = 0; xIndex < RoomGrid.dimensions.x; xIndex++)
        {
            for (int yIndex = 0; yIndex < RoomGrid.dimensions.y; yIndex++)
            {
                var index = new IntVector2(xIndex, yIndex);
                if (roomGrid.isEmptyCenter(index, colliderComp.size, true))
                {
                    var worldPosition = RoomGrid.GetLocationFromCell(index) + (Vector2)room.transform.position;
                    nodes[index] = new GraphNode(index, worldPosition);
                }
            }
        }

        foreach (KeyValuePair<IntVector2, GraphNode> node in nodes)
        {
            foreach(IntVector2 dir in cardinalDirections)
            {
                var checkIndex = IntVector2.Add(node.Key, dir);
                if (nodes.ContainsKey(checkIndex))
                {
                    node.Value.neighboors.Add((edgeDist: 1, node: nodes[checkIndex]));
                }
            }
            foreach (IntVector2 dir in angledDirections)
            {
                var checkIndex = IntVector2.Add(node.Key, dir);
                var checkIndexVertical = IntVector2.Add(node.Key, new IntVector2(0, dir.y));
                var checkIndexHorizontal = IntVector2.Add(node.Key, new IntVector2(dir.x, 0));
                if (nodes.ContainsKey(checkIndex) && nodes.ContainsKey(checkIndexVertical) && nodes.ContainsKey(checkIndexHorizontal))
                {
                    node.Value.neighboors.Add((edgeDist: Mathf.Sqrt(2), node: nodes[checkIndex]));
                }
            }
        }
        return nodes;
    }
    void OnDrawGizmos()
    {
        if(graph != null)
        {
            foreach (KeyValuePair<IntVector2, GraphNode> pair in graph)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawCube(RoomGrid.GetLocationFromCell(pair.Key) + (Vector2)room.transform.position, new Vector3(0.7f, 0.3f, 0.3f));
            }
        }
        if (visited != null)
        {
            foreach (GraphNode node in visited)
            {
                Gizmos.color = Color.gray;
                Gizmos.DrawCube(RoomGrid.GetLocationFromCell(node.index) + (Vector2)room.transform.position, new Vector3(0.5f, 0.3f, 0.3f));
            }
        }
        if (fringe != null)
        {
            foreach (PathNode node in fringe.getList())
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawCube(RoomGrid.GetLocationFromCell(node.graphNode.index) + (Vector2)room.transform.position, new Vector3(0.3f, 0.3f, 0.3f));
            }
        }
        if (considered != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(RoomGrid.GetLocationFromCell(considered.graphNode.index) + (Vector2)room.transform.position, new Vector3(0.1f, 0.3f, 0.3f));
        }
        if (lastPath != null)
        {
            foreach(IntVector2 index in lastPath)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(RoomGrid.GetLocationFromCell(index) + (Vector2)room.transform.position, 0.2f);
            }
        }
    }
}

public class GraphNode
{
    public List<(float edgeDist, GraphNode node)> neighboors;
    public IntVector2 index;
    public Vector2 worldPosition;

    public GraphNode(IntVector2 i, Vector2 wp)
    {
        index = i;
        worldPosition = wp;
        neighboors = new List<(float edgeDist, GraphNode node)>();
    }

    public bool Equals(GraphNode other)
    {
        return (other.index == index);
    }

    public override int GetHashCode()
    {
        return index.GetHashCode();
    }
}

public class PathNode
{
    public GraphNode graphNode;
    public PathNode previous;
    public float distance;
    public float heuristic;
    public PathNode(GraphNode gn, float d, float h, PathNode prev = null)
    {
        graphNode = gn;
        distance = d;
        previous = prev;
        heuristic = h;
    }

    public List<IntVector2> TraceBackPath()
    {
        var path = new List<IntVector2>();
        PathNode currentNode = this;
        while (currentNode != null)
        {
            path.Insert(0, currentNode.graphNode.index);
            currentNode = currentNode.previous;
        }
        return path;
    }

    public override string ToString()
    {
        return heuristic.ToString("0.00");
    }
}

public class PathNodeComparer : IComparer<PathNode>
{
    public int Compare(PathNode x, PathNode y)
    {
        if(x.heuristic < y.heuristic)
        {
            return -1;
        } else if(x.heuristic > y.heuristic)
        {
            return 1;
        }
        return 0;
    }
} 
