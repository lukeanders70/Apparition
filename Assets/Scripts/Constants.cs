using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants
{
    public static readonly Dictionary<string, Vector2> directions = new Dictionary<string, Vector2>() {
        { "up", new Vector2(0, 1) },
        { "down", new Vector2(0, -1) },
        { "left", new Vector2(-1, 0) },
        { "right", new Vector2(1, 0) }
    };
}
