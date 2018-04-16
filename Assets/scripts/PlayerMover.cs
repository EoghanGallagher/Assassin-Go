using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour {

	[SerializeField]
	private Vector3 destintation;

	private PlayerCompass m_playerCompass;
	
	
	public bool isMoving = false;

	public iTween.EaseType easeType;

	public float moveSpeed = 1.5f;
	public float delay = 0f;
	public float iTweenDelay = 0f;

	Board m_board;

	private Transform _transform;
	// Use this for initialization
	void Awake()
	{
		m_board = Object.FindObjectOfType<Board>().GetComponent<Board>();

		if( m_board == null )
		{
			Debug.Log( "Board Object or script not found" );
			return;
		}

		m_playerCompass = Object.FindObjectOfType<PlayerCompass>().GetComponent<PlayerCompass>();

		if( m_playerCompass == null )
		{
			Debug.Log( "PlayerCompass Object or script not found" );
			return;
		}
	}

	void Start () 
	{
		_transform = transform;
		//StartCoroutine( "Test" );

		UpdateBoard();

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
	
	public void Move( Vector3 desitnationPos, float delayTime )
	{
		
		if( m_board != null )
		{
			Node targetNode = m_board.FindNodeAt( desitnationPos );

			if( targetNode != null && m_board.PlayerNode.LinkedNodes.Contains( targetNode ) )
				StartCoroutine( MoveRoutine( desitnationPos, delayTime ) );
		}
		
	}

	private IEnumerator MoveRoutine( Vector3 destinationPos , float delayTime )
	{
		isMoving = true;

		if( m_playerCompass != null )
		{
			m_playerCompass.ShowArrows( false );
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

		UpdateBoard();


		if( m_playerCompass != null )
		{
			m_playerCompass.ShowArrows( true );
		}
		
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

	public void UpdateBoard()
	{
		if( m_board != null )
		{
			m_board.UpdatePlayerNode();
		}
	}
}
