using System.Collections.Generic;

using UnityEngine;

public class Board : MonoBehaviour
{
    public static float spacing = 2f;

    public static readonly Vector2[] directions =
    {
        new Vector2(spacing, 0f),
        new Vector2(-spacing, 0f),
        new Vector2(0f, spacing),
        new Vector2(0f, -spacing)
    };

    public List<Node> AllNodes { get; private set; } = new List<Node>();
    public List<Transform> capturePositions;
    public Node PlayerNode { get; private set; }
    public Node GoalNode { get; private set; }    
    public int CurrentCapturePosition { get; set; }

    public GameObject goalPrefab;
    public float drawGoalTime = 2f;
    public float drawGoalDelay = 2f;
    public iTween.EaseType drawGoalEaseType = iTween.EaseType.easeOutExpo;
    public float capturePositionIconSize = 0.4f;
    public Color capturePositionIconColor = Color.blue;

    PlayerMover m_player;

    void Awake()
    {
        m_player = Object.FindObjectOfType<PlayerMover>().GetComponent<PlayerMover>();
        GetNodeList();

        GoalNode = FindGoalNode();
    }

    public void GetNodeList()
    {
        Node[] nList = Object.FindObjectsOfType<Node>();
        AllNodes = new List<Node>(nList);
    }

    public Node FindNodeAt(Vector3 pos)
    {
        Vector2 boardCoord = Utility.Vector2Round(new Vector2(pos.x, pos.z));
        return AllNodes.Find(n => n.Coordinate == boardCoord);
    }

    Node FindGoalNode()
    {
        return AllNodes.Find(n => n.isLevelGoal);
    }

    public Node FindPlayerNode()
    {
        if (m_player != null && !m_player.isMoving)
        {
            return FindNodeAt(m_player.transform.position);
        }
        return null;
    }

    public List<EnemyManager> FindEnemiesAt(Node node)
    {
        List<EnemyManager> foundEnemies = new List<EnemyManager>();
        EnemyManager[] enemies = FindObjectsOfType<EnemyManager>() as EnemyManager[];

        foreach(var enemy in enemies)
        {
            EnemyMover mover = enemy.GetComponent<EnemyMover>();
            if (mover.CurrentNode == node)
            {
                foundEnemies.Add(enemy);
            }
        }
        return foundEnemies;
    }

    public void UpdatePlayerNode()
    {
        PlayerNode = FindPlayerNode();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0f, 1f, 1f, 0.5f);
        if (PlayerNode != null)
        {
            Gizmos.DrawSphere(PlayerNode.transform.position, 0.2f);
        }

        Gizmos.color = capturePositionIconColor;

        foreach(var capturePos in capturePositions)
        {
            Gizmos.DrawCube(capturePos.position, Vector3.one * capturePositionIconSize);
        }
    }

    public void DrawGoal()
    {
        if (goalPrefab != null && GoalNode != null)
        {
            GameObject goalInstance = Instantiate(goalPrefab, GoalNode.transform.position,
                                                  Quaternion.identity);
            iTween.ScaleFrom(goalInstance, iTween.Hash(
                "scale", Vector3.zero,
                "time", drawGoalTime,
                "delay", drawGoalDelay,
                "easetype", drawGoalEaseType
            ));
        }
    }

    public void InitBoard()
    {
        if (PlayerNode != null)
        {
            PlayerNode.InitNode();
        }
    }
}
