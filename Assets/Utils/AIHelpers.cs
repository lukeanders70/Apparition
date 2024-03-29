using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

# nullable enable
public class AIHelpers
{
    public static List<IntVector2> cardinalDirections = new List<IntVector2> { new IntVector2(1, 0), new IntVector2(-1, 0), new IntVector2(0, 1), new IntVector2(0, -1) };
    public static List<IntVector2> angledDirections = new List<IntVector2> { new IntVector2(1, 1), new IntVector2(-1, 1), new IntVector2(1, -1), new IntVector2(-1, -1) };
    public static Vector3 RandomDirection()
    {
        Vector3 dir = new Vector3(UnityEngine.Random.Range(-1.0f, 1.0f), UnityEngine.Random.Range(-1.0f, 1.0f));
        dir.Normalize();
        return dir;
    }

    public static List<T> ChooseN<T>(int n, List<T> l)
    {
        var newList = new List<T>();
        while(n > 0 && l.Count > 0)
        {
            var index = UnityEngine.Random.Range(0, l.Count);
            newList.Add(l[index]);
            l.RemoveAt(index);
            n -= 1;
        }
        return newList;
    }

    public static GameObject? GetSpiritDestinationPlayer()
    {
        GameObject spirit = GameObject.Find("Spirit");
        if(spirit)
        {
            SpiritController controller = spirit.GetComponent<SpiritController>();
            if(controller)
            {
                return controller.futureParent;
            }
        }
        return null;
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

    public static Vector3? GetClosestEmpty(List<Vector3> directionsToCheck, Vector3 closePosition, Vector3 currentPosition)
    {
        Vector3? bestSolution = null;
        var closestPosition = -1.0f;
        foreach(Vector3 direction in directionsToCheck)
        {
            var positionToCheck = closePosition + direction;
            var distanceFromOptimal = Vector3.Distance(positionToCheck, currentPosition);
            var isClosest = (closestPosition == -1.0f || distanceFromOptimal < closestPosition);
            var isEmpty = true;
            foreach (Collider c in Physics.OverlapSphere(positionToCheck, 0.2f))
            {
                if(c.gameObject.tag == "Wall")
                {
                    isEmpty = false;
                    break;
                }
            }

            if (isClosest && isEmpty)
            {
                bestSolution = positionToCheck;
                closestPosition = distanceFromOptimal;
            }
        }
        return bestSolution;
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

    public static Vector2? GetClosestPlayerDirection(Vector3 position)
    {
        var player = GetClosestPlayer(position);
        if (player != null)
        {
            return (player.transform.position - position).normalized;
        }
        else
        {
            return null;
        }
    }

    public static GameObject? GetClosestMovingPlayer(Vector3 position)
    {
        GameObject player1 = GameObject.Find("Player1");
        GameObject player2 = GameObject.Find("Player2");
        if(player1 == null || player2 == null)
        {
            return null;
        }
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

    public static GameObject? GetInactivePlayer()
    {
        return GameObject.Find("Player").GetComponent<PlayerHandler>().GetInActivePlayer(); ;
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
}

[Serializable]
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

    public static Vector2 Add(Vector2 v1, IntVector2 v2)
    {
        return new Vector2(v1.x + v2.x, v1.y + v2.y);
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
