using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

# nullable enable
public class AIHelpers
{
    private static List<IntVector2> cardinalDirections = new List<IntVector2> { new IntVector2(1, 0), new IntVector2(-1, 0), new IntVector2(0, 1), new IntVector2(0, -1) };
    private static List<IntVector2> angledDirections = new List<IntVector2> { new IntVector2(1, 1), new IntVector2(-1, 1), new IntVector2(1, -1), new IntVector2(-1, -1) };
    public static Vector3 RandomDirection()
    {
        Vector3 dir = new Vector3(UnityEngine.Random.Range(-1.0f, 1.0f), UnityEngine.Random.Range(-1.0f, 1.0f));
        dir.Normalize();
        return dir;
    }

    public static GameObject? GetClosestPlayer(Vector3 position)
    {
        GameObject player1 = GameObject.Find("Player1");
        GameObject player2 = GameObject.Find("Player2");

        if (player1 != null && player2 != null)
        {
            if (Vector3.Distance(player1.transform.position, position) < Vector3.Distance(player2.transform.position, position))
            {
                return player1;
            }
            else
            {
                return player2;
            }
        }
        return null;
    }

    public static float? ClosestPlayerDistance(Vector3 position)
    {
        var player = GetClosestPlayer(position);
        if(player != null)
        {
            return Vector2.Distance(position, player.transform.position);
        } else
        {
            return null;
        }
    }

    public static GameObject? GetClosestMovingPlayer(Vector3 position)
    {
        GameObject player1 = GameObject.Find("Player1");
        GameObject player2 = GameObject.Find("Player2");
        bool player1Moving = player1.GetComponent<Rigidbody2D>().velocity.magnitude > 0.1f;
        bool player2Moving = player2.GetComponent<Rigidbody2D>().velocity.magnitude > 0.1f;

        if (player1 != null && player2 != null)
        {
            var player1Closer = Vector3.Distance(player1.transform.position, position) < Vector3.Distance(player2.transform.position, position);
            if (player1Moving && ((!player2Moving) || player1Closer))
            {
                return player1;
            }
            else if (player2Moving && ((!player1Moving) || !player1Closer))
            {
                return player2;
            }
        }
        return null;
    }

    public static IEnumerator MoveTo(Rigidbody2D rb, Vector3 position, float speed, Action callback)
    {
        while(Vector2.Distance(rb.position, position) > 0.05)
        {
            Vector3 dir = ((Vector2)position - rb.position).normalized;
            rb.velocity = (dir * speed);
            yield return null;
        }
        rb.velocity = Vector3.zero;
        if (callback != null)
        {
            callback();
        }
        yield break;
    }

    public static IEnumerator MoveAlongSpline(Rigidbody2D rb, Vector2 colliderOfset, Vector2 roomOffset, List<IntVector2> spline, float speed, Action callback)
    {
        foreach (IntVector2 gridPosition in spline)
        {
            var position = (RoomGrid.GetLocationFromCell(gridPosition) + roomOffset) - colliderOfset;
            Debug.Log("Going Towards: " + position);
            while (Vector2.Distance(rb.position, position) > 0.05)
            {
                Vector3 dir = (position - rb.position).normalized;
                rb.velocity = (dir * speed);
                yield return null;
            }
            rb.position = position;
        }
        Debug.Log("Ended Spline Move");
        rb.velocity = Vector3.zero;
        if (callback != null)
        {
            callback();
        }
        yield break;
    }

    private static float HeuristicDistance(IntVector2 index, IntVector2 desiredIndex, float distanceSoFar)
    {
        //return Vector2.Distance(new Vector2(index.x, index.y), new Vector2(desiredIndex.x, desiredIndex.y)) + distanceSoFar;
        var xDis = Math.Abs(index.x - desiredIndex.x);
        var yDis = Math.Abs(index.y - desiredIndex.y);
        // first term is if we only travel on diagonal (optimal), second term is remaining that we have to travel in a straight line
        return (Math.Min(xDis, yDis) * 1.414f) + (Math.Abs(xDis - yDis)) + distanceSoFar;
    }

    private static void AddFringIfLower(Dictionary<IntVector2, (float H, PathNode node)> fringe, PathNode newNode, float newDistance)
    {
        if(!fringe.ContainsKey(newNode.index) )
        {
            fringe.Add(newNode.index, (newDistance, newNode));
        } else if (fringe[newNode.index].H > newDistance)
        {
            fringe[newNode.index] = (newDistance, newNode);
        }
    }

    private static void AddToFringe(Dictionary<IntVector2, (float H, PathNode node)> fringe, HashSet<IntVector2> visited, RoomGrid grid, PathNode node, IntVector2 endingIndex)
    {
        foreach (var dir in cardinalDirections)
        {
            var position = IntVector2.Add(node.index, dir);
            var canMove = grid.isEmpty(position, new IntVector2(1, 1));
            var beenVisted = visited.Contains(position);
            if (canMove && !beenVisted)
            {
                var newNode = new PathNode(position, node.distance + 1, node);
                AddFringIfLower(
                    fringe,
                    newNode,
                    HeuristicDistance(position, endingIndex, newNode.distance)
                );
            }
        }
        foreach (var dir in angledDirections)
        {
            var position = IntVector2.Add(node.index, dir);
            var beenVisted = visited.Contains(position);
            if (!beenVisted)
            {
                var positionVertical = IntVector2.Add(node.index, new IntVector2(0, dir.y));
                var positionHorizontal = IntVector2.Add(node.index, new IntVector2(dir.x, 0));
                var canMove = grid.isEmpty(position, null) && grid.isEmpty(positionHorizontal, null) && grid.isEmpty(positionVertical, null);
                if (canMove)
                {
                    var newNode = new PathNode(position, node.distance + 1.414f, node);
                    AddFringIfLower(
                        fringe,
                        newNode,
                        HeuristicDistance(position, endingIndex, newNode.distance)
                    );
                }
            }
        }
        //testAI.fringe = fringe;
    }

    public static List<IntVector2> Pathfind(GameObject room, GameObject o, Vector2 desiredPosition)
    {
        var grid = room.GetComponent<RoomController>().roomGrid;
        var endingIndex = grid.GetCellFromLocation(desiredPosition - (Vector2)room.transform.position);

        BoxCollider2D oCollider;
        bool hasCollider = o.TryGetComponent(out oCollider);
        if (!hasCollider)
        {
            return new List<IntVector2> { endingIndex };
        }

        var startingIndex = grid.GetCellFromLocation((Vector2)o.transform.localPosition + oCollider.offset);
        Debug.Log("Starting Index: " + startingIndex);
        Debug.Log("Ending Index: " + endingIndex);

        var visited = new HashSet<IntVector2>();
        var fringe = new Dictionary<IntVector2, (float H, PathNode node)>();
        fringe.Add(startingIndex, (HeuristicDistance(startingIndex, endingIndex, 0), new PathNode(startingIndex, 0, null)));

        var runs = 0;
        while(fringe.Count > 0)
        {
            //yield return new WaitForSeconds(1.0f);
            PathNode considered;
            float H;
            do
            {
                var consideredFringeKey = MinPosition(fringe, (pair) => pair.Item1);
                if(consideredFringeKey == null)
                {
                    new List<IntVector2>();
                }
                considered = fringe[consideredFringeKey].Item2;
                H = fringe[consideredFringeKey].Item1;
                fringe.Remove(consideredFringeKey);
            } while (visited.Contains(considered.index));

            visited.Add(considered.index);
            //testAI.active = considered.index;
            //testAI.visited = visited;
            Debug.Log("Considered: " + considered.index + " with H = " + H);
            

            if (considered != null)
            {
                if(considered.index.Equals(endingIndex))
                {
                    return considered.TraceBackPath();
                } else
                {
                    AddToFringe(fringe, visited, grid, considered, endingIndex);
                }
            }
            runs += 1;
        }

        return new List<IntVector2>();
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
}

public class IntVector2 : IEquatable<IntVector2>
{
    public int x;
    public int y;
    public IntVector2(int xVal, int yVal)
    {
        x = xVal;
        y = yVal;
    }

    public override string ToString()
    {
        return "(" + x + ", " + y + ")";
    }

    public Vector2 ToVector2()
    {
        return new Vector2(x, y);
    }

    public static IntVector2 Add(IntVector2 v1, IntVector2 v2)
    {
        return new IntVector2(v1.x + v2.x, v1.y + v2.y);
    }

    public bool Equals(IntVector2 other)
    {
        return (other.x == x) && (other.y == y);
    }

    public override int GetHashCode()
    {
        return 31 * x + 17 * y;
    }
}

public class PathNode
{
    public IntVector2 index;
    public PathNode? previous;
    public float distance;
    public PathNode(IntVector2 i, float d, PathNode? prev)
    {
        index = i;
        distance = d;
        previous = prev;
    }

    public List<IntVector2> TraceBackPath()
    {
        var path = new List<IntVector2>();
        PathNode? currentNode = this;
        while(currentNode != null)
        {
            path.Insert(0, currentNode.index);
            currentNode = currentNode.previous;
        }
        return path;
    }
}
