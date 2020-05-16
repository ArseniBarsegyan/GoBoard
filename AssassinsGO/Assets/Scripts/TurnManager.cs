using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    protected GameManager m_gameManager;
    public bool IsTurnComplete { get; set; }

    protected virtual void Awake()
    {
        m_gameManager = FindObjectOfType<GameManager>().GetComponent<GameManager>();
    }

    public void FinishTurn()
    {
        IsTurnComplete = true;
        
        // Update the game manager
    }
}
