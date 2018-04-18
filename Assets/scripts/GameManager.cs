using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Linq;


[System.Serializable]
public enum Turn
{
	Player,
	Enemy
}


public class GameManager : MonoBehaviour {

	
	Board m_board;
	PlayerManager m_player;

	[SerializeField] private List<EnemyManager> m_enemies;
	[SerializeField] private Turn m_currentTurn = Turn.Player;
	public Turn CurrentTurn { get{ return m_currentTurn; } }
	// Use this for initialization

	public bool HasLevelStarted {get; private set; }
	public bool IsGamePlaying {get; private set; }
	
	public bool IsGameOver {get; private set; }

	public bool HasLevelFinished {get; private set; }

	public float delay = 1f;


	public UnityEvent setupEvent;
	public UnityEvent startLevelEvent;
	public UnityEvent playLevelEvent;
	public UnityEvent endLevelEvent;
	public UnityEvent loseLevelEvent;



	void Awake()
	{
		m_board = GameObject.FindObjectOfType<Board>().GetComponent<Board>();
		m_player = GameObject.FindObjectOfType<PlayerManager>().GetComponent<PlayerManager>();
		
		EnemyManager[] enemies = GameObject.FindObjectsOfType<EnemyManager>() as EnemyManager[];

		m_enemies = enemies.ToList();
	}
	void Start () 
	{
		if( m_board != null && m_board != null )
		{
			StartCoroutine("RunGameLoop");
		}
		else
		{
			Debug.Log("GameManager Error: " + "no player or baord found");
		}
	}

	IEnumerator RunGameLoop()
	{

		Debug.Log("Running Game Loop");
		yield return StartCoroutine("StartLevelRoutine");
		yield return StartCoroutine("PlayLevelRoutine");
		yield return StartCoroutine("EndLevelRoutine");
	}

	IEnumerator StartLevelRoutine()
	{
		
		Debug.Log( "SetUp Event" );
		if( setupEvent != null )
		{
			setupEvent.Invoke();
		} 
		
		Debug.Log("Start Level Co routine");
		m_player.playerInput.InputEnabled = false;
		TogglePlayerInput( false );

		while( !HasLevelStarted )
		{
			//Show start screen
			//User presses button to start
			//HasLevelStarted = true;
			yield return null;
		}

		if( startLevelEvent != null )
		{
			startLevelEvent.Invoke();
		}

		
	}

	//gameplay stage
	IEnumerator PlayLevelRoutine()
	{
		Debug.Log("Play Level Co routine");
		IsGamePlaying = true;
		yield return new WaitForSeconds( delay );
		//m_player.playerInput.InputEnabled = true;
		TogglePlayerInput( true );

		if(playLevelEvent != null)
		{
			playLevelEvent.Invoke();
		}

		while(!IsGameOver)
		{
			//Check for game over condition

			yield return null;
			IsGameOver = IsWinner();

			//win
			//reach the end of the level
			
			

			//lose
			//player dies

			//IsGameover = true

			
		}

		Debug.Log("You woooooooon ----------");
		
	}

	public void LoseLevel()
	{
		StartCoroutine( LoseLevelRoutine() );
	}

	private IEnumerator LoseLevelRoutine()
	{
		IsGameOver = true;
		if( loseLevelEvent != null )
		{
			loseLevelEvent.Invoke();
		}
		
		Debug.Log("You Lost");
		yield return new WaitForSeconds( 2.0f );

		RestartLevel();
	}

	IEnumerator EndLevelRoutine()
	{
		Debug.Log("End Level Co routine");
		//Disable player input
		//m_player.playerInput.InputEnabled = false;
		TogglePlayerInput( false );

		if(endLevelEvent != null)
		{
			endLevelEvent.Invoke();
		}

		//show end screen

		while(!HasLevelFinished)
		{
			//User presses button to continue

			//HasLevelFinished = true

			//HasLevelFinished = true;
			yield return null;
		}

		RestartLevel();
		
		
	}


	void RestartLevel()
	{
		Scene scene = SceneManager.GetActiveScene();
		SceneManager.LoadScene( scene.name );
	}

	public void PlayLevel()
	{
		HasLevelStarted = true;
	}

	public void Test()
	{
		Debug.Log("This works");
	}

	private bool IsWinner()
	{
		if( m_board.PlayerNode != null )
		{
			return( m_board.PlayerNode == m_board.GoalNode );
		}
		
		return false;
	}


	private void TogglePlayerInput( bool b )
	{
		Debug.Log( "Player Input : " + b );
		m_player.playerInput.InputEnabled = b;
	}

	public void ToggleHasLevelFinished( bool b )
	{
		Debug.Log( "Has Level Finished : " + b );
		HasLevelFinished = b;
	}


	private void PlayPlayerTurn()
	{
		m_currentTurn = Turn.Player;
		m_player.IsTurnComplete = false;

		//Allow player to move
	}

	private void PlayEnemyTurn()
	{
		m_currentTurn = Turn.Enemy;

		foreach( EnemyManager enemy in m_enemies )
		{
			if( enemy != null )
			{
				enemy.IsTurnComplete = false;

				//Play each enemies turn
				enemy.PlayTurn();
			}
		}
	}

	private bool IsEnemyTurnComplete()
	{
		foreach( EnemyManager enemy in m_enemies )
		{
			if( !enemy.IsTurnComplete )
			{
				return false;
			}
		}
		return true;
	}

	public void UpdateTurn()
	{
		if( m_currentTurn == Turn.Player && m_player != null )
		{
			if( m_player.IsTurnComplete )
			{
				PlayEnemyTurn();
			}
		}
		else if( m_currentTurn == Turn.Enemy )
		{
			if( IsEnemyTurnComplete() )
			{
				PlayPlayerTurn();
			}
		}
	}
	

}
