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

    PlayerMover _player_mover;

    void Awake()
    {
        _player_mover = Object.FindObjectOfType<PlayerMover>().GetComponent<PlayerMover>();
        GetNodeList();
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
}
