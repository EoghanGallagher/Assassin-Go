using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Mover : MonoBehaviour 
{
	//Where the player is currently headed
	[SerializeField] private Vector3 destination;

	[SerializeField] protected bool faceDestination = false;

	//is player currently moving
	public bool isMoving = false;

	//what easetype to use for tweening
	public iTween.EaseType easeType = iTween.EaseType.easeInOutExpo;

	//how fast we move
	public float moveSpeed = 1.5f;

	public float rotateTime = 0.5f;

	public float rotateSpeed = 360f;
	
	//delay before calling itween
	public float iTweenDelay = 0f;

	protected Board m_board;

	protected Node m_currentNode;

	private Transform _transform;
	// Use this for initialization

	public UnityEvent finishMovementEvent;

	protected virtual void Awake()
	{
		m_board = Object.FindObjectOfType<Board>().GetComponent<Board>();

		if( m_board == null )
		{
			Debug.Log( "Board Object or script not found" );
			return;
		}

	}

	protected virtual void Start () 
	{
		_transform = transform;
		//StartCoroutine( "Test" );

		UpdateCurrentNode();

	}

	IEnumerator Test()
	{
		yield return new WaitForSeconds( 2.0f );
		MoveRight();
		yield return new WaitForSeconds( 2.0f );
		MoveRight();
		yield return new WaitForSeconds( 2.0f );
		MoveForward();
		yield return new WaitForSeconds( 2.0f );
		MoveBack();	
	}
	

	//Public method that invokes the move routine
	public void Move( Vector3 desitnationPos, float delayTime )
	{

		if(isMoving)
		{
			return;
		}
		
		if( m_board != null )
		{
			//Check if a node exists at the destination position.
			Node targetNode = m_board.FindNodeAt( desitnationPos );

			//Only move if there is a node and the current node exists
			if( targetNode != null && m_currentNode != null )
			{
				//Only move if the current node is linked to the target node
				if( m_currentNode.LinkedNodes.Contains( targetNode ) )
				{
					//Start move co routine
					StartCoroutine( MoveRoutine( desitnationPos, delayTime ) );
				}
				else
				{
					Debug.Log( "Mover: " + m_currentNode + " is not connected to target node" );
				}
			}
			
		
		}
		
	}

	protected virtual IEnumerator MoveRoutine( Vector3 destinationPos , float delayTime )
	{
		isMoving = true;

		destination = destinationPos;

		if( faceDestination )
		{
			FaceDestination();
			yield return new WaitForSeconds( 0.25f );
		}

		yield return new WaitForSeconds( delayTime );

		iTween.MoveTo( gameObject, iTween.Hash(
			"x", destinationPos.x,
			"y", destinationPos.y,
			"z", destinationPos.z,
			"delay", iTweenDelay,
			"easetype", easeType,
			"speed", moveSpeed

		));

		while( Vector3.Distance( destinationPos, _transform.position ) > 0.01f )
		{
			yield return null;
		}

		iTween.Stop( gameObject );
		_transform.position = destinationPos;
		isMoving = false;

		UpdateCurrentNode();

	}

	public void MoveLeft()
	{
		Vector3 newPosition = transform.position + new Vector3( -Board.spacing, 0f, 0f );
		Move( newPosition , 0 );
	}

	public void MoveRight()
	{
		Vector3 newPosition = transform.position + new Vector3( Board.spacing, 0f, 0f );
		Move( newPosition , 0 );
	}

	public void MoveForward()
	{
		Vector3 newPosition = transform.position + new Vector3( 0f, 0f, Board.spacing );
		Move( newPosition , 0 );
	}

	public void MoveBack()
	{
		Vector3 newPosition = transform.position + new Vector3( 0f, 0f, -Board.spacing );
		Move( newPosition , 0 );
	}

	protected void UpdateCurrentNode()
	{
		if( m_board != null )
		{
			m_currentNode = m_board.FindNodeAt( transform.position );
		}
	}


	//Rotate Object to face movement direction
	private void FaceDestination()
	{
		Vector3 relativePosition = destination - transform.position;

		Quaternion newRotation = Quaternion.LookRotation( relativePosition, Vector3.up );

		float newY = newRotation.eulerAngles.y;

		iTween.RotateTo( gameObject, iTween.Hash(
			"y", newY,
			"delay", 0f,
			"easetype", easeType,
			"time", rotateTime
		 ));
	}


}
