using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
	//(x,z) coordinate on Board
	Vector2 m_coordinate;
	//Public getter that rounds m_coordiante
	public Vector2 Coordinate { get { return Utility.Vector2Round( m_coordinate ); } }

	List<Node> m_neighbourNodes = new List<Node>();
	public List<Node> NeighbourNodes { get { return m_neighbourNodes; } }


	//List of nodes that already have links
	List<Node> m_linkedNodes = new List<Node>();
	public List<Node> LinkedNodes{ get { return m_linkedNodes; } }

	Board m_board;
	
	//reference to mesh to display the node
	public GameObject geometry;

	public GameObject linkPrefab;
	
	//Time scale for iTween animation to play.
	public float scaleTime = 0.3f;

	//ease in-out iTween animation
	public iTween.EaseType easeType = iTween.EaseType.easeInExpo;

	//Delay time before animation
	public float delay;

	bool m_isInitialized = false;

	private Transform _transform;

	public LayerMask obstacleLayer;

	public bool isLevelGoal = false;


	void Awake()
	{
		m_board = Object.FindObjectOfType<Board>();
		m_coordinate = new Vector2( transform.position.x , transform.position.z );
	}

	// Use this for initialization
	void Start () 
	{
		_transform = transform;
		
		if ( geometry != null )
		{
			geometry.transform.localScale = Vector3.zero;


			if( m_board != null )
			{
				m_neighbourNodes = FindNeighbours( m_board.AllNodes );
			}
		} 
	}

	public void ShowGeometry()
	{
		if ( geometry != null )
		{
			iTween.ScaleTo( geometry, iTween.Hash(
				"time", scaleTime,
				"scale", Vector3.one,
				"easetype", easeType,
				"delay", delay
			) );
		}
	}

	public List<Node> FindNeighbours( List<Node> nodes )
	{
		List<Node> nList = new List<Node>();

		foreach( Vector2 dir in Board.directions )
		{
			Node foundNeighbour = nodes.Find( n => n.Coordinate == Coordinate + dir );

			if( foundNeighbour != null && !nList.Contains( foundNeighbour ) )
			{
				nList.Add( foundNeighbour );
			}
		}
		
		return nList;
	}


	public void InitNode()
	{
		if( !m_isInitialized )
		{
			ShowGeometry();
			InitNeighbours();
			m_isInitialized = true;
		}
	}

	void InitNeighbours()
	{
		StartCoroutine( InitNeighboursRoutine() );
	}

	IEnumerator InitNeighboursRoutine()
	{
		yield return new WaitForSeconds( delay );
		
		foreach( Node n in m_neighbourNodes )
		{
			if( !m_linkedNodes.Contains( n ) )
			{

				Obstacle obstacle = FindObstacle( n );

				if( obstacle == null)
				{	
					LinkNode( n );
					n.InitNode();
				}
			}

		}
	}

	void LinkNode( Node targetNode )
	{
		if( linkPrefab != null )
		{
			//Create instance of link prefab ( Clone  )
			GameObject linkInstance = Instantiate( linkPrefab , _transform.position, Quaternion.identity );
			
			//Parent clone link to the node
			linkInstance.transform.parent = _transform;

			//Get access to the Link script
			Link link = linkInstance.GetComponent<Link>();
			
			//Draw a line using link between the current node and the target node
			if( link != null )
				link.DrawLink( _transform.position, targetNode.transform.position );
			
			if( !m_linkedNodes.Contains( targetNode ) )
				m_linkedNodes.Add( targetNode );

			if( !targetNode.LinkedNodes.Contains( this ) )	
				targetNode.LinkedNodes.Add( this );

		}
	}

	Obstacle FindObstacle( Node targetNode )  
	{
		Vector3 checkDirection = targetNode.transform.position - transform.position;
		RaycastHit raycastHit;

		if( Physics.Raycast(transform.position, checkDirection, out raycastHit, Board.spacing + 0.1f, obstacleLayer ))
		{
			//Debug.Log( "NODE FindObstacle: Hit an obstacle from " + this.name + " to " + targetNode.name );
			return raycastHit.collider.GetComponent<Obstacle>();
		}

		return null;
	}

	
}
