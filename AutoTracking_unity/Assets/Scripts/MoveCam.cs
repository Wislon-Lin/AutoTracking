using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCam : MonoBehaviour {
	public Transform Target;
	public float MoveSpeed = 1.0f;
	public float Dic = 0.1f;
	
	// Update is called once per frame
	void Update () {
		if (Vector2.Distance (Target.position,transform.position) > Dic)
        {
			gameObject.transform.localPosition = Vector2.Lerp(
				transform.position, Target.position, MoveSpeed * Time.deltaTime);
			gameObject.transform.localPosition = new Vector3 (
				gameObject.transform.localPosition.x,
				gameObject.transform.localPosition.y,
				-10
			);
		}
	}
}