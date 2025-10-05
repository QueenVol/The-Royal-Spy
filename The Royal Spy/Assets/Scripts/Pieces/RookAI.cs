using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RookAI : AIUnit
{
    protected override Vector2Int[] GetMoveDirections()
    {
        return new Vector2Int[]
        {
            new Vector2Int(1,0), new Vector2Int(-1,0),
            new Vector2Int(0,1), new Vector2Int(0,-1)
        };
    }

    protected override bool CanSlideInDirection(Vector2Int dir) => true;
}
