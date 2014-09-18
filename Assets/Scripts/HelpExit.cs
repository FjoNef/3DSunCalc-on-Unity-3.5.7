using UnityEngine;
using System.Collections;

public class HelpExit : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.Escape)) {
			Application.Quit();
		}
	}
	
	void OnGUI() {
		GUIContent tip = new GUIContent ("Help", "W,A,S,D to move.\nHold Left mous button to rotate the camera.\nPress Esc to exit.");
		GUI.Box (new Rect (Screen.width - 110, 2, 100, 30), tip);
		GUI.Label (new Rect (Screen.width - 260, 34, 280, 50), GUI.tooltip);
	}
}
