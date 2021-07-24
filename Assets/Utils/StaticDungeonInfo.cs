using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public sealed class StaticDungeonInfo
{
    public EnemyConfiguration[] enemyConfigurations;
    public RoomType[] roomTypes;
}

// Room Types //

[System.Serializable]
public class RoomType
{
    public string name;
    public EnemyProbability[] enemyProbabilities;
}

[System.Serializable]
public class EnemyProbability
{
    public string name;
    public float probability;
}

// Enemy Configurations //

[System.Serializable]
public class EnemyConfiguration
{
    public EnemyPosition[] positions;
}

[System.Serializable]
public class EnemyPosition
{
    public int x;
    public int y;
    public override string ToString() => $"{x}, {y}";

    public Vector2 ToVector() => new Vector2(x, y);
}