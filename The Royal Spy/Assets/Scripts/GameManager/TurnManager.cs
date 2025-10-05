using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance;
    public bool IsPlayerTurn = true;

    private List<AIUnit> aiUnits = new List<AIUnit>();

    void Awake()
    {
        Instance = this;
    }

    public void RegisterAI(AIUnit ai)
    {
        if (!aiUnits.Contains(ai))
            aiUnits.Add(ai);
    }

    public void EndPlayerTurn()
    {
        IsPlayerTurn = false;
        StartCoroutine(AITurn());
    }

    IEnumerator<WaitForSeconds> AITurn()
    {
        foreach (AIUnit ai in aiUnits)
        {
            ai.Act();
            yield return new WaitForSeconds(1f);
        }

        IsPlayerTurn = true;
    }

    public void EndAITurn()
    {

    }
}
