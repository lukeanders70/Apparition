using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public sealed class StaticDungeonInfo
{
    public enemyConfiguration[] enemyConfigurations;
    public RoomType[] roomTypes;
    public Vector2 getEnemyPosition(int numEnemies, int enemyIndex)
    {
        if (numEnemies - 1 < enemyConfigurations.Length)
        {
            if (enemyIndex < enemyConfigurations[numEnemies - 1].positions.Length)
            {
                return enemyConfigurations[numEnemies - 1].positions[enemyIndex].ToVector();
            }
        }
        return new Vector2(Random.Range(-2f, 2f), Random.Range(-2f, 2f));
    }

    public EnemyProbability[] GetEnemyProbabilities(string roomTypeName)
    {
        foreach (RoomType roomType in roomTypes)
        {
            if(roomType.name == roomTypeName)
            {
                return roomType.enemyProbabilities;
            }
        }
        return new EnemyProbability[] { };
    }
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
public class enemyConfiguration
{
    public enemyPosition[] positions;
}

[System.Serializable]
public class enemyPosition
{
    public int x;
    public int y;
    public override string ToString() => $"{x}, {y}";

    public Vector2 ToVector() => new Vector2(x, y);
}