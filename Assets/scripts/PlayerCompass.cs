using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCompass : MonoBehaviour 
{

	[SerializeField] private GameObject arrowPrefab;

	private List<GameObject> m_arrows = new List<GameObject>();

	[SerializeField] private float scale = 1f;
	[SerializeField] private float startDistance = 0.25f;
	[SerializeField] private float endDistance = 0.5f;
	[SerializeField] private float moveTime = 1f;
	[SerializeField] private iTween.EaseType easeType = iTween.EaseType.easeInOutExpo;
	[SerializeField] private float delay = 0f;



	Board m_board;

	private void Awake()
	{
		m_board = Object.FindObjectOfType<Board>().GetComponent<Board>();
		SetupArrows();
	
	}

	private void SetupArrows()
	{
		if( arrowPrefab == null )
		{
			Debug.LogWarning( "PLAYER COMPASS SETUP ARROWS ERROR: Missing arrow prefab");
			return;
		}

		foreach( Vector2 dir in Board.directions )
		{
			Vector3 dirVector = new Vector3( dir.normalized.x , 0 , dir.normalized.y );
		
			Quaternion rotation = Quaternion.LookRotation( dirVector );

			GameObject arrowInstance = Instantiate( arrowPrefab, transform.position + dirVector * startDistance, rotation );
			arrowInstance.transform.localScale = new Vector3( scale, scale, scale );
			arrowInstance.transform.parent = transform;
			m_arrows.Add( arrowInstance );
		}
	}

	private void MoveArrow( GameObject arrowInstance )
	{
		iTween.MoveBy( arrowInstance, iTween.Hash(
			"z", endDistance - startDistance,
			"looptype", iTween.LoopType.loop,
			"time", moveTime,
			"easetype" , easeType 
		));
	}

	private void MoveArrows()
	{
		foreach( GameObject arrow in m_arrows )
		{
			MoveArrow( arrow );
		}
	}

	public void ShowArrows( bool state )
	{
		if( m_board == null )
		{
			Debug.LogWarning( "PLAYER COMPASS SETUP ARROWS ERROR: Missing arrow prefab");
			return;
		}

		if( m_arrows == null || m_arrows.Count !=  Board.directions.Length )
		{
			Debug.LogWarning( "PLAYER COMPASS SETUP SHOW ARROWS ERROR: No Arrows found");
			return;
		}

		if( m_board.PlayerNode != null )
		{
			for( int i = 0; i < Board.directions.Length; i++ )
			{
				Node neighbour = m_board.PlayerNode.FindNeighbourAt( Board.directions[i] );

				if( neighbour == null || !state )
				{
					m_arrows[i].SetActive( false );
				}
				else
				{
					bool activeState  = m_board.PlayerNode.LinkedNodes.Contains( neighbour );
					m_arrows[i].SetActive( activeState );
				}
			}
		}

		ResetArrows();
		MoveArrows();
	}

	private void ResetArrows()
	{
		for( int i = 0; i < Board.directions.Length; i++ )
		{
			iTween.Stop( m_arrows[i] );
			Vector3 dirVector = new Vector3( Board.directions[i].normalized.x, 0f, Board.directions[i].normalized.y );
			Debug.Log( dirVector );
			m_arrows[i].transform.position = transform.position + dirVector * startDistance;
		}
	}
	
}
