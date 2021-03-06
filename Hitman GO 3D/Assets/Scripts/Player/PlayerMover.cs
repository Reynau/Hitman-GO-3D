﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : Mover {
    public PlayerCompass playerCompass;

    protected override void Awake()
    {
        base.Awake(); // Call parent Awake method
        playerCompass = Object.FindObjectOfType<PlayerCompass>().GetComponent<PlayerCompass>();
    }

    protected override void Start()
    {
        base.Start(); // Call parent Start method
        UpdateBoard();
    }

    protected override IEnumerator MoveRoutine(Vector3 destinationPos, float delayTime)
    {
        if (playerCompass != null)
        {
            playerCompass.ShowArrows(false);
        }

        faceDestination = true;
        yield return StartCoroutine(base.MoveRoutine(destinationPos, delayTime));

        UpdateBoard();        

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
