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

        finishMovementEvent.Invoke();

        yield return new WaitForSeconds(1.7f);
        if (_playerCompass != null)
        {
            _playerCompass.ShowArrows(true);
        }
    }

    void UpdateBoard ()
    {
        if (_board != null)
        {
            _board.UpdatePlayerNode();
        }
    }

    public void KillNearestEnemy()
    {
        EnemyManager[] enemies = Object.FindObjectsOfType<EnemyManager>() as EnemyManager[];
        EnemyManager nearestEnemy = null;

        foreach (EnemyManager enemy in enemies)
        {
            if (nearestEnemy == null)
            {
                nearestEnemy = enemy;
            }
            else
            {
                EnemyMover mover = enemy.GetComponent<EnemyMover>();
                EnemyMover nearestMover = nearestEnemy.GetComponent<EnemyMover>();
                if (CalculateDistance(mover.CurrentNode) < CalculateDistance(nearestMover.CurrentNode))
                {
                    nearestEnemy = enemy;
                }
            }
        }
        if (nearestEnemy != null)
        {
            EnemyMover nearestMoverDest = nearestEnemy.GetComponent<EnemyMover>();
            destination = nearestMoverDest.CurrentNode.transform.position;
            FaceDestination();
            nearestEnemy.Die();
        }
    }

    double CalculateDistance(Node target)
    {
        return System.Math.Sqrt(System.Math.Pow((target.transform.position.x - transform.position.x), 2) + (System.Math.Pow((target.transform.position.z - transform.position.z), 2)));
    }
}
