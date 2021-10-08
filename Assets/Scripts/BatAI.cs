using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatAI : BasicEnemyAI
{
    public Rigidbody2D rb;
    public float speed = 5.0f;

    private float maxOffsetMagnitude = 5.0f;
    private float oscilationPeriod = 1.0f;

    private float timeCycle = 0;

    private void Start()
    {
        base.Start();
        timeCycle = Random.Range(0, oscilationPeriod);
    }
    // Update is called once per frame
    void Update()
    {
        timeCycle = (timeCycle + Time.deltaTime) % oscilationPeriod;
        var rads = (timeCycle / oscilationPeriod) * (2 * Mathf.PI);
        var offset = maxOffsetMagnitude * Mathf.Sin(rads);
        var closestPlayer = AIHelpers.GetClosestMovingPlayer(transform.position);
        if(closestPlayer != null)
        {
            var playerDir = ((Vector2)closestPlayer.transform.position - rb.position);
            var perpDir = Vector2.Perpendicular(playerDir).normalized;
            var targetPoint = (Vector2)closestPlayer.transform.position + (perpDir * offset);

            var dir = (targetPoint - rb.position).normalized;
            rb.velocity = (dir * speed);
        } else
        {
            rb.velocity = Vector2.zero;
        }
    }
}
