using UnityEngine;
using System.Collections;

public class MovingCam : MonoBehaviour {
	
	float camSpeed = 20f;
	float xSpeed = 125.0f;
	float ySpeed = 50.0f;
	float x = 0.0f;
	float y = 0.0f;
	// Use this for initialization
	void Start () {
		Vector3 angles = transform.eulerAngles; 
		x = angles.y; 
		y = angles.x; 
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.W)){
			transform.position += transform.forward * camSpeed * Time.deltaTime;
		}
		else if (Input.GetKey(KeyCode.S)){
			transform.position -= transform.forward * camSpeed * Time.deltaTime;
		}
		if(Input.GetKey(KeyCode.A)){
			transform.position -= transform.right * camSpeed * Time.deltaTime;
		}
		else if (Input.GetKey(KeyCode.D)){
			transform.position += transform.right * camSpeed * Time.deltaTime; 
		}
		if(Input.GetMouseButton(0)){
			x += Input.GetAxis("Mouse X") * xSpeed * 0.02f; 
			y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f; 
			Quaternion rotation = Quaternion.Euler(y, x, 0); 
			transform.rotation = rotation;
		}
	}
}