using UnityEngine;
using System.Collections;
using System;
using SunPos;

public class Position : MonoBehaviour {
	
	public double longitude;
	public double latitude;
	public int year;
	public int month;
	public int day;
	public int hour;
	public int minut;
	public int second;
	
	void Start(){
		renderer.material.color = Color.yellow;
	}
	
	void Update () {
		SunCoord Sun = new SunCoord ();
		double[] result = Sun.Calculate (latitude, longitude, year, month, day, hour, minut, second);
		this.transform.position = new Vector3 ((float)(10.0 * Math.Sin (result [1] * Math.PI / 180.0) * Math.Sin (result [2] * Math.PI / 180.0)),
		                                       (float)(10.0 * Math.Cos (result [1] * Math.PI / 180.0)), (float)(10.0 * Math.Sin (result [1] * Math.PI / 180.0) * Math.Cos (result [2] * Math.PI / 180.0)));
	}
}