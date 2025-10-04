using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pieces : MonoBehaviour
{
    public int startX;
    public int startY;

    void Start()
    {
        Tiles[] allTiles = FindObjectsOfType<Tiles>();
        foreach (Tiles tile in allTiles)
        {
            if (tile.x == startX && tile.y == startY)
            {
                SnapToTile(tile);
                break;
            }
        }
    }

    public void SnapToTile(Tiles tile)
    {
        if (tile != null)
        {
            transform.position = tile.GetWorldPosition();
        }
    }

    public void SnapToNearestTile()
    {
        Tiles[] allTiles = FindObjectsOfType<Tiles>();
        Tiles nearestTile = null;
        float minDist = Mathf.Infinity;

        foreach (Tiles tile in allTiles)
        {
            float dist = Vector3.Distance(transform.position, tile.GetWorldPosition());
            if (dist < minDist)
            {
                minDist = dist;
                nearestTile = tile;
            }
        }

        SnapToTile(nearestTile);
    }
}
