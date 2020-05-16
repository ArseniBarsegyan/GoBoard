using System.Collections;

using UnityEngine;

public class EnemyMover : Mover
{
    protected override void Awake()
    {
        base.Awake();
        faceDestination = true;
    }

    protected override void Start()
    {
        base.Start();
        // StartCoroutine(TestMovementRoutine());
    }

    IEnumerator TestMovementRoutine()
    {
        yield return new WaitForSeconds(5);
        MoveForward();

        yield return new WaitForSeconds(2);
        MoveRight();

        yield return new WaitForSeconds(2);
        MoveForward();

        yield return new WaitForSeconds(2);
        MoveBackward();

        yield return new WaitForSeconds(2);
        MoveBackward();
    }
}
