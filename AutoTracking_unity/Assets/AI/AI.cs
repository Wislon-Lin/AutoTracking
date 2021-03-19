using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour {
	public enum PerceptionMode {Look,Listen,Circle}
	public PerceptionMode SetPerception = PerceptionMode.Look;
	public float AwareDic = 1.0f;
	public float LookAngle = 30.0f;
	public float ListenPower = 1.0f;
	public enum MoveMode {Stop,Patrol}
	public MoveMode SetMove = MoveMode.Stop;
	Quaternion OriRota = new Quaternion ();
	float OriZAngle = 0;
	bool LookAngleType = false;
	public int SwingHead = 15;
	public float RotaSpeed = 5f;
	bool LookRotaSW = false;
	bool LookAtSW = false;
	Vector2 StartPos = new Vector2 ();
	List<Vector2> PatrolPathList = new List<Vector2> ();
	bool PatrolType = false;
	int PathCount = 0;
	public AI[] MonGroup;
	public bool isCall = false;

	void Start () {
		StartPos = this.transform.position;
		SetPerceptionMode ();
		SetMoveMode ();
	}

	void SetMoveMode () {
		switch (SetMove) {
			case MoveMode.Stop:
				OriZAngle = transform.rotation.eulerAngles.z;
				OriRota = transform.rotation;
				LookRotaSW = true;
			break;
			case MoveMode.Patrol:
				PatrolPathList.Add (StartPos);
				if (this.transform.childCount > 0) {
					for (int i = 0; i < this.transform.childCount; i++) {
						PatrolPathList.Add (this.transform.GetChild (i).position);
					}
				}
				GotoNextPath ();
			break;
			default:
			break;
		}
	}

	public void CallGroup () {
		if (MonGroup.Length == 0) return;
		for (int i = 0; i < MonGroup.Length; i++) {
			if (MonGroup[i].isCall) continue;
			MonGroup[i].isCall = true;
			MonGroup[i].SearchGoto (transform.position);
		}
	}

	public void SearchGoto (Vector3 p) {
		switch (SetMove) {
			case MoveMode.Stop:
				LookRotaSW = false;
			break;
			case MoveMode.Patrol:
				
			break;
			default:
			break;
		}
		if (!this.gameObject.GetComponent<SearchGotoPath> ()) {
			SearchGotoPath g = this.gameObject.AddComponent<SearchGotoPath> ();
			g.SetScarch (p);
		}
	}

	public void SearchGotoOver () {
		if (this.gameObject.GetComponent<SearchGotoPath> ()) {
			DestroyImmediate (this.gameObject.GetComponent<SearchGotoPath> ());
			if (isCall) {
				isCall = false;
				switch (SetMove) {
					case MoveMode.Stop:
						if (!LookAtSW) {
							SearchGotoPath g = this.gameObject.AddComponent<SearchGotoPath> ();
							g.SetScarch (StartPos);
						}
					break;
					case MoveMode.Patrol:

					break;
					default:
					break;
				}
			}
			else {
				transform.rotation = OriRota;
				switch (SetMove) {
					case MoveMode.Stop:
						LookRotaSW = true;
					break;
					case MoveMode.Patrol:
						GotoNextPath ();
					break;
					default:
					break;
				}
			}
		}
	}

	public void GoBackStopLook () {
		if (this.gameObject.GetComponent<GotoPath> ()) {
			DestroyImmediate (this.gameObject.GetComponent<GotoPath> ());
		}
		transform.rotation = OriRota;
		LookRotaSW = true;
	}

	public void CheckCanNextPath () {
		if (this.gameObject.GetComponent<GotoPath> ()) {
			DestroyImmediate (this.gameObject.GetComponent<GotoPath> ());
		}
		if (!LookAtSW) {
			GotoNextPath ();
		}
	}

	void GotoNextPath () {
		Vector3 t = new Vector3 ();
		if (PatrolType) {
			int ck = PathCount - 1;
			if (ck < 0) {
				PathCount++;
				PatrolType = false;
			}
			else {
				PathCount--;
			}
			t = PatrolPathList[PathCount];
		}
		else {
			int ck = PathCount + 1;
			if (ck >= PatrolPathList.Count) {
				PathCount--;
				PatrolType = true;
			}
			else {
				PathCount++;
			}
			t = PatrolPathList[PathCount];
		}
		LookAtPlayer2D (t);
		GotoPath g = this.gameObject.AddComponent<GotoPath> ();
		g.SetPath (t);
	}

	void AutoRotaLook () {
		if (LookAngleType) {
			if (transform.rotation.eulerAngles.z < OriZAngle + SwingHead) {
				transform.Rotate(Vector3.forward, RotaSpeed * Time.deltaTime);
			} else {LookAngleType = false;}
		}
		else {
			if (transform.rotation.eulerAngles.z > OriZAngle - SwingHead) {
				transform.Rotate(Vector3.forward, -RotaSpeed * Time.deltaTime);
			} else {LookAngleType = true;}
		}
	}

	void SetPerceptionMode () {
		switch (SetPerception) {
			case PerceptionMode.Look:
				EyeGetTarget eye = this.gameObject.AddComponent<EyeGetTarget> ();
				eye.SetEye (AwareDic,LookAngle);
			break;
			case PerceptionMode.Listen:
				ListenGetTarget lis = this.gameObject.AddComponent<ListenGetTarget> ();
				lis.SetListenSize (AwareDic,ListenPower);
			break;
			case PerceptionMode.Circle:
				CircleGetTarget cir = this.gameObject.AddComponent<CircleGetTarget> ();
				cir.SetCircleSize (AwareDic);
			break;
			default:
			break;
		}
	}
	
	void LookAtPlayer2D () {
		LookAtPlayer2D (Sys.GetPlayer ().position);
	}

	public void LookAtPlayer2D (Vector3 t) {
		Vector2 dir = t - transform.position;
		float angle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg - 90;
		transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);
	}

	void Update () {
		if (LookAtSW) LookAtPlayer2D ();
		if (LookRotaSW) AutoRotaLook ();
	}
	
	public void SetLookAt () {
		if (LookAtSW) return;
		LookAtSW = true;
		switch (SetMove) {
			case MoveMode.Stop:
				LookRotaSW = false;
			break;
			case MoveMode.Patrol:
				CheckCanNextPath ();
			break;
			default:
			break;
		}
		CallGroup ();
		this.gameObject.AddComponent<CatchPlayer> ();
	}

	public void SetNotLookAt () {
		if (!LookAtSW) return;
		LookAtSW = false;
		if (this.gameObject.GetComponent<CatchPlayer> ()) {
			DestroyImmediate (this.gameObject.GetComponent<CatchPlayer> ());
		}
		switch (SetMove) {
			case MoveMode.Stop:
				if (!this.gameObject.GetComponent<SearchGotoPath> ()) {
					SearchGotoPath g = this.gameObject.AddComponent<SearchGotoPath> ();
					g.SetScarch (StartPos);
				}
			break;
			case MoveMode.Patrol:
				CheckCanNextPath ();
			break;
			default:
			break;
		}
	}
}