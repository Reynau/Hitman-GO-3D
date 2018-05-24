using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {
    Vector2 _coordinate;
    public Vector2 Coordinate { get { return Utility.Vector2Round(_coordinate); } }

    List<Node> _neighborNodes = new List<Node>();
    public List<Node> NeighborNodes { get { return _neighborNodes; } }

    List<Node> _linkedNodes = new List<Node>();
    public List<Node> LinkedNodes { get { return _linkedNodes; } }

    Board _board;

    public GameObject geometry;
    public GameObject linkPrefab;

    public float scaleTime = 0.3f;
    public iTween.EaseType easeType = iTween.EaseType.easeInExpo;

    public float delay = 1f;

    bool _isInitialized = false;

    public LayerMask obstacleLayer;

    public bool isLevelGoal = false;

    void Awake()
    {
        _board = Object.FindObjectOfType<Board>();
        _coordinate = new Vector2(transform.position.x, transform.position.z);
    }

    void Start () {
		if (geometry != null)
        {
            geometry.transform.localScale = Vector3.zero;

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
            Node foundNeighbor = FindNeighborAt(nodes, dir);

            if (foundNeighbor != null && !nList.Contains(foundNeighbor))
            {
                nList.Add(foundNeighbor);
            }
        }

        return nList;
    }

    public Node FindNeighborAt (List<Node> nodes, Vector2 dir)
    {
        return nodes.Find(n => n.Coordinate == Coordinate + dir);
    }

    public Node FindNeighborAt(Vector2 dir)
    {
        return FindNeighborAt(NeighborNodes, dir);
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
            if (!_linkedNodes.Contains(n))
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

    public void LinkNode (Node targetNode)
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
            if (!_linkedNodes.Contains(targetNode))
            {
                _linkedNodes.Add(targetNode);
            }

            if (!targetNode.LinkedNodes.Contains(this))
            {
                targetNode.LinkedNodes.Add(this);
            }
        }
    }

    public Link FindLinkAt (Node targetNode)
    {
        Link[] originLinks = GetComponentsInChildren<Link>();

        foreach (Link link in originLinks)
        {
            if (link.EndPos == targetNode.transform.position)
            {
                return link;
            }
        }

        return null;
    }

    public void RemoveLink (Node targetNode)
    {
        if (targetNode != null)
        {
            Link link = FindLinkAt(targetNode);
            if (link != null)
            {
                 _linkedNodes.Remove(targetNode);
                targetNode.LinkedNodes.Remove(this);
                link.RemoveLink();  // Animation
                Destroy(link.gameObject, link.delay);
            }
        }
    }

    Obstacle FindObstacle (Node targetNode)
    {
        Vector3 direction = targetNode.transform.position - transform.position;
        RaycastHit raycastHit;

        if (Physics.Raycast(transform.position, direction, out raycastHit, Board.spacing + 0.1f, obstacleLayer))
        {
            return raycastHit.collider.GetComponent<Obstacle>();
        }
        return null;
    }
}