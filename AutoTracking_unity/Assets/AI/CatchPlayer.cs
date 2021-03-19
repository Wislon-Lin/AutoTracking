using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchPlayer : MonoBehaviour {
	float MoveSpeed = 1.0f;
	float Dic = 0.05f;

	void Move () {
		if (Vector2.Distance (transform.localPosition,Sys.GetPlayer ().transform.position) > Dic) {
			transform.position = Vector2.MoveTowards(transform.localPosition,Sys.GetPlayer ().transform.position,Sys.TimedeltaTime * MoveSpeed);
		}
		else {
			//Debug.Log ("Catch");
		}
	}
	
	void Update () {
		Move ();
	}
}
