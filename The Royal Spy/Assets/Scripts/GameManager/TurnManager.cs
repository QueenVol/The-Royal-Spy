using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance;
    public bool IsPlayerTurn = false;

    public PlayerUnit playerUnit;
    public AIUnit kingUnit;

    private List<AIUnit> aiUnits = new List<AIUnit>();
    private HashSet<Vector2Int> plannedTargets = new HashSet<Vector2Int>();

    public bool playerDetected = false;

    void Awake()
    {
        Instance = this;
    }

    public void StartGame()
    {
        IsPlayerTurn = true;

        foreach (var ai in aiUnits)
        {
            ai.GeneratePlannedPath();
            ai.ShowPredictedPath();
        }
    }

    public void RegisterPlayer(PlayerUnit player)
    {
        playerUnit = player;
    }

    public void RegisterKing(AIUnit king)
    {
        kingUnit = king;
        RegisterAI(king);
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

    public void SetPlayerDetected()
    {
        playerDetected = true;
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

    public void OnKingDeath()
    {
        StartNEnd.Instance.EndGame(true);
        StopAllCoroutines();
    }

    public void OnPlayerDeath()
    {
        StartNEnd.Instance.EndGame(false);
        StopAllCoroutines();
    }
}