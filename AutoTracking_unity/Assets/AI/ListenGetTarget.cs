using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListenGetTarget : MonoBehaviour {
	float dic = 0.0f;
	float LisDic = 0.0f;
	bool SW = false;

	public void SetListenSize (float d, float l) {
		dic = d;
		LisDic = l;
		SW = true;
	}
	
	void Update () {
		if (SW) {
			Transform target = Sys.GetPlayer ();
			if (Vector2.Distance (target.position,transform.position) < dic) {
				if (Sys.GetPlayerData ().GetPlayerMoveSpeed () > LisDic) {
					transform.GetComponent<AI> ().SetLookAt ();
				}
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
