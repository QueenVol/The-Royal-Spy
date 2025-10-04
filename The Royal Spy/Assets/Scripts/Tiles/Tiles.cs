using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiles : MonoBehaviour
{
    public int x;
    public int y;

    public bool isWalkable = true;

    public void Init(int gridX, int gridY)
    {
        x = gridX;
        y = gridY;
    }


    public Vector3 GetWorldPosition()
    {
        return transform.position;
    }
}