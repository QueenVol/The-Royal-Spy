using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance;
    public bool IsPlayerTurn = true;

    private List<AIUnit> aiUnits = new List<AIUnit>();
    private HashSet<Vector2Int> plannedTargets = new HashSet<Vector2Int>();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (IsPlayerTurn)
        {
            foreach (var ai in aiUnits)
            {
                ai.GeneratePlannedPath();
                ai.ShowPredictedPath();
            }
        }
    }

    public void RegisterAI(AIUnit ai)
    {
        if (!aiUnits.Contains(ai))
            aiUnits.Add(ai);
    }

    public void EndPlayerTurn()
    {
        foreach (var ai in aiUnits)
            ai.ClearPredictedPath();

        IsPlayerTurn = false;
        StartCoroutine(AITurnCoroutine());
    }

    private IEnumerator AITurnCoroutine()
    {
        plannedTargets.Clear();

        aiUnits.RemoveAll(ai => ai == null);

        foreach (AIUnit ai in aiUnits)
        {
            if (ai == null) continue;

            ai.SetPlannedTargets(plannedTargets);

            bool finished = false;
            ai.Act(() => finished = true);

            yield return new WaitUntil(() => finished);

            var path = ai.GetPlannedPath();
            if (path.Count > 0)
                plannedTargets.Add(path[path.Count - 1]);
        }

        IsPlayerTurn = true;

        foreach (var ai in aiUnits)
        {
            if (ai != null)
            {
                ai.GeneratePlannedPath();
                ai.ShowPredictedPath();
            }
        }
    }
}