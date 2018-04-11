using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

	//Store horizontal and vertical input
	private float m_horiz, m_vert;

	//Public property for m_horiz
	public float Horiz{ get{ return m_horiz; } }

	//Public property for m_vert
	public float Vert{ get{ return m_vert; } }

	//Global flag for enabling and disabling user input
	private bool m_inputEnabled = false;

	public bool InputEnabled{ get{ return m_inputEnabled; } set { m_inputEnabled = value; } }
	// Use this for initialization

	//get keyboard input
	public void GetKeyInput()
	{
		//if input is enabled, get the raw axis data from the horizontal and vertical axes
		if( m_inputEnabled )
		{
			m_horiz = Input.GetAxisRaw( Tags.horiz );
			m_vert = Input.GetAxisRaw( Tags.vert );
		}
		else
		{
			m_horiz = 0;
			m_vert = 0;
		}
	}

}
