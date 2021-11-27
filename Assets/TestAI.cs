using System.Collections.Generic;
using UnityEngine;

public class TestAI : MonoBehaviour
{
    public Rigidbody2D rb;
    public HashSet<IntVector2> visited = new HashSet<IntVector2> { };
    public Dictionary<IntVector2, (float H, PathNode node)> fringe = new Dictionary<IntVector2, (float H, PathNode node)> { };
    public IntVector2 active = new IntVector2(0,0);
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var rg = GetComponentInParent<RoomController>().gameObject;
        var path = AIHelpers.Pathfind(rg, gameObject, new Vector2(11.5f, -5.5f));
        StartCoroutine(AIHelpers.MoveAlongSpline(rb, Vector2.zero, Vector2.zero, path, 1.0f, null));
    }

    void OnDrawGizmos()
    {
        foreach (IntVector2 v in visited)
        {
            Gizmos.color = Color.gray;
            Gizmos.DrawCube(RoomGrid.GetLocationFromCell(v), new Vector3(0.5f, 0.3f, 0.3f));
        }
        foreach (KeyValuePair<IntVector2, (float, PathNode)> pair in fringe)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawCube(RoomGrid.GetLocationFromCell(pair.Value.Item2.index), new Vector3(0.2f, 0.3f, 0.3f));
        }
        Gizmos.color = Color.red;
        Gizmos.DrawCube(RoomGrid.GetLocationFromCell(active), new Vector3(0.1f, 0.3f, 0.3f));
    }
}
