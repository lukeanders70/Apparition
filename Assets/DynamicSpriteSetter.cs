using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicSpriteSetter : MonoBehaviour
{
    [SerializeField]
    private string spritesPath;
    [SerializeField]
    private string fileBase;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private GameObject plugPrefab;

    private Sprite[] sprites = { };

    public void SpawnCallback(RoomGrid roomGrid, (int, int) location)
    {
        sprites = Resources.LoadAll<Sprite>(spritesPath);
        var neighboorOfSameType = new Dictionary<string, bool>()
        {
            { "U", IsSameType(roomGrid, (location.Item1, location.Item2 - 1)) },
            { "R", IsSameType(roomGrid, (location.Item1 + 1, location.Item2)) },
            { "D", IsSameType(roomGrid, (location.Item1, location.Item2 + 1)) },
            { "L", IsSameType(roomGrid, (location.Item1 - 1, location.Item2)) }
        };

        var cornersWithPlugs = new Dictionary<string, bool>()
        {
            { "DL", neighboorOfSameType["D"] && neighboorOfSameType["L"] && !IsSameType(roomGrid, (location.Item1 - 1, location.Item2 + 1)) },
            { "RD", neighboorOfSameType["R"] && neighboorOfSameType["D"] && !IsSameType(roomGrid, (location.Item1 + 1, location.Item2 + 1)) },
            { "UL", neighboorOfSameType["U"] && neighboorOfSameType["L"] && !IsSameType(roomGrid, (location.Item1 - 1, location.Item2 - 1)) },
            { "UR", neighboorOfSameType["U"] && neighboorOfSameType["R"] && !IsSameType(roomGrid, (location.Item1 + 1, location.Item2 -1)) }
        };

        UpdateMainSprite(neighboorOfSameType);
        AddCorners(cornersWithPlugs);
    }

    private void UpdateMainSprite(Dictionary<string, bool> neighboorOfSameType)
    {
        var spriteName = fileBase +
            (neighboorOfSameType["U"] ? "U" : "") +
            (neighboorOfSameType["R"] ? "R" : "") +
            (neighboorOfSameType["D"] ? "D" : "") +
            (neighboorOfSameType["L"] ? "L" : "");

        var sprite = System.Array.Find(sprites, (s) => s.name == spriteName);
        if (sprite != null)
        {
            spriteRenderer.sprite = sprite;
        }
        else
        {
            Debug.LogError("Failed to find sprite under path " + spritesPath + " with name " + spriteName);
        }
    }

    private void AddCorners(Dictionary<string, bool> cornersWithPlugs)
    {
        foreach (KeyValuePair<string, bool> item in cornersWithPlugs) {
            if (item.Value)
            {
                var spriteName = fileBase + "plug_" + item.Key;
                var sprite = System.Array.Find(sprites, (s) => s.name == spriteName);
                if (sprite != null)
                {
                    GameObject plug = Instantiate(
                        plugPrefab,
                        gameObject.transform,
                        true
                    );
                    plug.transform.localPosition = Vector3.zero;
                    plug.GetComponent<SpriteRenderer>().sprite = sprite;
                }
                else
                {
                    Debug.LogError("Failed to find sprite under path " + spritesPath + " with name " + spriteName);
                }
            }
        }
    }

    bool IsSameType(RoomGrid roomGrid, (int, int) index)
    {
        if (!roomGrid.IndexInBounds(index))
        {
            return false;
        }
        GameObject neighboor = roomGrid.getObject(index.Item1, index.Item2);
        var neighboorSpriteSetter = neighboor != null ? neighboor.GetComponent<DynamicSpriteSetter>(): null;
        return neighboorSpriteSetter != null && neighboorSpriteSetter.spritesPath == spritesPath;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
