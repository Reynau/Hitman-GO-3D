using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(EnemyMover))]
[RequireComponent(typeof(EnemySensor))]
[RequireComponent(typeof(EnemyAttack))]

public class EnemyManager : TurnManager {
    EnemyMover _enemyMover;
    EnemySensor _enemySensor;
    EnemyAttack _enemyAttack;
    Board _board;

    bool _isDead = false;
    public bool IsDead { get { return _isDead; } }
    public UnityEvent deathEvent;

    protected override void Awake()
    {
        base.Awake();

        _board = Object.FindObjectOfType<Board>().GetComponent<Board>();
        _enemyMover = GetComponent<EnemyMover>();
        _enemySensor = GetComponent<EnemySensor>();
        _enemyAttack = GetComponent<EnemyAttack>();
    }

    public void PlayTurn ()
    {
        if(_isDead)
        {
            FinishTurn();
            return;
        }

        StartCoroutine(PlayTurnRoutine());
    }

    IEnumerator PlayTurnRoutine ()
    {
        if (_gameManager != null && !_gameManager.IsGameOver)
        {
            _enemySensor.UpdateSensor(_enemyMover.CurrentNode);

            yield return new WaitForSeconds(0.5f);

            if (_enemySensor.FoundPlayer)
            {
                _gameManager.LoseLevel();

                Vector3 playerPosition = new Vector3(_board.PlayerNode.Coordinate.x, 0f, _board.PlayerNode.Coordinate.y);

                _enemyMover.Move(playerPosition, 0f);
                while (_enemyMover.isMoving)
                {
                    yield return null;
                }

                _enemyAttack.Attack();
            }
            else
            {
                _enemyMover.MoveOneTurn();
            }
        }
    }

    public void Die ()
    {
        if (_isDead)
        {
            return;
        }

        _isDead = true;

        if (deathEvent != null)
        {
            deathEvent.Invoke();
        }
    }
}
