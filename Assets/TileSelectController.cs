using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSelectController : MonoBehaviour
{
    [SerializeField]
    private GameObject TilePrefab;
    [SerializeField]
    private float MinHeight = 274;

    private GameObject[] ObsticlePrefabs;
    private GameObject[] EnemyPrefabs;
    private RectTransform rectTrans;
    private float TilePrefabHeight;

    private int numTiles = 0;

    // Start is called before the first frame update
    void Start()
    {
        ObsticlePrefabs = Resources.LoadAll<GameObject>("prefabs/Obstacles");
        EnemyPrefabs = Resources.LoadAll<GameObject>("prefabs/Enemies");
        rectTrans = gameObject.GetComponent<RectTransform>();
        rectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, MinHeight);

        RectTransform tilePrefabRectTrans = TilePrefab.GetComponent<RectTransform>();
        TilePrefabHeight = (tilePrefabRectTrans.sizeDelta.y * tilePrefabRectTrans.localScale.y);

        foreach (GameObject prefab in ObsticlePrefabs)
        {
            AddTile(GridCell.CellObjectType.obstacle, prefab);
        }
        foreach (GameObject prefab in EnemyPrefabs)
        {
            AddTile(GridCell.CellObjectType.enemy, prefab);
        }
    }

    void AddTile(GridCell.CellObjectType type, GameObject prefab)
    {
        var newTile = Instantiate(TilePrefab, gameObject.transform);
        newTile.GetComponentInChildren<RectTransform>().anchoredPosition = new Vector2(0, (-0.5f * TilePrefabHeight) - (numTiles * TilePrefabHeight));
        rectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Mathf.Max(MinHeight, (numTiles * TilePrefabHeight)));

        newTile.GetComponent<TileButtonController>().Setup(type, prefab);
        numTiles += 1;
    }
}
