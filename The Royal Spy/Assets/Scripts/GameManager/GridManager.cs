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

    private Tiles[,] tiles;
    private UnitBase selectedUnit;
    private List<Tiles> highlightedTiles = new List<Tiles>();

    public Dictionary<Vector2Int, UnitBase> unitPositions = new Dictionary<Vector2Int, UnitBase>();

    void Awake() => Instance = this;

    void Start()
    {
        GenerateGrid();
        SpawnAllPieces();
    }

    void GenerateGrid()
    {
        tiles = new Tiles[width, height];
        float offsetX = (width - 1) * cellSize / 2f;
        float offsetY = (height - 1) * cellSize / 2f;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 pos = new Vector3(x * cellSize - offsetX, y * cellSize - offsetY, 0);
                GameObject tileGO = Instantiate(tilePrefab, pos, Quaternion.identity, transform);
                tileGO.name = $"Tile {x},{y}";
                Tiles t = tileGO.GetComponent<Tiles>();
                t.Init(x, y);
                tiles[x, y] = t;
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
        GameObject go = Instantiate(prefab, tiles[x, y].GetWorldPosition(), Quaternion.identity);
        AIUnit ai = go.GetComponent<AIUnit>();
        ai.Init(x, y);
        ai.isPlayerControlled = false;
        TurnManager.Instance.RegisterAI(ai);
        unitPositions[new Vector2Int(x, y)] = ai;
    }

    void SpawnPlayer(GameObject prefab, int x, int y)
    {
        GameObject go = Instantiate(prefab, tiles[x, y].GetWorldPosition(), Quaternion.identity);
        PlayerUnit player = go.GetComponent<PlayerUnit>();
        player.Init(x, y);
        player.isPlayerControlled = true;
        unitPositions[new Vector2Int(x, y)] = player;
    }

    public Tiles GetTileAt(int x, int y)
    {
        if (x < 0 || x >= width || y < 0 || y >= height) return null;
        return tiles[x, y];
    }

    public bool IsTileOccupied(int x, int y)
    {
        return unitPositions.ContainsKey(new Vector2Int(x, y));
    }

    public UnitBase GetUnitAt(int x, int y)
    {
        unitPositions.TryGetValue(new Vector2Int(x, y), out UnitBase unit);
        return unit;
    }

    public void MoveUnit(UnitBase unit, Tiles targetTile)
    {
        if (unit == null || targetTile == null) return;

        Vector2Int oldPos = new Vector2Int(unit.x, unit.y);

        UnitBase targetUnit = GetUnitAt(targetTile.x, targetTile.y);
        if (targetUnit != null && targetUnit.isPlayerControlled != unit.isPlayerControlled)
        {
            targetUnit.TakeDamage(targetUnit.health);
        }

        unit.MoveTo(targetTile);

        unitPositions.Remove(oldPos);
        unitPositions[new Vector2Int(targetTile.x, targetTile.y)] = unit;
    }

    public void OnTileClicked(Tiles tile)
    {
        if (!TurnManager.Instance.IsPlayerTurn) return;

        if (selectedUnit == null)
        {
            UnitBase unit = GetUnitAt(tile.x, tile.y);
            if (unit != null && unit.isPlayerControlled)
                SelectUnit(unit);
        }
        else
        {
            if (highlightedTiles.Contains(tile))
            {
                MoveUnit(selectedUnit, tile);
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

        Vector2Int[] dirs = {
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
            if (t == null || !t.isWalkable) continue;

            UnitBase targetUnit = GetUnitAt(tx, ty);
            if (targetUnit == null)
            {
                t.Highlight(true);
                highlightedTiles.Add(t);
            }
            else if (targetUnit.isPlayerControlled != unit.isPlayerControlled)
            {
                t.WarningHighlight(true);
                highlightedTiles.Add(t);
            }
        }
    }

    void ClearHighlights()
    {
        foreach (Tiles t in highlightedTiles)
        {
            t.Highlight(false);
            t.WarningHighlight(false);
        }
        highlightedTiles.Clear();
    }
}