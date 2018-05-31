﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeath : MonoBehaviour {
    public Vector3 offscreenOffset = new Vector3(0f, 30f, 20f);
    public Vector3 offscreenOffsetDeath = new Vector3(0f, 10f, 0f);

    Board _board;
    public float deathDelay = 0f;
    public float offscreenDelay = 1f;

    public float iTweenDelay = 0f;
    public iTween.EaseType easeType = iTween.EaseType.easeInQuint;
    public float moveTime = 0.5f;

	void Awake () {
        _board = Object.FindObjectOfType<Board>().GetComponent<Board>();
	}

    public void MoveOffBoard (Vector3 target)
    {
        iTween.MoveTo(gameObject, iTween.Hash(
            "x", target.x,
            "y", target.y,
            "z", target.z,
            "delay", iTweenDelay,
            "easetype", easeType,
            "time", moveTime
        ));
    }

    public void Die ()
    {
        StartCoroutine(DieRoutine());
    }

    IEnumerator DieRoutine ()
    {
        yield return new WaitForSeconds(deathDelay);

        Vector3 offscreenPos = transform.position + offscreenOffset;

        MoveOffBoard(offscreenPos);

        yield return new WaitForSeconds(moveTime + offscreenDelay);

        if (_board.capturePositions.Count != 0 && _board.CurrentCapturePosition < _board.capturePositions.Count)
        {
            Vector3 capturePos = _board.capturePositions[_board.CurrentCapturePosition].position;
            transform.position = capturePos + offscreenOffsetDeath;
            MoveOffBoard(capturePos);
            yield return new WaitForSeconds(moveTime);
            ++_board.CurrentCapturePosition;
            _board.CurrentCapturePosition = Mathf.Clamp(_board.CurrentCapturePosition, 0, _board.capturePositions.Count - 1);
        }
    }
}
