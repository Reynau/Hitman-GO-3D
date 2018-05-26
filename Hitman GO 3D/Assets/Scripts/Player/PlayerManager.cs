using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerMover))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerDeath))]
[RequireComponent(typeof(PlayerCompass))]

public class PlayerManager : TurnManager
{
    public PlayerMover playerMover;
    public PlayerInput playerInput;

    Board _board;

    public int healthyCount = 0;
    public int fastFoodCount = 0;

    public UnityEvent deathEvent;

    protected override void Awake()
    {
        base.Awake();

        _board = Object.FindObjectOfType<Board>().GetComponent<Board>();

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

    public void Die ()
    {
        if (deathEvent != null)
        {
            deathEvent.Invoke();
        }
    }
    
    void CaptureEnemies ()
    {
        if (_board != null)
        {
            List<EnemyManager> enemies = _board.FindEnemiesAt(_board.PlayerNode);

            if (enemies.Count != 0)
            {
                foreach (EnemyManager enemy in enemies)
                {
                    if (enemy != null)
                    {
                        enemy.Die();
                    }
                }
            }
        }
    }

    void ActivateActivables ()
    {
        if (_board != null)
        {
            Activable activable = _board.FindActivableAt(_board.PlayerNode);
            if (activable != null)
            {
                activable.Activate();
            }
        }
        else
        {
            Debug.LogWarning("PLAYERMANAGER ActivateActivables Error: _board is null");
        }
    }

    void PickCollectibles()
    {
        if (_board != null)
        {
            Collectible collectible = _board.FindCollectibleAt(_board.PlayerNode);
            if (collectible != null)
            {
                if (collectible.name == "Appl" || collectible.name == "Bana" || collectible.name == "Oran")
                {
                    healthyCount++;
                }
                else
                {
                    fastFoodCount++;
                }
                collectible.PickCollectible();
            }
        }
        else
        {
            Debug.LogWarning("PLAYERMANAGER PickCollectible Error: _board is null");
        }
    }

    void PickSniper()
    {
        if (_board != null)
        {
            Sniper sniper = _board.FindSniperAt(_board.PlayerNode);
            if (sniper != null)
            {
                sniper.PickSniper();
                playerMover.KillNearestEnemy();
            }
        }
        else
        {
            Debug.LogWarning("PLAYERMANAGER PickSniper Error: _board is null");
        }
    }

    public override void FinishTurn()
    {
        ActivateActivables();
        PickCollectibles();
        PickSniper();
        CaptureEnemies();
        base.FinishTurn();
    }
}
