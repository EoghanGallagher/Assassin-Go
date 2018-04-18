using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementType{
	Stationary,
	Patrol,
	Spinner
}
public class EnemyMover : Mover 
{

	public Vector3 directionToMove = new Vector3( 0f, 0f, Board.spacing );

	public MovementType movementType = MovementType.Stationary;

	public float standTime = 1f;

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
		switch( movementType )
		{
			case MovementType.Patrol:
				Patrol();
			break;

			case MovementType.Stationary:
				Stand();
			break;

			case MovementType.Spinner:
				Spin();
			break;
		}
		
	}

	private void Patrol()
	{
		StartCoroutine( PatrolRoutine() );
	}

	IEnumerator PatrolRoutine()
	{
		
		// cache our starting position
        Vector3 startPos = new Vector3(m_currentNode.Coordinate.x, 0f, m_currentNode.Coordinate.y);
		//Vector3 startPos = new Vector3( m_currentNode.Coordinate.x, 0f, m_currentNode.Coordinate.y );

		//One space forward no matter the direction enemy is facing.
		Vector3 newDest = startPos + transform.TransformVector( directionToMove );
		//Vector3 newDest = startPos + transform.TransformVector( directionToMove );

		//Two spaces forward
		Vector3 nextDest = startPos + transform.TransformVector( directionToMove * 2f );
		
		Move( newDest , 0f );

		while( isMoving )
		{
			yield return null;
		}

		if( m_board != null )
		{
			Node newDestNode = m_board.FindNodeAt( newDest );
			Node nextDestNode = m_board.FindNodeAt( nextDest );

			//if the Node two spaces away does not exist OR is not connected to our destination Node...
            if (nextDestNode == null || !newDestNode.LinkedNodes.Contains(nextDestNode))
            {
                // turn to face our original Node and set that as our new destination
                destination = startPos;
                FaceDestination();

                // wait until we are done rotating
                yield return new WaitForSeconds(rotateTime);
            }
		}
		base.finishMovementEvent.Invoke();
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

	private void Spin()
	{
		StartCoroutine( SpinRoutine() );
	}

	IEnumerator SpinRoutine()
	{
		Vector3 localForward = new Vector3( 0f, 0f, Board.spacing );
		destination = transform.TransformVector( localForward * -1f ) + transform.position;

		FaceDestination();

		yield return new WaitForSeconds( rotateTime );

		base.finishMovementEvent.Invoke();
	}

}
