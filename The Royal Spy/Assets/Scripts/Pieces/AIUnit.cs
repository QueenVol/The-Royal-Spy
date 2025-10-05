using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        Vector2Int[] dirs = {
            new Vector2Int(1,0), new Vector2Int(-1,0),
            new Vector2Int(0,1), new Vector2Int(0,-1),
            new Vector2Int(1,1), new Vector2Int(-1,1),
            new Vector2Int(1,-1), new Vector2Int(-1,-1)
        };

        for (int i = 0; i < dirs.Length; i++)
        {
            int r = UnityEngine.Random.Range(i, dirs.Length);
            Vector2Int temp = dirs[i];
            dirs[i] = dirs[r];
            dirs[r] = temp;
        }

        foreach (var dir in dirs)
        {
            int newX = x + dir.x;
            int newY = y + dir.y;

            Tiles targetTile = GridManager.Instance.GetTileAt(newX, newY);
            if (targetTile != null && targetTile.isWalkable && !GridManager.Instance.IsTileOccupied(newX, newY))
            {
                GridManager.Instance.MoveUnit(this, targetTile);
                break;
            }
        }

        onComplete?.Invoke();
    }
}
