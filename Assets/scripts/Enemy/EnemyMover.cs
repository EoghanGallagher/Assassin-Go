using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : Mover {


	[SerializeField] private float standTime = 0.25f;

	protected override void Awake()
	{
		base.Awake();
		faceDestination = true;
	}
	
	
	protected override void Start () 
	{
		base.Start();
		//StartCoroutine( TestMovementRoutine() ); 
	}

	public void MoveOneTurn()
	{
		Stand();
	}

	private void Stand()
	{
		StartCoroutine( StandRoutine() );
	}

	IEnumerator StandRoutine()
	{
		yield return new WaitForSeconds( standTime );
		base.finishMovementEvent.Invoke();
	}



	IEnumerator TestMovementRoutine()
	{
		yield return new WaitForSeconds(9f);
		MoveForward();

		yield return new WaitForSeconds(2f);
		MoveRight();

		yield return new WaitForSeconds(2f);
		MoveBack();

		yield return new WaitForSeconds(2f);
		MoveRight();
	}

	
}
