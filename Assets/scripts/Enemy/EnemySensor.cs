using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySensor : MonoBehaviour 
{

	[SerializeField] private Vector3 directionToSearch = new Vector3( 0f, 0f, 2f );

	[SerializeField] private Node m_nodeToSearch;
	private Board m_board;
	
	[SerializeField] private bool m_foundPlayer = false;
	public bool FoundPlayer { get{ return m_foundPlayer; } }
	

	// Use this for initialization
	private void Awake () 
	{
		m_board = Object.FindObjectOfType<Board>().GetComponent<Board>();
	}


	//Testing only
	// private void Update()
	// {
	// 	UpdateSensor();
	// }

	public void UpdateSensor()
	{
		Debug.Log( "UPDATING SENSOR" );

		Vector3 worldSpacePositionToSearch = transform.TransformVector( directionToSearch ) + transform.position;
		Debug.Log( worldSpacePositionToSearch );
		
		if( m_board != null )
		{
			m_nodeToSearch = m_board.FindNodeAt( worldSpacePositionToSearch );

			if( m_nodeToSearch == m_board.PlayerNode )
			{
				m_foundPlayer = true;
			}
		}
	}



}
