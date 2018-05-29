using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Position
{
    N,
    E,
    S,
    W
};

public class Rock : Activable
{
    public float delay = 1.4f;

    Board _board;

    public Position pos;

    public GameObject arrowPrefab;
    GameObject _arrow;
    public float scale = 1f;
    public float startDistance = 0.25f;
    public float endDistance = 0.5f;

    public float moveSpeed = 2.1f;
    public float iTweenDelay = 0f;
    public float moveTime = 1f;
    public iTween.EaseType easeType = iTween.EaseType.easeInOutExpo;
    public iTween.EaseType easeTypePath = iTween.EaseType.linear;

    void Awake()
    {
        _board = Object.FindObjectOfType<Board>().GetComponent<Board>();
        InvokeRepeating("ChangeDir", 2, 2f);
        SetupArrow();
        MoveArrow();
    }

    public override IEnumerator Activate()
    {
        if (!_active)
        {
            yield return StartCoroutine(PickRockRoutine());
            _active = true;
        }
    }

    IEnumerator PickRockRoutine()
    {
        CancelInvoke("ChangeDir");
        Vector3 posRock = transform.position;
        Vector3 mid = transform.position;
        mid.y += 2f;
        switch (pos)
        {
            case Position.N:
                posRock.z += 4f;
                mid.z += 2f;
                break;
            case Position.E:
                posRock.x += 4f;
                mid.x += 2f;
                break;
            case Position.S:
                posRock.z -= 4f;
                mid.z -= 2f;
                break;
            case Position.W:
                posRock.x -= 4f;
                mid.x -= 2f;
                break;

        }
        Vector3 heigh = new Vector3(0f, 1f, 0f);
        Vector3[] path = { transform.position + heigh,  mid, posRock };
        ThrowRock(path);
        Destroy(_arrow);
        yield return new WaitForSeconds(delay);
        activateActivableEvent.Invoke();
        Node nodeDest = _board.FindNodeAt(posRock);
        if (nodeDest != null)
        {
            EnemyManager[] enemies = Object.FindObjectsOfType<EnemyManager>() as EnemyManager[];
            // Play rock sound

            foreach (EnemyManager enemy in enemies)
            {
                EnemyMover mover = enemy.GetComponent<EnemyMover>();

                if (mover.CurrentNode == nodeDest)
                {
                    enemy.Die();
                }
                else
                {
                    List<Node> neighs = mover.CurrentNode.FindNeighbors(_board.AllNodes);
                    Node nearestNode = FindNearestNode(neighs, nodeDest, mover.CurrentNode);
                    mover.destination = nearestNode.transform.position;
                    mover.FaceDestination();
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }

    Node FindNearestNode(List<Node> neighs, Node nodeDest, Node enemyNode)
    {
        Vector3 nodePos = nodeDest.transform.position;

        Node nearestNode = null;

        foreach (Node neigh in neighs)
        {
            if (!(enemyNode.FindLinkAt(neigh) == null && neigh.FindLinkAt(enemyNode) == null))
            {
                if (nearestNode == null)
                {
                    nearestNode = neigh;
                }
                else
                {
                    Vector3 neighPos = neigh.transform.position;
                    Vector3 nearestPos = nearestNode.transform.position;
                    if (Vector3.Distance(nodePos, neighPos) < Vector3.Distance(nodePos, nearestPos))
                    {
                        nearestNode = neigh;
                    }
                }
            }
        }
        return nearestNode;
    }

    void ChangeDir()
    {
        pos = Next(pos);
        ResetArrow();
        MoveArrow();
    }

    public static Position Next(Position pos)
    {
        Position[] positions = (Position[])System.Enum.GetValues(pos.GetType());
        int j = System.Array.IndexOf(positions, pos) + 1;
        return (positions.Length == j) ? positions[0] : positions[j];
    }

    void SetupArrow()
    {
        if (arrowPrefab == null)
        {
            Debug.LogWarning("ROCK SetupArrow Error: Missing arrow prefab!");
            return;
        }

        Vector2 dir = new Vector2(0f, 0f);
        switch (pos)
        {
            case Position.N:
                dir = Board.directions[2];
                break;
            case Position.E:
                dir = Board.directions[0];
                break;
            case Position.S:
                dir = Board.directions[3];
                break;
            case Position.W:
                dir = Board.directions[1];
                break;
        }
        if (dir.x != 0f || dir.y != 0f)
        {
            Vector3 dirVector = new Vector3(dir.normalized.x, 0f, dir.normalized.y);
            Quaternion rotation = Quaternion.LookRotation(dirVector);
            _arrow = Instantiate(arrowPrefab, transform.position + dirVector * startDistance, rotation);
            _arrow.transform.localScale = new Vector3(scale, scale, scale);
            _arrow.transform.parent = transform;
        }
    }

    void MoveArrow()
    {
        iTween.MoveBy(_arrow, iTween.Hash(
            "z", endDistance - startDistance,
            "looptype", iTween.LoopType.pingPong,
            "time", moveTime,
            "easetype", easeType
        ));
    }

    void ResetArrow()
    {
        if (_arrow != null)
        {
            iTween.Stop(_arrow);
            Destroy(_arrow);
            Vector2 dir = new Vector2(0f, 0f);
            switch (pos)
            {
                case Position.N:
                    dir = Board.directions[2];
                    break;
                case Position.E:
                    dir = Board.directions[0];
                    break;
                case Position.S:
                    dir = Board.directions[3];
                    break;
                case Position.W:
                    dir = Board.directions[1];
                    break;
            }
            Vector3 dirVector = new Vector3(dir.normalized.x, 0f, dir.normalized.y);
            Quaternion rotation = Quaternion.LookRotation(dirVector);
            _arrow = Instantiate(arrowPrefab, transform.position + dirVector * startDistance, rotation);
            _arrow.transform.localScale = new Vector3(scale, scale, scale);
            _arrow.transform.parent = transform;
        }
    }

    void ThrowRock(Vector3[] path)
    {
        iTween.MoveTo(gameObject, iTween.Hash(
            "path", path,
            "easetype", easeTypePath,
            "time", delay
        ));
    }
}
