using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GotoPath : MonoBehaviour {
	Vector3 Target;
	float MoveSpeed = 1.0f;
	float Dic = 0.01f;
	bool SW = false;
	int NextP = 0;

	public void SetPath (Vector3 t) {
		Target = t;
		SW = true;
	}

	public void SetPath (Vector3 t, int n) {
		Target = t;
		NextP = n;
		SW = true;
	}
	
	void Move () {
		if (Vector2.Distance (transform.localPosition,Target) > Dic) {
			transform.position = Vector2.MoveTowards(transform.localPosition,Target,Sys.TimedeltaTime * MoveSpeed);
		}
		else {
			SW = false;
			transform.localPosition = Target;
			if (NextP == 0) this.gameObject.GetComponent<AI> ().CheckCanNextPath ();
			else if (NextP == 1) {this.gameObject.GetComponent<AI> ().GoBackStopLook ();}
		}
	}
	
	void Update () {
		if (SW) Move ();
	}
}
