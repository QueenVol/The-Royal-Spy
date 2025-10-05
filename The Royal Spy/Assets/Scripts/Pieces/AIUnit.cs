using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIUnit : UnitBase
{
    public float thinkDelay = 0.5f;

    public virtual void Act()
    {
        StartCoroutine(AIAction());
    }

    protected virtual IEnumerator AIAction()
    {
        yield return new WaitForSeconds(thinkDelay);

        Vector2Int[] dirs = new Vector2Int[]
        {
            new Vector2Int(1,0), new Vector2Int(-1,0),
            new Vector2Int(0,1), new Vector2Int(0,-1)
        };

        Vector2Int dir = dirs[Random.Range(0, dirs.Length)];
        int newX = x + dir.x;
        int newY = y + dir.y;

        Tiles targetTile = GridManager.Instance.GetTileAt(newX, newY);
        if (targetTile != null && targetTile.isWalkable)
        {
            MoveTo(targetTile);
        }

        TurnManager.Instance.EndAITurn();
    }
}
