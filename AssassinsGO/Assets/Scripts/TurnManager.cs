using UnityEngine;

public class TurnManager : MonoBehaviour
{
    protected GameManager m_gameManager;
    public bool IsTurnComplete { get; set; }

    protected virtual void Awake()
    {
        m_gameManager = FindObjectOfType<GameManager>().GetComponent<GameManager>();
    }

    public virtual void FinishTurn()
    {
        IsTurnComplete = true;
        
        if (m_gameManager != null)
        {
            m_gameManager.UpdateTurn();
        }
    }
}
