using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private PlayerManager m_player;

    private void Awake()
    {
        m_player = FindObjectOfType<PlayerManager>().GetComponent<PlayerManager>();
    }

    public void Attack()
    {
        if (m_player != null)
        {
            m_player.Die();
        }
    }
}
