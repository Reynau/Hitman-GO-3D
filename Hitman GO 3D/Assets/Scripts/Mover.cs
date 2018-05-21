using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Mover : MonoBehaviour {
    public Vector3 destination;
    public bool faceDestination = false;
    public bool isMoving = false;
    public iTween.EaseType easeType;

    public float moveSpeed = 1.5f;
    public float iTweenDelay = 0f;
    public float rotateTime = 0.5f;

    protected Board _board;
    protected Node _currentNode;

    public UnityEvent finishMovementEvent;

    protected virtual void Awake()
    {
        _board = Object.FindObjectOfType<Board>().GetComponent<Board>();
    }

    protected virtual void Start()
    {
        UpdateCurrentNode();
    }

    public void Move(Vector3 destinationPos, float delayTime = 0.25f)
    {
        if (isMoving)
        {
            return;
        }

        if (_board != null)
        {
            Node targetNode = _board.FindNodeAt(destinationPos);

            if (targetNode != null && _currentNode != null)
            {
                if (_currentNode.LinkedNodes.Contains(targetNode))
                {
                    StartCoroutine(MoveRoutine(destinationPos, delayTime));
                }
            }
            else
            {
                Debug.LogWarning("MOVER Move Error: Missing targetNode or _currentNode!");
            }
        }
    }

    protected virtual IEnumerator MoveRoutine(Vector3 destinationPos, float delayTime)
    {
        isMoving = true;
        destination = destinationPos;
        if (faceDestination)
        {
            FaceDestination();
            yield return new WaitForSeconds(0.25f);
        }
        yield return new WaitForSeconds(delayTime);
        iTween.MoveTo(gameObject, iTween.Hash(
            "x", destinationPos.x,
            "y", destinationPos.y,
            "z", destinationPos.z,
            "delay", iTweenDelay,
            "easetype", easeType,
            "speed", moveSpeed
        ));

        while (Vector3.Distance(destinationPos, transform.position) > 0.01f)
        {
            yield return null;
        }

        iTween.Stop(gameObject);
        transform.position = destinationPos;
        isMoving = false;

        UpdateCurrentNode();
    }

    public void MoveLeft()
    {
        Vector3 newPosition = transform.position + new Vector3(-Board.spacing, 0f, 0f);
        Move(newPosition, 0f);
    }

    public void MoveRight()
    {
        Vector3 newPosition = transform.position + new Vector3(Board.spacing, 0f, 0f);
        Move(newPosition, 0f);
    }

    public void MoveForward()
    {
        Vector3 newPosition = transform.position + new Vector3(0f, 0f, Board.spacing);
        Move(newPosition, 0f);
    }

    public void MoveBackward()
    {
        Vector3 newPosition = transform.position + new Vector3(0f, 0f, -Board.spacing);
        Move(newPosition, 0f);
    }

    protected void UpdateCurrentNode ()
    {
        if (_board != null)
        {
            _currentNode = _board.FindNodeAt(transform.position);
        }
    }

    protected void FaceDestination ()
    {
        Vector3 movementDir = destination - transform.position;
        Quaternion newRotation = Quaternion.LookRotation(movementDir);
        float newYAngle = newRotation.eulerAngles.y;

        iTween.RotateTo(gameObject, iTween.Hash(
            "y", newYAngle,
            "delay", 0f,
            "easetype", easeType,
            "time", rotateTime
        ));
    }
}
