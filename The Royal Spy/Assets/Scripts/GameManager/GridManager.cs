using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

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
    private UnitBase selectedUnit;
    private List<Tiles> highlightedTiles = new List<Tiles>();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        GenerateGrid();
        SpawnAllPieces();
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
                tile.GetComponent<Tiles>().Init(x, y);
                tiles[x, y] = tile;
            }
        }
    }

    void SpawnAllPieces()
    {
        SpawnAI(kingPrefab, 4, 7);
        SpawnAI(queenPrefab, 3, 7);
        SpawnAI(bishopPrefab, 2, 7);
        SpawnAI(knightPrefab, 5, 7);
        SpawnAI(rookPrefab, 0, 7);
        SpawnAI(pawnPrefab, 1, 6);

        SpawnPlayer(spyPrefab, 0, 0);
    }

    void SpawnAI(GameObject prefab, int x, int y)
    {
        GameObject go = Instantiate(prefab, tiles[x, y].transform.position, Quaternion.identity);
        AIUnit ai = go.GetComponent<AIUnit>();
        ai.Init(x, y);
        TurnManager.Instance.RegisterAI(ai);
    }

    void SpawnPlayer(GameObject prefab, int x, int y)
    {
        GameObject go = Instantiate(prefab, tiles[x, y].transform.position, Quaternion.identity);
        PlayerUnit player = go.GetComponent<PlayerUnit>();
        player.isPlayerControlled = true;
        player.Init(x, y);
    }

    public Tiles GetTileAt(int x, int y)
    {
        if (x < 0 || x >= width || y < 0 || y >= height) return null;
        return tiles[x, y]?.GetComponent<Tiles>();
    }

    public void OnTileClicked(Tiles tile)
    {
        if (!TurnManager.Instance.IsPlayerTurn) return;

        if (selectedUnit == null)
        {
            UnitBase unit = GetUnitAt(tile.x, tile.y);
            if (unit != null && unit.isPlayerControlled)
            {
                SelectUnit(unit);
            }
        }
        else
        {
            if (highlightedTiles.Contains(tile))
            {
                selectedUnit.MoveTo(tile);
                ClearHighlights();
                selectedUnit = null;
                TurnManager.Instance.EndPlayerTurn();
            }
            else
            {
                ClearHighlights();
                selectedUnit = null;
            }
        }
    }

    public void SelectUnit(UnitBase unit)
    {
        selectedUnit = unit;
        ShowMoveOptions(unit);
    }

    void ShowMoveOptions(UnitBase unit)
    {
        ClearHighlights();

        Vector2Int[] dirs = new Vector2Int[]
        {
            new Vector2Int(1,0), new Vector2Int(-1,0),
            new Vector2Int(0,1), new Vector2Int(0,-1),
            new Vector2Int(1,1), new Vector2Int(-1,1),
            new Vector2Int(1,-1), new Vector2Int(-1,-1)
        };

        foreach (Vector2Int d in dirs)
        {
            int tx = unit.x + d.x;
            int ty = unit.y + d.y;

            Tiles t = GetTileAt(tx, ty);
            if (t != null && t.isWalkable)
            {
                t.Highlight(true);
                highlightedTiles.Add(t);
            }
        }
    }

    void ClearHighlights()
    {
        foreach (Tiles t in highlightedTiles)
            t.Highlight(false);
        highlightedTiles.Clear();
    }

    UnitBase GetUnitAt(int x, int y)
    {
        foreach (UnitBase u in FindObjectsOfType<UnitBase>())
        {
            if (u.x == x && u.y == y)
                return u;
        }
        return null;
    }
}
