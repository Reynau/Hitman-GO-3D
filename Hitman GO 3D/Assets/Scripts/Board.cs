﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {
    public static float spacing = 2f;

    public static readonly Vector2[] directions =
    {
        new Vector2(spacing, 0f),   // E
        new Vector2(-spacing, 0f),  // W
        new Vector2(0f, spacing),   // N
        new Vector2(0f, -spacing)   // S
    };

    List<Node> _allNodes = new List<Node>();
    public List<Node> AllNodes { get { return _allNodes; } }

    Node _playerNode;
    public Node PlayerNode { get { return _playerNode; } }

    Node _goalNode;
    public Node GoalNode { get { return _goalNode; } }

    public GameObject goalPrefab;
    public float drawGoalTime = 2f;
    public float drawGoalDelay = 1f;
    public iTween.EaseType drawGoalEaseType = iTween.EaseType.easeOutExpo;

    public float delayToCloseDoors = 5f;

    public float initBoardDelay = 2f;

    PlayerMover _player_mover;

    public List<Transform> capturePositions;
    int _currentCapturePosition = 0;
    public int CurrentCapturePosition { get { return _currentCapturePosition; } set { _currentCapturePosition = value; } }

    public float capturePositionGizmoSize = 0.4f;
    public Color capturePositionGizmoColor = Color.blue;

    public List<Transform> capturedCollectiblePositions;
    int _currentCapturedCollectiblePosition = 0;
    public int CurrentCapturedCollectiblePosition { get { return _currentCapturedCollectiblePosition; } set { _currentCapturedCollectiblePosition = value; } }

    public float capturedCollectibleGizmoSize = 0.3f;
    public Color capturedCollectibleGizmoColor = Color.green;

    void Awake()
    {
        _player_mover = Object.FindObjectOfType<PlayerMover>().GetComponent<PlayerMover>();
        GetNodeList();

        _goalNode = FindGoalNode();
    }

    public void GetNodeList ()
    {
        Node[] nList = GameObject.FindObjectsOfType<Node>();
        _allNodes = new List<Node>(nList);
    }

    public Node FindNodeAt (Vector3 pos)
    {
        Vector2 boardCoord = Utility.Vector2Round(new Vector2(pos.x, pos.z));
        return _allNodes.Find(n => n.Coordinate == boardCoord);
    }

    Node FindGoalNode ()
    {
        return _allNodes.Find(n => n.isLevelGoal);
    }

    public Node FindPlayerNode ()
    {
        if (_player_mover != null && !_player_mover.isMoving)
        {
            return FindNodeAt(_player_mover.transform.position);
        }
        return null;
    }

    public List<EnemyManager> FindEnemiesAt (Node node)
    {
        List<EnemyManager> foundEnemies = new List<EnemyManager>();
        EnemyManager[] enemies = Object.FindObjectsOfType<EnemyManager>() as EnemyManager[];

        foreach (EnemyManager enemy in enemies)
        {
            EnemyMover mover = enemy.GetComponent<EnemyMover>();

            if (mover.CurrentNode == node)
            {
                foundEnemies.Add(enemy);
            }
        }

        return foundEnemies;
    }

    public Activable FindActivableAt (Node node)
    {
        Activable[] activables = Object.FindObjectsOfType<Activable>() as Activable[];

        foreach (Activable activable in activables)
        {
            if (activable == null) continue;

            if (activable.transform.position == node.transform.position)
            {
                return activable;
            }
        }
        return null;
    }

    public Collectible FindCollectibleAt(Node node)
    {
        Collectible[] collectibles = Object.FindObjectsOfType<Collectible>() as Collectible[];

        foreach (Collectible collectible in collectibles)
        {
            if (collectible == null) continue;

            if (collectible.transform.position == node.transform.position)
            {
                return collectible;
            }
        }
        return null;
    }

    public Sniper FindSniperAt(Node node)
    {
        Sniper sniper = Object.FindObjectOfType<Sniper>();
        
        if (sniper != null)
        {
            if (sniper.transform.position == node.transform.position)
            {
                return sniper;
            }
        }
        return null;
    }

    public EnemyManager FindNearestEnemy()
    {
        Vector3 playerPosition = _player_mover.transform.position;

        EnemyManager[] enemies = Object.FindObjectsOfType<EnemyManager>() as EnemyManager[];
        EnemyManager nearestEnemy = null;

        foreach (EnemyManager enemy in enemies)
        {
            if (nearestEnemy == null)
            {
                nearestEnemy = enemy;
            }
            else
            {
                Vector3 enemyPosition = enemy.transform.position;
                Vector3 nearestEnemyPosition = nearestEnemy.transform.position;
                if (Vector3.Distance(playerPosition, enemyPosition) < Vector3.Distance(playerPosition, nearestEnemyPosition))
                {
                    nearestEnemy = enemy;
                }
            }
        }
        return nearestEnemy;
    }

    public void UpdatePlayerNode ()
    {
        _playerNode = FindPlayerNode();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0f, 1f, 1f, 0.5f);
        if (_playerNode != null)
        {
            Gizmos.DrawSphere(_playerNode.transform.position, 0.2f);
        }

        Gizmos.color = capturePositionGizmoColor;
        foreach (Transform capturePos in capturePositions)
        {
            Gizmos.DrawCube(capturePos.position, Vector3.one * capturePositionGizmoSize);
        }

        Gizmos.color = capturedCollectibleGizmoColor;
        foreach (Transform capturedCollectiblePos in capturedCollectiblePositions)
        {
            Gizmos.DrawCube(capturedCollectiblePos.position, Vector3.one * capturedCollectibleGizmoSize);
        }
    }

    public void DrawGoal ()
    {
        if (goalPrefab != null && _goalNode != null)
        {
            GameObject goalInstance = Instantiate(goalPrefab, _goalNode.transform.position, Quaternion.identity);
            iTween.ScaleFrom(goalInstance, iTween.Hash(
                "scale", Vector3.zero,
                "time", drawGoalTime,
                "delay", drawGoalDelay,
                "easetype", drawGoalEaseType
            ));
        }
    }

    public void InitActivables ()
    {
        Activable[] activables = Object.FindObjectsOfType<Activable>() as Activable[];

        foreach (Activable activable in activables)
        {
            if (activable != null)
            {
                activable.Init();
            }
        }
    }

    public void InitBoard ()
    {
        if (_playerNode != null)
        {
            StartCoroutine(InitBoardRoutine());
        }
    }

    IEnumerator InitBoardRoutine () {
        yield return new WaitForSeconds(initBoardDelay);
        _playerNode.InitNode();
        yield return new WaitForSeconds(delayToCloseDoors);
        InitActivables();
    }
}
