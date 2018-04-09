using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	
	Board m_board;
	PlayerManager m_player;
	// Use this for initialization

	public bool HasLevelStarted {get; private set; }
	public bool IsGamePlaying {get; private set; }
	
	public bool IsGameOver {get; private set; }

	public bool HasLevelFinished {get; private set; }

	public float delay = 1f;

	public UnityEvent startLevelEvent;
	public UnityEvent playLevelEvent;
	public UnityEvent endLevelEvent;

	void Awake()
	{
		m_board = Object.FindObjectOfType<Board>().GetComponent<Board>();
		m_player = Object.FindObjectOfType<PlayerManager>().GetComponent<PlayerManager>();
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
		Debug.Log("Start Level Co routine");
		m_player.playerInput.InputEnabled = false;

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
		m_player.playerInput.InputEnabled = true;

		if(playLevelEvent != null)
		{
			playLevelEvent.Invoke();
		}

		while(!IsGameOver)
		{
			//Check for game over condition

			//win
			//reach the end of the level

			//lose
			//player dies

			//IsGameover = true

			yield return null;
		}

		
	}

	IEnumerator EndLevelRoutine()
	{
		Debug.Log("End Level Co routine");
		//Disable player input
		m_player.playerInput.InputEnabled = false;

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
	

}
