using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchGotoPath : MonoBehaviour {
	Vector3 Target;
	List<Vector3> PathList = new List<Vector3> ();
	float MoveSpeed = 1.0f;
	int GotoCount = 0;
	bool SW = false;

	public void SetScarch (Vector3 t) {
		Target = t;
		PathList = Sys.GetSys.GetAIPath.SetStartAndEndPos (transform.localPosition,Target);
		this.gameObject.GetComponent<AI> ().LookAtPlayer2D (PathList[GotoCount]);
		SW = true;
	}

	void OverPath () {
		this.gameObject.GetComponent<AI> ().SearchGotoOver ();
	}

	void Move () {
		if (Vector2.Distance (transform.localPosition,PathList[GotoCount]) > 0.01f) {
			transform.position = Vector2.MoveTowards(transform.localPosition,PathList[GotoCount],Sys.TimedeltaTime * MoveSpeed);
		}
		else {
			transform.localPosition = PathList[GotoCount];
			int ck = GotoCount + 1;
			if (ck >= PathList.Count) {
				SW = false;
				OverPath ();
			}
			else {
				GotoCount++;
				this.gameObject.GetComponent<AI> ().LookAtPlayer2D (PathList[GotoCount]);
			}
		}
	}
	
	void Update () {
		if (SW) Move ();
	}
}
