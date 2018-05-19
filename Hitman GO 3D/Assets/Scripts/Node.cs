﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {
    Vector2 _coordinate;
    public Vector2 Coordinate { get { return Utility.Vector2Round(_coordinate); } }

    List<Node> _neighborNodes = new List<Node>();
    public List<Node> NeighborNodes { get { return _neighborNodes; } }

    Board _board;

    public GameObject geometry;
    public GameObject linkPrefab;

    public float scaleTime = 0.3f;
    public iTween.EaseType easeType = iTween.EaseType.easeInExpo;

    public bool run = false;

    public float delay = 1f;

    bool _isInitialized = false;

    void Awake()
    {
        _board = Object.FindObjectOfType<Board>();
        _coordinate = new Vector2(transform.position.x, transform.position.z);
    }

    void Start () {
		if (geometry != null)
        {
            geometry.transform.localScale = Vector3.zero;

            if (run)
            {
                InitNode();
            }

            if (_board != null)
            {
                _neighborNodes = FindNeighbors(_board.AllNodes);
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

    public List<Node> FindNeighbors (List<Node> nodes)
    {
        List<Node> nList = new List<Node>();

        foreach (Vector2 dir in Board.directions)
        {
            Node foundNeighbor = nodes.Find(n => n.Coordinate == _coordinate + dir);

            if (foundNeighbor != null && !nList.Contains(foundNeighbor))
            {
                nList.Add(foundNeighbor);
            }
        }

        return nList;
    }

    public void InitNode ()
    {
        if (!_isInitialized)
        {
            ShowGeometry();
            InitNeighbors();
            _isInitialized = true;
        }
    }

    void InitNeighbors ()
    {
        StartCoroutine(InitNeighborsRoutine());
    }

    IEnumerator InitNeighborsRoutine ()
    {
        yield return new WaitForSeconds(delay);

        foreach (Node n in _neighborNodes)
        {
            LinkNode(n);
            n.InitNode();
        }
    }

    void LinkNode (Node targetNode)
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
        }
    }
}