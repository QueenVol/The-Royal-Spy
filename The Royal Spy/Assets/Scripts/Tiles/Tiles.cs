using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiles : MonoBehaviour
{
    public int x;
    public int y;

    // 是否可走、是否有陷阱等，可以在这里拓展
    public bool isWalkable = true;

    public void Init(int gridX, int gridY)
    {
        x = gridX;
        y = gridY;
    }

    private void OnMouseDown()
    {
    }
}
