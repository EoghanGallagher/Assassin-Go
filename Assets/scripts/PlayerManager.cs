using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (PlayerMover))]
[RequireComponent(typeof (PlayerInput))]
public class PlayerManager : MonoBehaviour 
{

	public PlayerMover playerMover;
	public PlayerInput playerInput;
	// Update is called once per frame

	private void Awake()
	{
		playerMover = GetComponent<PlayerMover>();
		playerInput = GetComponent<PlayerInput>();

		playerInput.InputEnabled = true;

	}
	void Update () 
	{
		if( playerMover.isMoving )
		{
			return;
		}

		playerInput.GetKeyInput();

		if( playerInput.Vert == 0 )
		{
			if( playerInput.Horiz < 0 )
			{
				playerMover.MoveLeft();
			}
			else if( playerInput.Horiz > 0 )
			{
				playerMover.MoveRight();
			}
		}

		if( playerInput.Horiz == 0 )
		{
			if( playerInput.Vert < 0 )
			{
				playerMover.MoveBack();
			}
			else if( playerInput.Vert > 0 )
			{
				playerMover.MoveForward();
			}
		}
	}
}
