using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int width = 8;
    public int height = 8;
    public float cellSize = 1f;
    public GameObject tilePrefab;

    private GameObject[,] tiles;

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        tiles = new GameObject[width, height];

        float offsetX = (width - 1) * cellSize / 2f;
        float offsetY = (height - 1) * cellSize / 2f;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 pos = new Vector3(x * cellSize - offsetX, y * cellSize - offsetY, 0);
                GameObject tile = Instantiate(tilePrefab, pos, Quaternion.identity, transform);
                tile.name = $"Tile {x},{y}";

                SpriteRenderer sr = tile.GetComponent<SpriteRenderer>();
                if (sr != null)
                    sr.color = ((x + y) % 2 == 0) ? Color.white : Color.gray;

                Tiles tileScript = tile.GetComponent<Tiles>();
                if (tileScript != null)
                    tileScript.Init(x, y);

                tiles[x, y] = tile;
            }
        }
    }
}
