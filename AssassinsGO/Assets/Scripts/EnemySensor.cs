using UnityEngine;

public class EnemySensor : MonoBehaviour
{
    public Vector3 directionToSearch = new Vector3(0f, 0f, 0f);

    Node m_nodeToSearch;
    Board m_board;

    public bool FoundPlayer { get; private set; }

    void Awake()
    {
        m_board = FindObjectOfType<Board>().GetComponent<Board>();
    }

    public void UpdateSensor(Node enemyNode)
    {
        Vector3 worldSpacePositionToSearch = 
            transform.TransformVector(directionToSearch) 
            + transform.position;

        if (m_board != null)
        {
            m_nodeToSearch = m_board.FindNodeAt(worldSpacePositionToSearch);
            if (!enemyNode.LinkedNodes.Contains(m_nodeToSearch))
            {
                FoundPlayer = false;
                return;
            }

            if (m_nodeToSearch == m_board.PlayerNode)
            {
                FoundPlayer = true;
            }
        }
    }
}
