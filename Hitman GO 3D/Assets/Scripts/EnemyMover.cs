using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : Mover {
    protected override void Awake()
    {
        base.Awake();
        faceDestination = true;
    }

    protected override void Start () {
        base.Start();
        StartCoroutine(TestMovementRoutine());
	}
	
    IEnumerator TestMovementRoutine ()
    {
        yield return new WaitForSeconds(5f);
        MoveForward();

        yield return new WaitForSeconds(2f);
        MoveRight();

        yield return new WaitForSeconds(2f);
        MoveLeft();

        yield return new WaitForSeconds(2f);
        MoveBackward();
    }
}
