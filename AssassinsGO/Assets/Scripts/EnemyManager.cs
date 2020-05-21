﻿using System.Collections;

using UnityEngine;

[RequireComponent(typeof(EnemyMover))]
[RequireComponent(typeof(EnemySensor))]
[RequireComponent(typeof(EnemyAttack))]
public class EnemyManager : TurnManager
{
    EnemyMover m_enemyMover;
    EnemySensor m_enemySensor;
    Board m_board;
    EnemyAttack m_enemyAttack;

    protected override void Awake()
    {
        base.Awake();

        m_board = FindObjectOfType<Board>().GetComponent<Board>();
        m_enemyMover = GetComponent<EnemyMover>();
        m_enemySensor = GetComponent<EnemySensor>();
        m_enemyAttack = GetComponent<EnemyAttack>();
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

                Vector3 playerPosition = new Vector3(
                    m_board.PlayerNode.Coordinate.x, 
                    0f,
                    m_board.PlayerNode.Coordinate.y);
                m_enemyMover.Move(playerPosition, 0f);
                while (m_enemyMover.isMoving)
                {
                    yield return null;
                }
                m_enemyAttack.Attack();                
            }
            else
            {
                m_enemyMover.MoveOneTurn();
            }
        }              
    }
}
