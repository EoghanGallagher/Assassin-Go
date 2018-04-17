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
		//Detect player
		m_enemySensor.UpdateSensor();

		//Attack Player
		//Wait
		yield return new WaitForSeconds( 0.1f );
		//Movement
		m_enemyMover.MoveOneTurn();

		//Wait
		
	}
	
}
