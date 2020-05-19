﻿using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Node : MonoBehaviour
{
    Vector2 m_coordinate;
    public Vector2 Coordinate { get { return Utility.Vector2Round(m_coordinate); } }
    public List<Node> NeighborNodes { get; private set; } = new List<Node>();
    public List<Node> LinkedNodes { get; } = new List<Node>();
    Board m_board;

    public GameObject geometry;
    public GameObject linkPrefab;
    public float scaleTime = 0.3f;
    public iTween.EaseType easeType = iTween.EaseType.easeInExpo;
    public float delay = 1f;
    bool m_isInitialized = false;
    public LayerMask obstacleLayer;
    public bool isLevelGoal = false;

    void Awake()
    {
        m_board = Object.FindObjectOfType<Board>();
        m_coordinate = new Vector2(transform.position.x, transform.position.z);
    }

    void Start()
    {
        if (geometry != null)
        {
            geometry.transform.localScale = Vector3.zero;
            if (m_board != null)
            {
                NeighborNodes = FindNeighbors(m_board.AllNodes);
            }
        }
    }

    public void ShowGeometry()
    {
        if (geometry != null)
        {
            iTween.ScaleTo(geometry, iTween.Hash(
                "time", scaleTime,
                "scale", Vector3.one,
                "easetype", easeType,
                "delay", delay
            ));
        }
    }

    public List<Node> FindNeighbors(List<Node> nodes)
    {
        List<Node> nList = new List<Node>();

        foreach (Vector2 dir in Board.directions)
        {
            Node foundNeighbor = FindNeighborAt(nodes, dir);

            if (foundNeighbor != null && !nList.Contains(foundNeighbor))
            {
                nList.Add(foundNeighbor);
            }
        }
        return nList;
    }

    public Node FindNeighborAt(List<Node> nodes, Vector2 dir)
    {
        return nodes.Find(n => n.Coordinate == Coordinate + dir);
    }

    public Node FindNeighborAt(Vector2 dir)
    {
        return FindNeighborAt(NeighborNodes, dir);
    }


    public void InitNode()
    {
        if (!m_isInitialized)
        {
            ShowGeometry();

            InitNeighbors();

            m_isInitialized = true;
        }
    }

    void InitNeighbors()
    {
        StartCoroutine(InitNeighborsRoutine());
    }

    IEnumerator InitNeighborsRoutine()
    {
        yield return new WaitForSeconds(delay);

        foreach (Node n in NeighborNodes)
        {
            if (!LinkedNodes.Contains(n))
            {
                Obstacle obstacle = FindObstacle(n);
                if (obstacle == null)
                {
                    LinkNode(n);
                    n.InitNode();
                }
            }
        }
    }

    void LinkNode(Node targetNode)
    {
        if (linkPrefab != null)
        {
            GameObject linkInstance = Instantiate(linkPrefab, transform.position, Quaternion.identity);
            linkInstance.transform.parent = transform;

            Link link = linkInstance.GetComponent<Link>();
            if (link != null)
            {
                link.DrawLink(transform.position, targetNode.transform.position);
            }

            if (!LinkedNodes.Contains(targetNode))
            {
                LinkedNodes.Add(targetNode);
            }

            if (!targetNode.LinkedNodes.Contains(this))
            {
                targetNode.LinkedNodes.Add(this);
            }
        }
    }

    Obstacle FindObstacle(Node targetNode)
    {
        Vector3 checkDirection = targetNode.transform.position - transform.position;
        RaycastHit raycastHit;

        if (Physics.Raycast(transform.position, checkDirection, out raycastHit, Board.spacing + 0.1f,
                            obstacleLayer))
        {
            return raycastHit.collider.GetComponent<Obstacle>();
        }
        return null;
    }
}
