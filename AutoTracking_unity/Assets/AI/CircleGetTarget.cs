using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleGetTarget : MonoBehaviour {
	float dic = 0.0f;
	bool SW = false;

	public void SetCircleSize (float d) {
		dic = d;
		SW = true;
	}
	
	void Update () {
		if (SW) {
			Transform target = Sys.GetPlayer ();
			if (Vector2.Distance (target.position,transform.position) < dic) {
				if (!Sys.GetPlayerData ().Hide)
					transform.GetComponent<AI> ().SetLookAt ();
				else {
					transform.GetComponent<AI> ().SetNotLookAt ();
				}
			}
			else {
				transform.GetComponent<AI> ().SetNotLookAt ();
			}
		}
	}
}
