using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMover))]
[RequireComponent(typeof(PlayerInput))]

public class PlayerManager : TurnManager
{
    public PlayerMover playerMover;
    public PlayerInput playerInput;

    protected override void Awake()
    {
        base.Awake();

        playerMover = GetComponent<PlayerMover>();
        playerInput = GetComponent<PlayerInput>();
        playerInput.InputEnabled = true;
    }

    void Update () {
		if (playerMover.isMoving || _gameManager.CurrentTurn != Turn.Player)
        {
            return;
        }

        playerInput.GetKeyInput();

        if (playerInput.V == 0f)
        {
            if (playerInput.H < 0f)
            {
                playerMover.MoveLeft();
            }
            else if (playerInput.H > 0f)
            {
                playerMover.MoveRight();
            }
        }
        else if (playerInput.H == 0f)
        {
            if (playerInput.V < 0f)
            {
                playerMover.MoveBackward();
            }
            else if (playerInput.V > 0f)
            {
                playerMover.MoveForward();
            }
        }
	}
}
