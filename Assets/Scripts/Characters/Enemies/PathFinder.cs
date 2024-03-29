using CollectionTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{

    public BoxCollider2D colliderComp;

    public GameObject room;
    public RoomGrid roomGrid;

    private Dictionary<IntVector2, GraphNode> graph;
    private HashSet<GraphNode> visited;
    private PriorityQueue<PathNode> fringe;
    private PathNode considered;
    private List<Vector2> lastPath;

    private float graphXOffset;
    private float graphYOffset;

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

        // if height is two, node points should halfway vetween room grid cells so that we
        // can fit through gaps that are two tall.
        graphXOffset = Mathf.CeilToInt(colliderComp.size.x) % 2 == 0 ? 0.0f : 0.5f;
        graphYOffset = Mathf.CeilToInt(colliderComp.size.y) % 2 == 0 ? 0.0f : 0.5f;

        graph = buildGraph();
    }


    private static float HeuristicDistance(IntVector2 index, IntVector2 desiredIndex, float distanceSoFar)
    {
        var xDis = Mathf.Abs(index.x - desiredIndex.x);
        var yDis = Mathf.Abs(index.y - desiredIndex.y);
        // first term is if we only travel on diagonal (optimal), second term is remaining that we have to travel in a straight line
        return (Mathf.Min(xDis, yDis) * (float) Math.Sqrt(2)) + (Mathf.Abs(xDis - yDis)) + distanceSoFar;
    }

    public List<Vector2> Pathfind(GameObject room, GameObject o, Vector2 desiredPosition)
    {
        var endingNode = findClosestGraphNode(desiredPosition - (Vector2)room.transform.position);
        if (endingNode == null)
        {
            Debug.LogError("desired ending position not in graph: " + desiredPosition);
            return new List<Vector2>();
        }

        var startingNode = findClosestGraphNode((Vector2)o.transform.localPosition + colliderComp.offset);
        if (startingNode == null)
        {
            Debug.LogError("desired starting position not in graph");
            return new List<Vector2>();
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
        lastPath = new List<Vector2>();
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

    public IEnumerator MoveAlongSpline(Rigidbody2D rb, Vector2 roomOffset, List<Vector2> spline, float speed, Action callback)
    {
        foreach (Vector2 worldPosition in spline)
        {
            var desiredPosition = (worldPosition - colliderComp.offset);
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

    public List<Vector2> WanderFind(GameObject o, int minDistance, int maxDistance)
    {
        var path = new List<Vector2>();
        int desiredDistance = UnityEngine.Random.Range(minDistance, maxDistance + 1);
        HashSet<GraphNode> explored = new HashSet<GraphNode>();

        var nextNode = findClosestGraphNode(o.transform.localPosition);
        if (nextNode == null)
        {
            Debug.LogError("desired starting position not in graph");
            return new List<Vector2>();
        }
        while(desiredDistance > 0)
        {
            explored.Add(nextNode);
            path.Add(nextNode.worldPosition);
            desiredDistance -= 1;

            var travelableNeighboors = new List<GraphNode>();
            foreach ((float edgeDist, GraphNode node) n in nextNode.neighboors)
            {
                if (!explored.Contains(n.node))
                {
                    travelableNeighboors.Add(n.node);
                }
            }
            if(travelableNeighboors.Count == 0)
            {
                return path;
            }
            nextNode = travelableNeighboors[UnityEngine.Random.Range(0, travelableNeighboors.Count)];
            foreach (GraphNode neighboor in travelableNeighboors)
            {
                explored.Add(neighboor);
            }
        }
        return path;
    }

    private GraphNode findClosestGraphNode(Vector2 position)
    {
        var index = roomGrid.GetCellFromLocation(position + new Vector2(graphXOffset - 0.5f, graphYOffset -0.5f));
        if (graph.ContainsKey(index)){
            return graph[index];
        }
        foreach(IntVector2 dir in AIHelpers.cardinalDirections)
        {
            var testIndex = IntVector2.Add(index, dir);
            if (graph.ContainsKey(testIndex))
            {
                return graph[testIndex];
            }
        }
        foreach (IntVector2 dir in AIHelpers.angledDirections)
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
                var position = new Vector2(xIndex + graphXOffset, yIndex + graphYOffset);
                var index = new IntVector2(xIndex, yIndex);
                if (roomGrid.isPointEmptyCenter(position, colliderComp.size, true))
                {
                    var worldPosition = RoomGrid.GetLocationFromPoint(position) + (Vector2)room.transform.position;
                    nodes[index] = new GraphNode(index, worldPosition);
                }
            }
        }

        foreach (KeyValuePair<IntVector2, GraphNode> node in nodes)
        {
            foreach(IntVector2 dir in AIHelpers.cardinalDirections)
            {
                var checkIndex = IntVector2.Add(node.Key, dir);
                if (nodes.ContainsKey(checkIndex))
                {
                    node.Value.neighboors.Add((edgeDist: 1, node: nodes[checkIndex]));
                }
            }
            foreach (IntVector2 dir in AIHelpers.angledDirections)
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
                Gizmos.DrawCube(pair.Value.worldPosition, new Vector3(0.7f, 0.3f, 0.3f));
            }
        }
        if (visited != null)
        {
            foreach (GraphNode node in visited)
            {
                Gizmos.color = Color.gray;
                Gizmos.DrawCube(node.worldPosition, new Vector3(0.5f, 0.3f, 0.3f));
            }
        }
        if (fringe != null)
        {
            foreach (PathNode node in fringe.getList())
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawCube(node.graphNode.worldPosition, new Vector3(0.3f, 0.3f, 0.3f));
            }
        }
        if (considered != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(considered.graphNode.worldPosition, new Vector3(0.1f, 0.3f, 0.3f));
        }
        if (lastPath != null)
        {
            foreach(Vector2 worldPosition in lastPath)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(worldPosition, 0.2f);
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

    public List<Vector2> TraceBackPath()
    {
        var path = new List<Vector2>();
        PathNode currentNode = this;
        while (currentNode != null)
        {
            path.Insert(0, currentNode.graphNode.worldPosition);
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
