using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public RectTransform mr;
	public GameObject target;
	public float speed = 2.5f;
	float newspeed = 0.0f;
	private Vector3 direction;
	private Coroutine cououtine;
	public bool Hide = false;

	public void SetHideType (bool sw) 
	{
		if (sw == Hide) {return;}
		Hide = sw;
	}

	private IEnumerator Move()
	{
		while(true)
		{
			float moveSizeX = Mathf.Abs (this.mr.anchoredPosition.x);
			float moveSizeY = Mathf.Abs (this.mr.anchoredPosition.y);
			this.newspeed = this.speed / 100f;
			this.newspeed = ((newspeed * moveSizeX) + (newspeed * moveSizeY)) / 2;
			if (newspeed > speed) newspeed = 0 + speed;
			this.target.transform.position += this.direction * Time.deltaTime * this.newspeed;
			yield return null;
		}
	}

	public float GetPlayerMoveSpeed () {return newspeed;}

	public void BeginMove(){
		this.cououtine = StartCoroutine(this.Move());
	}

	public void EndMove(){
		this.newspeed = 0.0f;
		StopCoroutine(this.cououtine);
	}

	public void UpdateDirection(Vector3 direction){
		this.direction = direction;
	}
}