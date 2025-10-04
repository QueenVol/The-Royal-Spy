using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int width = 8;
    public int height = 8;
    public float cellSize = 1f;
    public GameObject tilePrefab;
    public GameObject kingPrefab;
    public GameObject queenPrefab;
    public GameObject bishopPrefab;
    public GameObject knightPrefab;
    public GameObject rookPrefab;
    public GameObject pawnPrefab;
    public GameObject spyPrefab;

    private GameObject[,] tiles;

    void Start()
    {
        GenerateGrid();
        PiecesLoc();
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

    void PiecesLoc()
    {
        SpawnPieces(kingPrefab, 4, 7);
        SpawnPieces(bishopPrefab, 3, 7);
        SpawnPieces(knightPrefab, 2, 7);
        SpawnPieces(knightPrefab, 5, 7);
        SpawnPieces(spyPrefab, 0, 0);
    }

    void SpawnPieces(GameObject prefab, int x, int y)
    {
        if (tiles[x, y] == null) return;

        Vector3 pos = tiles[x, y].transform.position;
        GameObject piece = Instantiate(prefab, pos, Quaternion.identity);
        piece.name = prefab.name + $" ({x},{y})";

        Pieces pieceScript = piece.GetComponent<Pieces>();
        if (pieceScript != null)
        {
            pieceScript.startX = x;
            pieceScript.startY = y;
            pieceScript.SnapToTile(tiles[x, y].GetComponent<Tiles>());
        }
    }
}
