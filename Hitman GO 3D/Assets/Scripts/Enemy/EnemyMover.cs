using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementType
{
    Stationary,
    Patrol,
    Spinner
}

public class EnemyMover : Mover {
    public Vector3 directionToMove = new Vector3(0f, 0f, Board.spacing);

    public MovementType movementType = MovementType.Stationary;

    public float standTime = 1f;

    protected override void Awake()
    {
        base.Awake();
        faceDestination = true;
    }

    protected override void Start () {
        base.Start();
	}

    public void MoveOneTurn ()
    {
        switch (movementType)
        {
            case MovementType.Stationary:
                Stand();
                break;
            case MovementType.Patrol:
                Patrol();
                break;
            case MovementType.Spinner:
                Spin();
                break;
        }
    }

    void Stand ()
    {
        StartCoroutine(StandRoutine());
    }

    IEnumerator StandRoutine ()
    {
        yield return new WaitForSeconds(standTime);
        finishMovementEvent.Invoke();
    }

    void Patrol()
    {
        StartCoroutine(PatrolRoutine());
    }

    IEnumerator PatrolRoutine()
    {
        Vector3 startPos = new Vector3(_currentNode.Coordinate.x, 0f, _currentNode.Coordinate.y);
        Vector3 newDest = startPos + transform.TransformDirection(directionToMove);
        Vector3 nextDest = startPos + transform.TransformDirection(directionToMove * 2f);

        Move(newDest, 0f);

        while (isMoving)
        {
            yield return null;
        }
        
        if (_board != null)
        {
            Node newDestNode = _board.FindNodeAt(newDest);
            Node nextDestNode = _board.FindNodeAt(nextDest);

            if (nextDestNode == null || !newDestNode.LinkedNodes.Contains(nextDestNode))
            {
                destination = startPos;
                FaceDestination();

                yield return new WaitForSeconds(rotateTime);
            }
        }

        finishMovementEvent.Invoke();
    }

    void Spin()
    {
        StartCoroutine(SpinRoutine());
    }

    IEnumerator SpinRoutine()
    {
        Vector3 localForward = new Vector3(0f, 0f, Board.spacing);
        destination = transform.position + transform.TransformDirection(localForward) * -1f;
        FaceDestination();

        yield return new WaitForSeconds(rotateTime);

        finishMovementEvent.Invoke();
    }
}
