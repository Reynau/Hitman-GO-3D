using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Sniper : Activable
{
    public float delay;

    Board _board;
    PlayerMover _player_mover;

    private void Awake()
    {
        _board = Object.FindObjectOfType<Board>().GetComponent<Board>();
        _player_mover = Object.FindObjectOfType<PlayerMover>().GetComponent<PlayerMover>();
    }
    
    public override void Activate()
    {
        if (!_active)
        {
            StartCoroutine(PickSniperRoutine());
            _active = true;
        }
    }

    IEnumerator PickSniperRoutine()
    {
        yield return new WaitForSeconds(delay);
        activateActivableEvent.Invoke();
        EnemyManager nearestEnemy = _board.FindNearestEnemy();

        if (nearestEnemy != null)
        {
            EnemyMover nearestMoverDest = nearestEnemy.GetComponent<EnemyMover>();
            _player_mover.destination = nearestMoverDest.CurrentNode.transform.position;
            _player_mover.FaceDestination();
            // Play shot sound
            nearestEnemy.Die();
        }
    }
}
