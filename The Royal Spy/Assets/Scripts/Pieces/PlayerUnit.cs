using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : UnitBase
{
    private void OnMouseDown()
    {
        if (TurnManager.Instance.IsPlayerTurn)
        {
            GridManager.Instance.SelectUnit(this);
        }
    }
}
