using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIUnit : UnitBase
{
    public float thinkDelay = 0.5f;
    private List<Vector2Int> plannedPath = new List<Vector2Int>();
    private HashSet<Vector2Int> otherPlannedTargets;

    public void Act(Action onComplete)
    {
        StartCoroutine(AIAction(onComplete));
    }

    protected IEnumerator AIAction(Action onComplete)
    {
        yield return new WaitForSeconds(thinkDelay);

        if (plannedPath == null || plannedPath.Count == 0)
            GeneratePlannedPath();

        if (plannedPath.Count > 0)
        {
            Vector2Int target = plannedPath[plannedPath.Count - 1];
            Tiles tile = GridManager.Instance.GetTileAt(target.x, target.y);
            if (tile != null)
            {
                GridManager.Instance.MoveUnit(this, tile);
            }
        }

        onComplete?.Invoke();
    }

    protected virtual Vector2Int[] GetMoveDirections()
    {
        return new Vector2Int[]
        {
            new Vector2Int(1,0), new Vector2Int(-1,0),
            new Vector2Int(0,1), new Vector2Int(0,-1),
            new Vector2Int(1,1), new Vector2Int(-1,1),
            new Vector2Int(1,-1), new Vector2Int(-1,-1)
        };
    }

    protected virtual bool CanSlideInDirection(Vector2Int dir) => false;

    public List<Vector2Int> GetAllPossibleMoves()
    {
        List<Vector2Int> moves = new List<Vector2Int>();
        Vector2Int[] dirs = GetMoveDirections();

        foreach (var dir in dirs)
        {
            int step = 1;
            while (true)
            {
                int newX = x + dir.x * step;
                int newY = y + dir.y * step;

                Tiles tile = GridManager.Instance.GetTileAt(newX, newY);
                if (tile == null || !tile.isWalkable) break;
                if (GridManager.Instance.IsTileOccupied(newX, newY))
                {
                    UnitBase target = GridManager.Instance.GetUnitAt(newX, newY);
                    if (target != null && target.isPlayerControlled != this.isPlayerControlled)
                    {
                        moves.Add(new Vector2Int(newX, newY));
                    }
                    break;
                }
                if (otherPlannedTargets != null && otherPlannedTargets.Contains(new Vector2Int(newX, newY))) break;

                moves.Add(new Vector2Int(newX, newY));

                if (!CanSlideInDirection(dir)) break;
                step++;
            }
        }
        return moves;
    }

    public virtual List<Vector2Int> GetPlannedPath()
    {
        plannedPath.Clear();

        List<Vector2Int> allMoves = GetAllPossibleMoves();
        if (allMoves.Count == 0)
            return plannedPath;

        Vector2Int target = allMoves[UnityEngine.Random.Range(0, allMoves.Count)];

        if (CanSlideInDirection(GetMoveDirTowards(target)))
        {
            Vector2Int dir = GetMoveDirTowards(target);
            Vector2Int current = new Vector2Int(x, y);

            int safetyCounter = 0;
            while (current != target && safetyCounter < 50)
            {
                current += dir;
                plannedPath.Add(current);
                safetyCounter++;
            }
        }
        else
        {
            plannedPath.Add(target);
        }

        return plannedPath;
    }

    private Vector2Int GetMoveDirTowards(Vector2Int target)
    {
        int dx = Mathf.Clamp(target.x - x, -1, 1);
        int dy = Mathf.Clamp(target.y - y, -1, 1);
        return new Vector2Int(dx, dy);
    }

    public void GeneratePlannedPath()
    {
        plannedPath.Clear();

        List<Vector2Int> allMoves = GetAllPossibleMoves();
        if (allMoves.Count == 0) return;

        Vector2Int target = allMoves[UnityEngine.Random.Range(0, allMoves.Count)];

        if (CanSlideInDirection(GetMoveDirTowards(target)))
        {
            Vector2Int dir = GetMoveDirTowards(target);
            Vector2Int current = new Vector2Int(x, y);
            int safetyCounter = 0;
            while (current != target && safetyCounter < 50)
            {
                current += dir;
                plannedPath.Add(current);
                safetyCounter++;
            }
        }
        else
        {
            plannedPath.Add(target);
        }
    }

    public void ShowPredictedPath()
    {
        if (plannedPath == null || plannedPath.Count == 0)
            GeneratePlannedPath();

        foreach (var pos in plannedPath)
        {
            Tiles tile = GridManager.Instance.GetTileAt(pos.x, pos.y);
            if (tile != null)
                tile.WarningHighlight(true);
        }
    }

    public void ClearPredictedPath()
    {
        if (plannedPath == null) return;

        foreach (var pos in plannedPath)
        {
            Tiles tile = GridManager.Instance.GetTileAt(pos.x, pos.y);
            if (tile != null)
                tile.WarningHighlight(false);
        }
    }

    public void SetPlannedTargets(HashSet<Vector2Int> targets)
    {
        otherPlannedTargets = targets;
    }
}