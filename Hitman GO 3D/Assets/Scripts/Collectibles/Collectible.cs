using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum CollectibleType
{
    FastFood,
    HealthyFood
}

public class Collectible : MonoBehaviour
{
    Board _board;
    Camera _cam;

    public CollectibleType type;

    public bool isPicked = false;

    public float pickDelay = 0f;
    public float offscreenDelay = 0.5f;

    public float iTweenDelayMoveOffBoard = 0f;
    public iTween.EaseType easeType = iTween.EaseType.easeInOutQuint;
    public float moveTime = 1f;

    public Animator collectibleAnimController;

    public string collectiblePickTrigger = "isPicked";

    void Awake()
    {
        _board = Object.FindObjectOfType<Board>().GetComponent<Board>();
        _cam = Object.FindObjectOfType<Camera>().GetComponent<Camera>();
    }

    public void MoveOffBoard(Vector3 target)
    {
        iTween.MoveTo(gameObject, iTween.Hash(
            "x", target.x,
            "y", target.y,
            "z", target.z,
            "delay", iTweenDelayMoveOffBoard,
            "easetype", easeType,
            "time", moveTime
        ));
    }

    public void PickCollectible()
    {
        if (!isPicked)
        {
            StartCoroutine(PickRoutine());
            isPicked = true;
        }
    }

    IEnumerator PickRoutine()
    {
        yield return new WaitForSeconds(pickDelay);

        // Change to animation with only rotation in Y axis
        if (collectibleAnimController != null)
        {
            collectibleAnimController.SetTrigger(collectiblePickTrigger);
        }

        Vector3 offscreenPos = _cam.transform.position + _cam.transform.forward;
        offscreenPos.y += _cam.transform.forward.y * 0.7f;

        MoveOffBoard(offscreenPos);

        yield return new WaitForSeconds(moveTime + offscreenDelay);

        if (_board.capturedCollectiblePositions.Count != 0 && _board.CurrentCapturedCollectiblePosition < _board.capturedCollectiblePositions.Count)
        {
            Vector3 capturedCollectiblePos = _board.capturedCollectiblePositions[_board.CurrentCapturedCollectiblePosition].position;
            transform.position = offscreenPos;
            MoveOffBoard(capturedCollectiblePos);
            yield return new WaitForSeconds(moveTime);
            ++_board.CurrentCapturedCollectiblePosition;
            _board.CurrentCapturedCollectiblePosition = Mathf.Clamp(_board.CurrentCapturedCollectiblePosition, 0, _board.capturedCollectiblePositions.Count - 1);
        }
    }
}
