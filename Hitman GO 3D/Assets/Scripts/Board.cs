using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {
    public static float spacing = 2f;

    public static readonly Vector2[] directions =
    {
        new Vector2(spacing, 0f),
        new Vector2(-spacing, 0f),
        new Vector2(0f, spacing),
        new Vector2(0f, -spacing)
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

    PlayerMover _player_mover;

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

    public void UpdatePlayerNode ()
    {
        _playerNode = FindPlayerNode();
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

    public void InitBoard ()
    {
        if (_playerNode != null)
        {
            _playerNode.InitNode();
        }
    }
}
