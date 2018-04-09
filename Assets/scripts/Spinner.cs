using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour {


	[SerializeField] private float rotateSpeed = 20f;
	// Use this for initialization
	void Start () 
	{
		iTween.RotateBy( gameObject , iTween.Hash(
			"y", 360f,
			"loopType", iTween.LoopType.loop,
			"speed", rotateSpeed,
			"easetype", iTween.EaseType.linear
		) );
	}
	
	
}
