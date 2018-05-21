using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyMover))]
[RequireComponent(typeof(EnemySensor))]

public class EnemyManager : TurnManager {
    EnemyMover _enemyMover;
    EnemySensor _enemySensor;
    Board _board;

    protected override void Awake()
    {
        base.Awake();

        _board = Object.FindObjectOfType<Board>().GetComponent<Board>();
        _enemyMover = GetComponent<EnemyMover>();
        _enemySensor = GetComponent<EnemySensor>();
    }

    public void PlayTurn ()
    {
        StartCoroutine(PlayTurnRoutine());
    }

    IEnumerator PlayTurnRoutine ()
    {
        if (_gameManager != null)
        {
            _enemySensor.UpdateSensor();

            yield return new WaitForSeconds(0.5f);

            if (_enemySensor.FoundPlayer)
            {


                _gameManager.LoseLevel();
            }
            else
            {
                _enemyMover.MoveOneTurn();
            }
        }
    }
}
