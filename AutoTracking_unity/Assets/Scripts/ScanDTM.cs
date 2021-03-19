using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanDTM : MonoBehaviour 
{
	public float DTMSize = 6f;
	public float DTMResolution = 0.2f;
	public Transform GizmoPath;
	public GameObject Gizmo;




	public void StartScanDTM () {
		float DTMStartX = - (DTMSize / 2);
		float DTMStartY = DTMSize / 2;
		int count = 0;
		for (float x = DTMStartX; x <= DTMSize / 2; x+=DTMResolution) 
		{
			for (float y = DTMStartY; y >= - (DTMSize / 2); y-=DTMResolution) {
				Vector3 ori = new Vector3 (x,y,5);
				RaycastHit2D ObjHit = Physics2D.Raycast(ori, Vector3.forward);
				if (ObjHit.collider && ObjHit.transform.name.Contains ("Floor")) {
					GameObject newP = Instantiate (Gizmo,GizmoPath);
					newP.transform.position = new Vector2 (x,y);
					newP.name = "Path_" + count;
					count++;
					newP.SetActive (true);
				}
			}
		}
	}
}
