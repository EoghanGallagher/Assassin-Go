using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent( typeof( EnemyMover ) )]
[RequireComponent( typeof( EnemySensor ) )]
public class EnemyManager : TurnManager 
{
	private EnemyMover m_enemyMover;
	private EnemySensor m_enemySensor;
	private Board m_board;

	protected override void Awake()
	{
		base.Awake();

		m_board = Object.FindObjectOfType<Board>().GetComponent<Board>();
		m_enemyMover = GetComponent<EnemyMover>();
		m_enemySensor = GetComponent<EnemySensor>();
	}

	public void PlayTurn()
	{
		StartCoroutine( PlayTurnRoutine() );
	}

	IEnumerator PlayTurnRoutine()
	{
		
		if( m_gameManager != null && !m_gameManager.IsGameOver )
		{
			//Detect player
			m_enemySensor.UpdateSensor();

			//Wait
			yield return new WaitForSeconds( 0f );

			if( m_enemySensor.FoundPlayer )
			{
				//Found Player 
				Debug.Log("Found Player");
				//Attack

				//notify Gamemanager to lose the level
				m_gameManager.LoseLevel();
			}
			else
			{
				//No Player Found
				//Move
				m_enemyMover.MoveOneTurn();
			}
		}
		
	}
	
}
