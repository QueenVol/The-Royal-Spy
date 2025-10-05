using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BishopAI : AIUnit
{
    protected override Vector2Int[] GetMoveDirections()
    {
        return new Vector2Int[]
        {
            new Vector2Int(1,1), new Vector2Int(-1,1),
            new Vector2Int(1,-1), new Vector2Int(-1,-1)
        };
    }

    protected override bool CanSlideInDirection(Vector2Int dir) => true;
}
