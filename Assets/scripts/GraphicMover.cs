using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GraphicMoverMode
{
	MoveTo,
	ScaleTo,
	MoveFrom
}
public class GraphicMover : MonoBehaviour 
{

	public GraphicMoverMode mode;

	[SerializeField] private Transform startXform;
	[SerializeField] private Transform endXform;

	[SerializeField] private float moveTime = 1f;
	[SerializeField] private float delay = 0f;
	[SerializeField] private iTween.LoopType loopType = iTween.LoopType.none;
	[SerializeField] private iTween.EaseType easeType = iTween.EaseType.easeOutExpo;

	void Awake()
	{
		if( endXform == null )
		{
			Debug.Log("End Xform is equal to null");
			endXform = new GameObject( gameObject.name + "XformEnd" ).transform;
			endXform.position = transform.position;
			endXform.rotation = transform.rotation;
			endXform.localScale = transform.localScale;
		}

		if( startXform == null )
		{
			startXform = new GameObject( gameObject.name + "XformStart" ).transform;
			startXform.position = transform.position;
			startXform.rotation = transform.rotation;
			startXform.localScale = transform.localScale;
		}

		Reset();

	
	}

	public void Reset()
	{
		switch( mode )
		{
			case GraphicMoverMode.MoveTo:
				
				if(	startXform != null )
				{
					transform.position = startXform.position;
				}

			break;

			case GraphicMoverMode.MoveFrom:
				
				if( endXform != null )
				{
					transform.position = endXform.position;
				}

			break;

			case GraphicMoverMode.ScaleTo:
				
				if(startXform != null)
				{
					transform.localScale = startXform.localScale;
				}

			break;	
		}
	}

	public void Move()
	{
		switch( mode )
		{
			case GraphicMoverMode.MoveTo:
				iTween.MoveTo( gameObject, iTween.Hash(
					"position" , endXform.position,
					"time", moveTime,
					"delay", delay,
					"easetype", easeType,
					"loopType", loopType 
				) );
			break;

			case GraphicMoverMode.ScaleTo:
					iTween.ScaleTo( gameObject, iTween.Hash(
					"scale" , endXform.localScale,
					"time", moveTime,
					"delay", delay,
					"easetype", easeType,
					"loopType", loopType 
				) );
			break;

			case GraphicMoverMode.MoveFrom:
					Debug.Log("Moving From");
					iTween.MoveFrom( gameObject, iTween.Hash(
					"position" , startXform.position,
					"time", moveTime,
					"delay", delay,
					"easetype", easeType,
					"loopType", loopType 
				) );
			break;	
		}
	}
	

	
}
