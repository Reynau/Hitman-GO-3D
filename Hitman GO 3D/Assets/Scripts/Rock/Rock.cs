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
    public float delay;

    Board _board;
    PlayerMover _player_mover;

    public Position pos;

    private void Awake()
    {
        _board = Object.FindObjectOfType<Board>().GetComponent<Board>();
        _player_mover = Object.FindObjectOfType<PlayerMover>().GetComponent<PlayerMover>();
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
        yield return new WaitForSeconds(delay);        
        activateActivableEvent.Invoke();
        Vector3 posRock = _player_mover.CurrentNode.transform.position;
        switch (pos)
        {
            case Position.N:
                posRock.z += 4f;
                break;
            case Position.E:
                posRock.x += 4f;
                break;
            case Position.S:
                posRock.z -= 4f;
                break;
            case Position.W:
                posRock.x -= 4f;
                break;

        }
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
}
