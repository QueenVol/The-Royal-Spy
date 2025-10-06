using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartNEnd : MonoBehaviour
{
    public static StartNEnd Instance;

    public GameObject startPanel;
    public GameObject endPanel;
    public GameObject winningText;
    public GameObject losingText;
    public GameObject casualRestartButton;

    public AudioSource song;

    void Awake()
    {
        Instance = this;
    }

    public void OnStartButtonClicked()
    {
        startPanel.SetActive(false);
        song.Stop();
        TurnManager.Instance.StartGame();
    }

    public void EndGame(bool playerWon)
    {
        endPanel.SetActive(true);
        if (playerWon)
        {
            winningText.SetActive(true);
            losingText.SetActive(false);
            casualRestartButton.SetActive(false);
        }
        if (!playerWon)
        {
            winningText.SetActive(false);
            losingText.SetActive(true);
            casualRestartButton.SetActive(false);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
