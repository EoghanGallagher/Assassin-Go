using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour 
{
	public static float spacing = 2f;

	public static readonly Vector2[] directions =
	{
		new Vector2( spacing , 0f ),
		new Vector2( -spacing , 0f ),
		new Vector2( 0f, spacing ),
		new Vector2( 0f, -spacing )
	};

	List<Node> m_allNodes = new List<Node>();
	public List<Node> AllNodes { get{ return m_allNodes; } }


	PlayerMover m_player;
	Node m_playerNode;

	public Node GoalNode{get; private set; } 

	public GameObject goalPrefab;
	public float drawGoalTime = 2f;
	public float drawGoalDelay = 2f;
	public iTween.EaseType drawGoalEaseType = iTween.EaseType.easeOutExpo;



	public Node PlayerNode { get{ return m_playerNode; } }



	void Awake()
	{
		m_player = Object.FindObjectOfType<PlayerMover>().GetComponent<PlayerMover>();
		GetNodeList();

		GoalNode = FindGoalNode();
		
	}

	public void GetNodeList()
	{
		Node[] nList = GameObject.FindObjectsOfType<Node>();
		m_allNodes = new List<Node>( nList );
	}


	public Node FindNodeAt( Vector3 pos )
	{
		Vector2 boardCoord = Utility.Vector2Round( new Vector2( pos.x , pos.z) );

		return m_allNodes.Find( n => n.Coordinate == boardCoord );
	}

	Node FindGoalNode()
	{
		return m_allNodes.Find( n => n.isLevelGoal );
	}

	public Node FindPlayerNode()
	{
		if( m_player != null && !m_player.isMoving )
		{
			return FindNodeAt( m_player.transform.position );
		}
		
		return null;
	}

	public void UpdatePlayerNode()
	{
		m_playerNode = FindPlayerNode();
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = new Color( 0f, 1f, 1f, 0.5f );

		if( m_playerNode != null )
		{
			Gizmos.DrawSphere( m_playerNode.transform.position, 0.2f );
		}

	}

	public void DrawGoal()
	{
		if(goalPrefab != null && GoalNode != null )
		{
			GameObject goalInstance = Instantiate( goalPrefab, GoalNode.transform.position, Quaternion.identity );

			iTween.ScaleFrom( goalInstance , iTween.Hash(
				"scale", Vector3.zero,
				"time", drawGoalTime,
				"delay", drawGoalDelay,
				"easeType", drawGoalEaseType
			));
		}
	}


	public void InitBoard()
	{
		if( PlayerNode != null )
			PlayerNode.InitNode();
		
	}

	
}
