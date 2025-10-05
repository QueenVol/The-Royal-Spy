using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIUnit : UnitBase
{
    public float thinkDelay = 0.5f;

    public void Act(Action onComplete)
    {
        StartCoroutine(AIAction(onComplete));
    }

    protected IEnumerator AIAction(Action onComplete)
    {
        yield return new WaitForSeconds(thinkDelay);

        List<Vector2Int> possibleMoves = GetAllPossibleMoves();

        if (possibleMoves.Count > 0)
        {
            Vector2Int target = possibleMoves[UnityEngine.Random.Range(0, possibleMoves.Count)];
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

    private List<Vector2Int> GetAllPossibleMoves()
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
                if (GridManager.Instance.IsTileOccupied(newX, newY)) break;

                moves.Add(new Vector2Int(newX, newY));

                if (!CanSlideInDirection(dir)) break;
                step++;
            }
        }

        return moves;
    }
}
