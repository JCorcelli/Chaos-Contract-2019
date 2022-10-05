using UnityEngine;
using System.Collections;

public class PrintScreenOnce : MonoBehaviour {

	// Use this for initialization
	void Start () {
		 ScreenCapture.CaptureScreenshot(Application.persistentDataPath + "Screenshot.png" );
	}
	
}
