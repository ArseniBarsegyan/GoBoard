using System.Collections;

using UnityEngine;

[RequireComponent(typeof(EnemyMover))]
[RequireComponent(typeof(EnemySensor))]
public class EnemyManager : TurnManager
{
    EnemyMover m_enemyMover;
    EnemySensor m_enemySensor;
    Board m_board;

    protected override void Awake()
    {
        base.Awake();

        m_board = FindObjectOfType<Board>().GetComponent<Board>();
        m_enemyMover = GetComponent<EnemyMover>();
        m_enemySensor = GetComponent<EnemySensor>();
    }

    void Update()
    {
    }

    public void PlayTurn()
    {
        StartCoroutine(PlayTurnRoutine());
    }

    IEnumerator PlayTurnRoutine()
    {
        if (m_gameManager != null && !m_gameManager.IsGameOver)
        {
            m_enemySensor.UpdateSensor();
            yield return new WaitForSeconds(0f);

            if (m_enemySensor.FoundPlayer)
            {
                m_gameManager.LoseLevel();
            }
            else
            {
                m_enemyMover.MoveOneTurn();
            }
        }              
    }
}
