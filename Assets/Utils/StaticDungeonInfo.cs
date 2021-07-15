using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public sealed class StaticDungeonInfo
{
    public enemyConfiguration[] enemyConfigurations;
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
}

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