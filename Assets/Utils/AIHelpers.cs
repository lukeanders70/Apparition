using System.Collections;
using System.Collections.Generic;
using UnityEngine;

# nullable enable
public class AIHelpers
{
    public static Vector3 RandomDirection()
    {
        Vector3 dir = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
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

    public static IEnumerator MoveTo(Rigidbody2D rb, Vector3 position, float speed, System.Action callback)
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
