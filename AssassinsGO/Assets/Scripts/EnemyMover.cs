using System.Collections;

using UnityEngine;

public class EnemyMover : Mover
{
    public float StandTime = 2f;

    protected override void Awake()
    {
        base.Awake();
        faceDestination = true;
    }

    protected override void Start()
    {
        base.Start();
    }

    public void MoveOneTurn()
    {
        Stand();
    }

    void Stand()
    {
        StartCoroutine(StandRoutine());
    }

    IEnumerator StandRoutine()
    {
        yield return new WaitForSeconds(StandTime);
        finishMovementEvent.Invoke();
    }
}
