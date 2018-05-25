using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : Mover {
    PlayerCompass _playerCompass;

    protected override void Awake()
    {
        base.Awake(); // Call parent Awake method
        _playerCompass = Object.FindObjectOfType<PlayerCompass>().GetComponent<PlayerCompass>();
    }

    protected override void Start()
    {
        base.Start(); // Call parent Start method
        UpdateBoard();
    }

    protected override IEnumerator MoveRoutine(Vector3 destinationPos, float delayTime)
    {
        if (_playerCompass != null)
        {
            _playerCompass.ShowArrows(false);
        }

        faceDestination = true;
        yield return StartCoroutine(base.MoveRoutine(destinationPos, delayTime));

        UpdateBoard();

        if (_playerCompass != null)
        {
            _playerCompass.ShowArrows(true);
        }

        finishMovementEvent.Invoke();
    }

    void UpdateBoard ()
    {
        if (_board != null)
        {
            _board.UpdatePlayerNode();
        }
    }
}
