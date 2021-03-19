using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeGetTarget : MonoBehaviour {
	public float LookDic = 1.0f;
	public float LookAngle = 30.0f;
	bool SW = false;
	
	public void SetEye (float eyedic, float a) {
		LookDic = eyedic;
		LookAngle = a;
		SW = true;
	}

	void Update () {
		if (SW) SetScope ();
	}

	void SetScope () {
		Vector2 v2Dir = Vector2.up;
		Quaternion rota = transform.rotation;
		//Vector2 dic = (StartPoint.position + (rota * v2Dir) * LookDic);
		Quaternion up = Quaternion.Euler (transform.rotation.eulerAngles.x,transform.rotation.eulerAngles.y,transform.rotation.eulerAngles.z - LookAngle);
		Quaternion down = Quaternion.Euler (transform.rotation.eulerAngles.x,transform.rotation.eulerAngles.y,transform.rotation.eulerAngles.z + LookAngle);
		Vector2 pointUp =  (transform.position  + (up * v2Dir) * LookDic);
		Vector2 pointDown =  (transform.position  + (down * v2Dir) * LookDic);
		Debug.DrawLine (transform.position,pointUp,Color.red);
		Debug.DrawLine (transform.position,pointDown,Color.red);
		Debug.DrawLine (pointUp,pointDown,Color.red);
		Transform player = Sys.GetPlayer ();
		if (isINTriangle(player.position,transform.position,pointUp,pointDown)) {
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

	private float triangleArea (float v0x,float v0y,float v1x,float v1y,float v2x,float v2y) {
        return Mathf.Abs((v0x * v1y + v1x * v2y + v2x * v0y - v1x * v0y - v2x * v1y - v0x * v2y) / 2f);
    }
 
	bool isINTriangle (Vector2 point,Vector2 v0,Vector2 v1,Vector2 v2) {
 		float t = triangleArea (v0.x,v0.y,v1.x,v1.y,v2.x,v2.y);
		float a = 
		triangleArea (v0.x,v0.y,v1.x,v1.y,point.x,point.y) + 
		triangleArea (v0.x,v0.y,point.x,point.y,v2.x,v2.y) + 
		triangleArea (point.x,point.y,v1.x,v1.y,v2.x,v2.y);
		if (Mathf.Abs(t - a) <= 0.01f) {return true;}
		else {return false;}
	}
}
