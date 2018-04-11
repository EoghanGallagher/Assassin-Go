using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class EndScreen : MonoBehaviour 
{

	[SerializeField] private PostProcessingProfile blurProfile;
	[SerializeField] private PostProcessingProfile normalProfile;

	[SerializeField] private PostProcessingBehaviour cameraPostProcess;


	public void EnableCameraBlur( bool state )
	{
		
		
		
		 if( cameraPostProcess != null && blurProfile != null && normalProfile != null )
		 {
			 cameraPostProcess.profile = ( state ) ? blurProfile : normalProfile;	 
		 }


	}



}
