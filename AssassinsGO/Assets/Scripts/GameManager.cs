using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[System.Serializable]
public enum Turn 
{
    Player,
    Enemy
}

public class GameManager : MonoBehaviour
{
    Board m_board;
    PlayerManager m_player;
    List<EnemyManager> m_enemies;

    public Turn CurrentTurn { get; private set; } = Turn.Player;
    public bool HasLevelStarted { get; set; } = false;
    public bool IsGamePlaying { get; set; } = false;
    public bool IsGameOver { get; set; } = false;
    public bool HasLevelFinished { get; set; } = false;    

    public float delay = 1f;

    public UnityEvent setupEvent;
    public UnityEvent startLevelEvent;
    public UnityEvent playLevelEvent;
    public UnityEvent endLevelEvent;
    public UnityEvent loseLevelEvent;

    void Awake()
    {
        m_board = FindObjectOfType<Board>().GetComponent<Board>();
        m_player = FindObjectOfType<PlayerManager>().GetComponent<PlayerManager>();
        var enemies = FindObjectsOfType<EnemyManager>() as EnemyManager[];
        m_enemies = enemies.ToList();
    }

    void Start()
    {
        if (m_player != null && m_board != null)
        {
            StartCoroutine("RunGameLoop");
        }
        else
        {
            Debug.LogWarning("GAMEMANAGER Error: no player or board found!");
        }
    }

    IEnumerator RunGameLoop()
    {
        yield return StartCoroutine("StartLevelRoutine");
        yield return StartCoroutine("PlayLevelRoutine");
        yield return StartCoroutine("EndLevelRoutine");
    }

    IEnumerator StartLevelRoutine()
    {
        Debug.Log("SETUP LEVEL");
        if (setupEvent != null)
        {
            setupEvent.Invoke();
        }

        Debug.Log("START LEVEL");
        m_player.playerInput.InputEnabled = false;
        while (!HasLevelStarted)
        {
            yield return null;
        }

        if (startLevelEvent != null)
        {
            startLevelEvent.Invoke();
        }
    }

    IEnumerator PlayLevelRoutine()
    {
        Debug.Log("PLAY LEVEL");
        IsGamePlaying = true;
        yield return new WaitForSeconds(delay);
        m_player.playerInput.InputEnabled = true;

        if (playLevelEvent != null)
        {
            playLevelEvent.Invoke();
        }

        while (!IsGameOver)
        {
			yield return null;
             IsGameOver = IsWinner();
        }
    }

    IEnumerator EndLevelRoutine()
    {
        Debug.Log("END LEVEL");
        m_player.playerInput.InputEnabled = false;

        if (endLevelEvent != null)
        {
            endLevelEvent.Invoke();
        }

        while (!HasLevelFinished)
        {
            yield return null;
        }

        RestartLevel();
    }

    public void LoseLevel()
    {
        StartCoroutine(LoseLevelRoutine());
    }

    IEnumerator LoseLevelRoutine()
    {
        IsGameOver = true;

        yield return new WaitForSeconds(1.5f);
        if (loseLevelEvent != null)
        {
            loseLevelEvent.Invoke();
        }
        yield return new WaitForSeconds(2f);
        Debug.Log("LOSE! ==============");
        RestartLevel();
    }

    void RestartLevel()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void PlayLevel()
    {
        HasLevelStarted = true;
    }

    bool IsWinner()
    {
        if (m_board.PlayerNode != null)
        {
            return (m_board.PlayerNode == m_board.GoalNode);
        }
        return false;
    }

    void PlayPlayerTurn()
    {
        CurrentTurn = Turn.Player;
        m_player.IsTurnComplete = false;
    }

    void PlayEnemyTurn()
    {
        CurrentTurn = Turn.Enemy;

        foreach(var enemy in m_enemies)
        {
            if (enemy != null)
            {
                enemy.IsTurnComplete = false;
                enemy.PlayTurn();
            }
        }
    }

    bool IsEnemyTurnComplete()
    {
        foreach(var enemy in m_enemies)
        {
            if (!enemy.IsTurnComplete)
            {
                return false;
            }
        }
        return true;
    }

    public void UpdateTurn()
    {
        if (CurrentTurn == Turn.Player && m_player != null)
        {
            if (m_player.IsTurnComplete)
            {
                PlayEnemyTurn();
            }
        }
        else if (CurrentTurn == Turn.Enemy)
        {
            if (IsEnemyTurnComplete())
            {
                PlayPlayerTurn();
            }            
        }
    }
}
