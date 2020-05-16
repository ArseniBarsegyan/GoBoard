using UnityEngine;

[RequireComponent(typeof(EnemyMover))]
[RequireComponent(typeof(EnemySensor))]
public class EnemyManager : MonoBehaviour
{
    EnemyMover m_enemyMover;
    EnemySensor m_enemySensor;
    Board m_board;

    private void Awake()
    {
        m_board = FindObjectOfType<Board>().GetComponent<Board>();
        m_enemyMover = GetComponent<EnemyMover>();
        m_enemySensor = GetComponent<EnemySensor>();
    }

    void Update()
    {
    }
}
