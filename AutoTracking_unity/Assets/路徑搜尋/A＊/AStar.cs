using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour 
{
	public Transform StartPos;
	public Transform EndPos;
	public PathData NowPath = new PathData ();
	public List<PathData> OpenList = new List<PathData> ();
	public List<PathData> CloseList = new List<PathData> ();
	Vector3 StartPosV3 = new Vector3 ();
	Vector3 EndPosV3 = new Vector3 ();

	public List<Vector3> SetStartAndEndPos (Vector3 s, Vector3 e) {
		StartPosV3 = s;
		EndPosV3 = e;
		StartPos = V3ToPos (StartPosV3);
		EndPos = V3ToPos (EndPosV3);
		return StartSearch ();
	}

	Transform V3ToPos (Vector3 v) {
		Transform plist = Sys.GetPathList ();
		float dic = Sys.GetSys.SendToScanDTM.DTMResolution + 0.1f; //每個節點多0.1f;
		List<Transform> tmpList = new List<Transform> ();
		for (int i = 0; i < plist.childCount; i++)//搜尋周圍節點 
		{
			Transform aroundPath = plist.GetChild (i);
				if (Vector2.Distance (v,aroundPath.localPosition) < dic) 
			{
				tmpList.Add (aroundPath);
			}
		}
		int useID = 0;
		for (int p = 0; p < tmpList.Count; p++) {
			if (p == 0) continue;
			else {
				if (Vector2.Distance (v,tmpList[p].localPosition) < 
					Vector2.Distance (v,tmpList[useID].localPosition)) {
					useID = p;
				}
			}
		}
		return tmpList[useID];
	}

	List<Vector3> StartSearch ()  //路徑搜尋
	{
		PathColorClear ();
		OpenList.Clear ();
		CloseList.Clear (); 
		NowPath = new PathData (); //站在哪一個
		PathData newP = new PathData ();
		newP.Path = StartPos;
		newP.PathVar = 0;
		OpenList.Add (newP);
		while (OpenList.Count > 0) //反覆搜尋路徑
		{
			NowPath = GetBasePath ();
			if (NowPath.Path.name == EndPos.name) 
			{
				//Debug.Log ("Over");
				return GetGotoPath ();
			}
			else 
			{
				CloseList.Add (NowPath);
				RemovePathFormOpenList (NowPath);
				GetTargetPath ();
			}
		}
		return null;
	}

	PathData GetBasePath ()  //搜尋下一個節點
	{
		PathData newP = new PathData ();
		for (int i = 0; i < OpenList.Count; i++) 
		{
			if (i == 0) newP = OpenList[i];
			else {
				if (newP.PathVar > OpenList[i].PathVar) 
				{
					newP = OpenList[i];
				}
			}
		}
		return newP;
	}

	void GetTargetPath () //計算哪一個節點最好
	{
		Transform plist = Sys.GetPathList ();  //抓所有節點
		float dic = Sys.GetSys.SendToScanDTM.DTMResolution + 0.1f; //抓距離
		for (int i = 0; i < plist.childCount; i++) {
			Transform aroundPath = plist.GetChild (i);
			if (Vector2.Distance (NowPath.Path.localPosition,aroundPath.localPosition) < dic) {
				if (!CheckPathInCloseList (aroundPath)) //當節點沒有被偵測過 避免重複偵測
				{continue;}
				else if (!CheckPathInOpenList (aroundPath)) {
					float gV = Vector2.Distance (StartPos.localPosition,aroundPath.localPosition); //開始結點位置
					float hV = Vector2.Distance (EndPos.localPosition,aroundPath.localPosition);   //
					PathData newP = new PathData ();
					newP.Path = aroundPath;
					newP.PathVar = gV + hV;
					newP.Pev = NowPath.Path.name;
					OpenList.Add (newP);
				}
			}
		}
	}

	void RemovePathFormOpenList (PathData t) {
		int id = 0;
		for (int i = 0; i < OpenList.Count; i++) {
			if (OpenList[i].Path.name == t.Path.name) {
				id = i;
				break;
			}
		}
		OpenList.RemoveAt (id);
	}

	bool CheckPathInOpenList (Transform t) {
		if (OpenList.Count == 0) return false;
		for (int i = 0; i < OpenList.Count; i++) {
			if (t.name == OpenList[i].Path.name) {
				return true;
			}
		}
		return false;
	}

	bool CheckPathInCloseList (Transform t) {
		if (CloseList.Count == 0) return true;
		for (int i = 0; i < CloseList.Count; i++) {
			if (t.name == CloseList[i].Path.name) {
				return false;
			}
		}
		return true;
	}

	PathData GetPathFormCloseList (string n)
	{
		PathData p = new PathData ();
		for (int i = 0; i < CloseList.Count; i++) {
			if (CloseList[i].Path.name == n) {
				p = CloseList[i];
				return p;
			}
		}
		return null;
	}

	List<Vector3> GetGotoPath ()
	{
		List<Vector3> getPathList = new List<Vector3> ();
		getPathList.Add (NowPath.Path.localPosition);
		SetPathColor (NowPath.Path.name);
		string pev = NowPath.Pev;
		while (pev != "") 
		{
			PathData p = new PathData ();
			p = GetPathFormCloseList (pev);
			getPathList.Add (p.Path.localPosition);
			SetPathColor (pev);
			pev = p.Pev;
		}
		getPathList.Add (StartPosV3);
		getPathList.Reverse ();
		getPathList.Add (EndPosV3);
		return getPathList;
	}

	void PathColorClear () //把所有顏色回到橘色
	{
		Transform plist = Sys.GetPathList ();
		for (int i = 0; i < plist.childCount; i++) {
			plist.GetChild (i).GetComponent<SpriteRenderer> ().color = Color.yellow;
		}
	}

	void SetPathColor (string n)//把節點改成紅色
	{
		Transform plist = Sys.GetPathList ();
		for (int i = 0; i < plist.childCount; i++) {
			if (plist.GetChild (i).name == n) {
				plist.GetChild (i).GetComponent<SpriteRenderer> ().color = Color.red;
				return;
			}
		}
	}
}

[System.Serializable] //容器
public class PathData 
{
	public Transform Path;//節點
	public float PathVar = 0.0f;//F=G+H(節點最佳數值)
	public string Pev = "";//上一層
}