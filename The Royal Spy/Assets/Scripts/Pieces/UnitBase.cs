using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitBase : MonoBehaviour
{
    public int x;
    public int y;
    public int health = 10;
    public int moveRange = 2;
    public bool isPlayerControlled = false;

    public virtual void Init(int gridX, int gridY)
    {
        x = gridX;
        y = gridY;
        SnapToTile(GridManager.Instance.GetTileAt(gridX, gridY));
    }

    public virtual void SnapToTile(Tiles tile)
    {
        if (tile != null)
            transform.position = tile.GetWorldPosition();
    }

    public virtual void MoveTo(Tiles tile)
    {
        if (tile != null)
        {
            x = tile.x;
            y = tile.y;
            SnapToTile(tile);
        }
    }

    public virtual void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        GridManager.Instance.unitPositions.Remove(new Vector2Int(x, y));
        Destroy(gameObject);
    }
}